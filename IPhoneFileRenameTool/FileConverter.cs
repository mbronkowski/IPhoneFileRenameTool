using ImageMagick;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;
using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Runtime.Serialization;

namespace IPhoneFileRenameTool
{
    internal class FileConverter
    {
        protected string FolderPath { get; set; }
        protected string FilePostfix { get; set; }
        protected string FileFormat { get; set; }
        protected bool ConvertHcifToJpg { get; set; }
        protected double DateHourOffset { get; set; }
        protected bool Recursive { get; set; }
        protected bool DeleteAaeFiles { get; set; }
        private string _currentFilePath;
        private ConversionResult ConversionResult { get; set; } = new ConversionResult();

        public static FileConverter Create(string folderPath, string filePostfix, string fileFormat, bool convertHcifToJpg, double dateHourOffset, bool recursive = false, bool deleteAaeFiles = false)
        {
            var fileConverter = new FileConverter();
            fileConverter.FolderPath = folderPath;
            fileConverter.FilePostfix = filePostfix;
            fileConverter.FileFormat = fileFormat;
            fileConverter.ConvertHcifToJpg = convertHcifToJpg;
            fileConverter.DateHourOffset = dateHourOffset;
            fileConverter.Recursive = recursive;
            fileConverter.DeleteAaeFiles = deleteAaeFiles;
            return fileConverter;
        }

        public async Task<ConversionResult> RenameAndConvertAsync(
            IProgress<(int current, int total, string fileName)>? progress,
            CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                ConversionResult = new ConversionResult();
                var searchOption = Recursive
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly;
                var files = System.IO.Directory.GetFiles(FolderPath, "*", searchOption);
                int total = files.Length;
                int current = 0;

                foreach (var file in files)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        ConversionResult.SetCancelled();
                        break;
                    }
                    _currentFilePath = file;
                    progress?.Report((++current, total, Path.GetFileName(file)));
                    try
                    {
                        ConversionResult.IncrementProcessedFiles();
                        if (!IsValidMediaFile()) { ConversionResult.IncrementConversionSkipped(); continue; }
                        if (ShouldBeConverted()) ConvertHeifToJpg(file);
                        else ConversionResult.IncrementConversionSkipped();
                        RenameFile();
                    }
                    catch (Exception ex) { ConversionResult.IncrementErrors(); Log.Error(ex, "error"); }
                }
                return ConversionResult;
            }, cancellationToken);
        }

        private bool IsValidMediaFile()
        {
            string extension = Path.GetExtension(_currentFilePath).ToUpperInvariant();
            return extension == ".HEIC" || extension == ".JPEG" || extension == ".JPG" || extension == ".MOV" || extension == ".MP4";
        }

        private void RenameFile()
        {
            var dateTeken = GetMediaFileCreationTime(_currentFilePath);
            if (dateTeken != null)
            {
                if (!FileAlreadyHasCorrectName(dateTeken.Value))
                {
                    string newFilePath = GetFinalAvaliableCorrectFilePath(dateTeken.Value);
                    File.Move(_currentFilePath, newFilePath);
                    ConversionResult.IncrementRenamedFiles();
                    Log.Information($"Renamed {_currentFilePath} to {newFilePath}");
                }
                else
                {
                    ConversionResult.IncrementRenameSkipped();
                }
            }
            else
            {
                ConversionResult.AddFailedFile(_currentFilePath);
            }
        }

        private string GetFinalAvaliableCorrectFilePath(DateTime dateTeken)
        {
            string correctFileName = GetCorrectFileName(dateTeken);
            string extension = Path.GetExtension(_currentFilePath);
            string fileDirectory = Path.GetDirectoryName(_currentFilePath)!;
            string newFilePathBase = Path.Combine(fileDirectory, correctFileName);
            string newFilePathWithoutExtension = newFilePathBase;
            int i = 1;
            while (File.Exists(newFilePathWithoutExtension + extension))
            {
                newFilePathWithoutExtension = newFilePathBase + $"_{i++}";
            }
            return newFilePathWithoutExtension + extension;
        }

        private bool FileAlreadyHasCorrectName(DateTime dateTeken)
        {
            string correctFileName = GetCorrectFileName(dateTeken);
            return Path.GetFileNameWithoutExtension(_currentFilePath).StartsWith(correctFileName);
        }

        private string GetCorrectFileName(DateTime dateTeken)
        {
            return dateTeken.AddHours(DateHourOffset).ToString(FileFormat) + FilePostfix;
        }

        private bool ShouldBeConverted()
        {
            return ConvertHcifToJpg && Path.GetExtension(_currentFilePath).ToUpperInvariant() == ".HEIC";
        }

        private void ConvertHeifToJpg(string heifFilePath)
        {
            string jpgFilePath = Path.ChangeExtension(_currentFilePath, ".JPG");
            using (var image = new MagickImage(heifFilePath))
            {
                image.Write(jpgFilePath);
                _currentFilePath = jpgFilePath;
                File.Delete(heifFilePath);
                ConversionResult.IncrementConvertedFiles();
            }
            if (DeleteAaeFiles)
            {
                string aaePath = Path.ChangeExtension(heifFilePath, ".AAE");
                if (File.Exists(aaePath))
                {
                    File.Delete(aaePath);
                    ConversionResult.IncrementDeletedAaeFiles();
                    Log.Information($"Deleted AAE sidecar: {aaePath}");
                }
            }
        }

        private DateTime? GetMediaFileCreationTime(string filePath)
        {
            try
            {
                var directories = ImageMetadataReader.ReadMetadata(filePath);
                if (filePath.EndsWith(".MOV", StringComparison.OrdinalIgnoreCase) || filePath.EndsWith(".MP4", StringComparison.OrdinalIgnoreCase))
                {
                    QuickTimeMetadataHeaderDirectory? directory = directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
                    if (directory != null && directory.ContainsTag((int)QuickTimeMetadataHeaderDirectory.TagCreationDate))
                    {
                        return directory.GetDateTime((int)QuickTimeMetadataHeaderDirectory.TagCreationDate);
                    }
                    else
                    {
                        QuickTimeMovieHeaderDirectory? directory2 = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault();
                        if (directory2 != null)
                        {
                            return directory2.GetDateTime((int)QuickTimeMovieHeaderDirectory.TagCreated);
                        }
                    }
                }
                else if (filePath.EndsWith(".JPG", StringComparison.OrdinalIgnoreCase) || filePath.EndsWith(".HEIC", StringComparison.OrdinalIgnoreCase))
                {
                    ExifSubIfdDirectory? directory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                    if (directory != null)
                    {
                        return directory.GetDateTime((int)ExifSubIfdDirectory.TagDateTimeOriginal);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                ConversionResult.IncrementErrors();
                Log.Error(ex, "Error while extracting creation time");
                return null;
            }
        }
    }
}

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

        private string _currentFilePath;
        private ConversionResult ConversionResult { get; set; } = new ConversionResult();
        public static FileConverter Create(string folderPath, string filePostfix, string fileFormat, bool convertHcifToJpg)
        {
            var fileConverter = new FileConverter();
            fileConverter.FolderPath = folderPath;
            fileConverter.FilePostfix = filePostfix;
            fileConverter.FileFormat = fileFormat;
            fileConverter.ConvertHcifToJpg = convertHcifToJpg; 
            return fileConverter;
        }

        public ConversionResult RenameAndConvert()
        {
            ConversionResult = new ConversionResult();
            var files = System.IO.Directory.GetFiles(FolderPath);
            foreach (var file in files)
            {
                _currentFilePath = file;
                try
                {
                    ConversionResult.IncrementProcessedFiles();
                    if (!IsValidMediaFile())
                    {
                        ConversionResult.IncrementConversionSkipped();
                        continue;
                    }
                    if (ShouldBeConverted())
                    {
                        ConvertHeifToJpg(file);
                    }
                    else
                    {
                        ConversionResult.IncrementConversionSkipped();
                    }
                    RenameFile();
                    
                }
                catch (Exception ex)
                {
                    ConversionResult.IncrementErrors();
                    Log.Error(ex,"error");
                }
            }
            return ConversionResult;
        }

        private bool IsValidMediaFile()
        {
            string extension = Path.GetExtension(_currentFilePath).ToUpper();
            return extension == ".HEIC" || extension == ".JPEG" || extension == ".JPEG" || extension == ".MOV" || extension == ".MP4";
        }

        private void RenameFile()
        {
            var dateTeken = GetMediaFileCreationTime(_currentFilePath);
            if (dateTeken != null)
            {
                if(!FileAlreadyHasCorrectName(dateTeken.Value))
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
                ConversionResult.IncrementFailedToRetriveCreationDate();
            }
        }

        private string GetFinalAvaliableCorrectFilePath(DateTime dateTeken)
        {
            string correctFileName = GetCorrectFileName(dateTeken);
            string extension = Path.GetExtension(_currentFilePath);
            string newFilePathBase = Path.Combine(FolderPath, correctFileName);
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
            return dateTeken.ToString(FileFormat) + FilePostfix;
        }

        private bool ShouldBeConverted()
        {
            return ConvertHcifToJpg && Path.GetExtension(_currentFilePath) == ".HEIC";
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
        }

        private DateTime? GetMediaFileCreationTime(string filePath)
        {
            try
            {
                // Read all metadata from the video file
                var directories = ImageMetadataReader.ReadMetadata(filePath);
                if (filePath.EndsWith(".MOV") || filePath.EndsWith(".MP4"))
                {
                    QuickTimeMetadataHeaderDirectory? directory = directories.OfType<QuickTimeMetadataHeaderDirectory>().FirstOrDefault();
                    if (directory != null)
                    {
                        return directory.GetDateTime((int)QuickTimeMetadataHeaderDirectory.TagCreationDate);
                    }
                }
                else if (filePath.EndsWith(".JPG") || filePath.EndsWith(".HEIC"))
                {
                    ExifSubIfdDirectory? directory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                    if (directory != null)
                    {
                        return directory.GetDateTime((int)ExifSubIfdDirectory.TagDateTimeOriginal);
                    }
                }
                return null; // If creation time couldn't be extracted
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

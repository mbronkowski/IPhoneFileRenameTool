using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IPhoneFileRenameTool
{
    internal class ConversionResult
    {
        private int ConvertedFiles { get; set; } = 0;
        private int RenamedFiles { get; set; } = 0;
        private int ConversionSkipped { get; set; } = 0;
        private int RenameSkipped { get; set; } = 0;
        private int Errors { get; set; } = 0;
        private int FailedToRetriveCreationDate { get; set; } = 0;
        private int ProcessedFiles { get; set; } = 0;
        private int DeletedAaeFiles { get; set; } = 0;
        private readonly List<string> _failedFiles = new List<string>();

        public void IncrementConvertedFiles() => ConvertedFiles++;
        public void IncrementRenamedFiles() => RenamedFiles++;
        public void IncrementConversionSkipped() => ConversionSkipped++;
        public void IncrementRenameSkipped() => RenameSkipped++;
        public void IncrementErrors() => Errors++;
        public void IncrementProcessedFiles() => ProcessedFiles++;
        public void IncrementDeletedAaeFiles() => DeletedAaeFiles++;

        public void AddFailedFile(string filePath)
        {
            FailedToRetriveCreationDate++;
            _failedFiles.Add(Path.GetFileName(filePath));
        }

        public bool IsCancelled { get; private set; } = false;
        public void SetCancelled() => IsCancelled = true;

        public override string ToString()
        {
            string prefix = IsCancelled ? "*** Operacja anulowana ***" + Environment.NewLine : "";
            var sb = new StringBuilder(prefix);
            sb.AppendLine($"Processed {ProcessedFiles} files");
            sb.AppendLine($"Renamed {RenamedFiles} files");
            sb.AppendLine($"Converted {ConvertedFiles} files");
            if (DeletedAaeFiles > 0)
                sb.AppendLine($"Deleted {DeletedAaeFiles} AAE sidecar files");
            sb.AppendLine($"Skipped conversion of {ConversionSkipped} files");
            sb.AppendLine($"Skipped renaming of {RenameSkipped} files");
            sb.AppendLine($"Failed to retrieve creation date of {FailedToRetriveCreationDate} files");
            if (_failedFiles.Count > 0)
            {
                foreach (var f in _failedFiles.Take(10))
                    sb.AppendLine("  - " + f);
                if (_failedFiles.Count > 10)
                    sb.AppendLine($"  ... i {_failedFiles.Count - 10} więcej");
            }
            sb.AppendLine($"Encountered {Errors} errors");
            return sb.ToString();
        }
    }
}

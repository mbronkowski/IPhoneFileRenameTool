using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void IncrementConvertedFiles() => ConvertedFiles++;
        public void IncrementRenamedFiles() => RenamedFiles++;
        public void IncrementConversionSkipped() => ConversionSkipped++;
        public void IncrementRenameSkipped() => RenameSkipped++;
        public void IncrementErrors() => Errors++;
        public void IncrementProcessedFiles() => ProcessedFiles++;
        public void IncrementFailedToRetriveCreationDate() => FailedToRetriveCreationDate++;

        public override string ToString()
        {
            return $"Processed {ProcessedFiles} files{Environment.NewLine}" +
                $"Renamed {RenamedFiles} files{Environment.NewLine}" +
                $"Converted {ConvertedFiles} files{Environment.NewLine}" +
                $"Skipped conversion of {ConversionSkipped} files{Environment.NewLine}" +
                $"Skipped renaming of {RenameSkipped} files{Environment.NewLine}" +
                $"Failed to retrive creation date of {FailedToRetriveCreationDate} files{Environment.NewLine}" +
                $"Encountered {Errors} errors{Environment.NewLine}";
        }

    }
}

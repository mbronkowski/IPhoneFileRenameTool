# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run

```bash
# Build
dotnet build IPhoneFileRenameTool.sln

# Run (WinForms app — requires Windows)
dotnet run --project IPhoneFileRenameTool/IPhoneFileRenameTool.csproj

# Run with a pre-selected folder
IPhoneFileRenameTool.exe "C:\path\to\media\folder"
```

There are no automated tests in this project.

## Architecture

This is a .NET 7.0 WinForms app (Windows-only) that renames iPhone media files using embedded metadata and optionally converts HEIC images to JPEG.

### Data Flow

1. **Program.cs** — Initializes Serilog (logs to `{folderPath}/log.txt`), creates the main form (optionally with a pre-selected folder from CLI args), and launches the WinForms application.

2. **IPhoneFileRanameForm.cs** — Main UI. On "Rename" click, reads form values, persists settings to the Windows Registry (`HKEY_CURRENT_USER\SOFTWARE\EdsSoftware\IphoneFileReranameTool`), creates a `FileConverter`, calls `RenameAndConvert()`, and displays the result string. Also handles "Register file explorer action" to add a Windows Explorer right-click context menu entry via Registry.

3. **FileConverter.cs** — All file processing logic. Created via `FileConverter.Create(folderPath, postfix, format, convertHeic, hourOffset)`.
   - `RenameAndConvert()` iterates files, validates extension, optionally converts HEIC→JPG (via ImageMagick), then renames each file.
   - `GetMediaFileCreationTime()` extracts date from metadata: videos use `QuickTimeMetadataHeaderDirectory`/`QuickTimeMovieHeaderDirectory`; images use `ExifSubIfdDirectory.TagDateTimeOriginal`.
   - `RenameFile()` applies the hour offset, formats the name as `{dateFormat}{postfix}.{ext}`, and handles collisions by appending `_1`, `_2`, etc.

4. **ConversionResult.cs** — Simple counter bag tracking renamed, converted, skipped, failed, and error counts. Has a `ToString()` that formats the summary message shown to the user.

### Supported File Types
`.HEIC`, `.JPEG`, `.JPG`, `.MOV`, `.MP4` (case-insensitive)

### Key Dependencies
- **Magick.NET-Q16-AnyCPU** — HEIC-to-JPEG conversion
- **MetadataExtractor** — EXIF/QuickTime metadata reading
- **Serilog + Serilog.Sinks.File** — File logging

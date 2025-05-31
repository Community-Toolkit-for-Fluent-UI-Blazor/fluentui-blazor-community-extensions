// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

internal sealed class FileExtensionTypeProvider
{
    private readonly Dictionary<string, string> _mappedExtensions;
    private readonly string _unknownValue;

    public FileExtensionTypeProvider(FileExtensionTypeLabels? fileExtensionTypeLabels)
    {
        ArgumentNullException.ThrowIfNull(fileExtensionTypeLabels, nameof(fileExtensionTypeLabels));
        _mappedExtensions = new()
        {
            [".aac"] = fileExtensionTypeLabels.WindowsAudioFile,
            [".adt"] = fileExtensionTypeLabels.WindowsAudioFile,
            [".adts"] = fileExtensionTypeLabels.WindowsAudioFile,
            [".accdb"] = fileExtensionTypeLabels.MicrosoftAccessDatabaseFile,
            [".accde"] = fileExtensionTypeLabels.MicrosoftAccessExecuteOnlyFile,
            [".accdr"] = fileExtensionTypeLabels.MicrosoftAccessRuntimeDatabase,
            [".accdt"] = fileExtensionTypeLabels.MicrosoftAccessDatabaseTemplate,
            [".aif"] = fileExtensionTypeLabels.AudioInterchangeFileFormatFile,
            [".aifc"] = fileExtensionTypeLabels.AudioInterchangeFileFormatFile,
            [".aiff"] = fileExtensionTypeLabels.AudioInterchangeFileFormatFile,
            [".aspx"] = fileExtensionTypeLabels.AspNetActiveServerPage,
            [".asp"] = fileExtensionTypeLabels.AspFile,
            [".avi"] = fileExtensionTypeLabels.AudioVideoInterleaveFile,
            [".bat"] = fileExtensionTypeLabels.BatchFile,
            [".bin"] = fileExtensionTypeLabels.BinaryFile,
            [".bmp"] = fileExtensionTypeLabels.BitmapFile,
            [".cab"] = fileExtensionTypeLabels.Cab,
            [".cda"] = fileExtensionTypeLabels.Cda,
            [".csv"] = fileExtensionTypeLabels.Csv,
            [".dif"] = fileExtensionTypeLabels.Dif,
            [".dll"] = fileExtensionTypeLabels.Dll,
            [".doc"] = fileExtensionTypeLabels.MicrosoftWordDocument,
            [".docm"] = fileExtensionTypeLabels.MicrosoftWordMacroEnabledDocument,
            [".docx"] = fileExtensionTypeLabels.MicrosoftWordDocument,
            [".dot"] = fileExtensionTypeLabels.MicrosoftWordTemplate,
            [".dotx"] = fileExtensionTypeLabels.MicrosoftWordTemplate,
            [".eml"] = fileExtensionTypeLabels.Eml,
            [".eps"] = fileExtensionTypeLabels.Eps,
            [".exe"] = fileExtensionTypeLabels.Exe,
            [".flv"] = fileExtensionTypeLabels.Flv,
            [".gif"] = fileExtensionTypeLabels.Gif,
            [".html"] = fileExtensionTypeLabels.Html,
            [".htm"] = fileExtensionTypeLabels.Html,
            [".ini"] = fileExtensionTypeLabels.Ini,
            [".iso"] = fileExtensionTypeLabels.Iso,
            [".jar"] = fileExtensionTypeLabels.Jar,
            [".jpg"] = fileExtensionTypeLabels.Jpg,
            [".jpeg"] = fileExtensionTypeLabels.Jpeg,
            [".m4a"] = fileExtensionTypeLabels.M4a,
            [".mdb"] = fileExtensionTypeLabels.Mdb,
            [".mid"] = fileExtensionTypeLabels.Midi,
            [".midi"] = fileExtensionTypeLabels.Midi,
            [".mov"] = fileExtensionTypeLabels.Mov,
            [".mp3"] = fileExtensionTypeLabels.Mp3,
            [".mp4"] = fileExtensionTypeLabels.Mp4,
            [".mpeg"] = fileExtensionTypeLabels.Mpeg,
            [".mpg"] = fileExtensionTypeLabels.Mpg,
            [".msi"] = fileExtensionTypeLabels.Msi,
            [".mui"] = fileExtensionTypeLabels.Mui,
            [".pdf"] = fileExtensionTypeLabels.Pdf,
            [".png"] = fileExtensionTypeLabels.Png,
            [".pot"] = fileExtensionTypeLabels.MicrosoftPowerPointTemplate,
            [".potm"] = fileExtensionTypeLabels.MicrosoftPowerPointMacroEnabled,
            [".potx"] = fileExtensionTypeLabels.MicrosoftPowerPointTemplate,
            [".ppam"] = fileExtensionTypeLabels.MicrosoftPowerPointAddIn,
            [".pps"] = fileExtensionTypeLabels.MicrosoftPowerPointSlide,
            [".ppsm"] = fileExtensionTypeLabels.MicrosoftPowerPointMacroEnabled,
            [".ppsx"] = fileExtensionTypeLabels.MicrosoftPowerPointSlide,
            [".ppt"] = fileExtensionTypeLabels.MicrosoftPowerPointDocument,
            [".pptm"] = fileExtensionTypeLabels.MicrosoftPowerPointMacroEnabled,
            [".pptx"] = fileExtensionTypeLabels.MicrosoftPowerPointDocument,
            [".psd"] = fileExtensionTypeLabels.AdobePhotoshopFile,
            [".pst"] = fileExtensionTypeLabels.OutlookDataStore,
            [".pub"] = fileExtensionTypeLabels.MicrosoftPublisherFile,
            [".rar"] = fileExtensionTypeLabels.ArchiveCompressedFile,
            [".7zip"] = fileExtensionTypeLabels.ArchiveCompressedFile,
            [".rtf"] = fileExtensionTypeLabels.RichTextFormatFile,
            [".sldm"] = fileExtensionTypeLabels.MicrosoftPowerPointMacroEnabled,
            [".sldx"] = fileExtensionTypeLabels.MicrosoftPowerPointSlide,
            [".swf"] = fileExtensionTypeLabels.ShockwaveFlashFile,
            [".sys"] = fileExtensionTypeLabels.MicrosoftWindowsSystemSettings,
            [".tif"] = fileExtensionTypeLabels.TaggedImageFormatFile,
            [".tiff"] = fileExtensionTypeLabels.TaggedImageFormatFile,
            [".tmp"] = fileExtensionTypeLabels.TemporaryDataFile,
            [".txt"] = fileExtensionTypeLabels.TextDocument,
            [".vob"] = fileExtensionTypeLabels.VideoObjectFile,
            [".vsd"] = fileExtensionTypeLabels.MicrosoftVisio,
            [".vsdm"] = fileExtensionTypeLabels.MicrosoftVisioMacroEnabled,
            [".vsdx"] = fileExtensionTypeLabels.MicrosoftVisioDrawing,
            [".vss"] = fileExtensionTypeLabels.MicrosoftVisio,
            [".vssm"] = fileExtensionTypeLabels.MicrosoftVisioMacroEnabled,
            [".vst"] = fileExtensionTypeLabels.MicrosoftVisioTemplate,
            [".vstm"] = fileExtensionTypeLabels.MicrosoftVisioMacroEnabled,
            [".vstx"] = fileExtensionTypeLabels.MicrosoftVisioTemplate,
            [".wav"] = fileExtensionTypeLabels.WaveAudioFile,
            [".wbk"] = fileExtensionTypeLabels.MicrosoftWordDocument,
            [".wks"] = fileExtensionTypeLabels.MicrosoftWorksFile,
            [".wma"] = fileExtensionTypeLabels.WindowsMediaAudioFile,
            [".wmd"] = fileExtensionTypeLabels.WindowsMediaDownloadFile,
            [".wmv"] = fileExtensionTypeLabels.WindowsMediaVideoFile,
            [".wmz"] = fileExtensionTypeLabels.WindowsMediaSkinsFile,
            [".wms"] = fileExtensionTypeLabels.WindowsMediaSkinsFile,
            [".wpd"] = fileExtensionTypeLabels.WordPerfectDocument,
            [".wp5"] = fileExtensionTypeLabels.WordPerfectDocument,
            [".xla"] = fileExtensionTypeLabels.MicrosoftExcelAddIn,
            [".xlam"] = fileExtensionTypeLabels.MicrosoftExcelAddIn,
            [".xll"] = fileExtensionTypeLabels.MicrosoftExcelDllAddIn,
            [".xlm"] = fileExtensionTypeLabels.MicrosoftExcelMacroEnabled,
            [".xlsm"] = fileExtensionTypeLabels.MicrosoftExcelMacroEnabled,
            [".xls"] = fileExtensionTypeLabels.MicrosoftExcelWorkbook,
            [".xlsx"] = fileExtensionTypeLabels.MicrosoftExcelWorkbook,
            [".xlt"] = fileExtensionTypeLabels.MicrosoftExcelTemplate,
            [".xltm"] = fileExtensionTypeLabels.MicrosoftExcelTemplate,
            [".xps"] = fileExtensionTypeLabels.Xps,
            [".zip"] = fileExtensionTypeLabels.ArchiveCompressedFile,
            [".pbix"] = fileExtensionTypeLabels.MicrosoftPowerBiDocument,
            [".svg"] = fileExtensionTypeLabels.SvgFile
        };

        _unknownValue = fileExtensionTypeLabels.UnknownValue;
    }

    public string Get(string? extension)
    {
        if (string.IsNullOrEmpty(extension))
        {
            return string.Empty;
        }

        if (_mappedExtensions.TryGetValue(extension.ToLowerInvariant(), out var mappedExtension))
        {
            return mappedExtension;
        }

        return _unknownValue;
    }
}


// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public record FileExtensionTypeLabels
{
    private FileExtensionTypeLabels() { }

    public static FileExtensionTypeLabels Default { get; } = new FileExtensionTypeLabels();

    public static FileExtensionTypeLabels French { get; } = new FileExtensionTypeLabels()
    {
        AdobePhotoshopFile = "Fichier Adobe Photoshop",
        ArchiveCompressedFile = "Fichier d'archivage compressée",
        AspFile = "Fichier Active Server Page",
        AspNetActiveServerPage = "Fichier Active Server Page .NET",
        AudioInterchangeFileFormatFile = "Fichier de format d'Échange audio",
        AudioVideoInterleaveFile = "Fichier Audio Vidéo entrelacé",
        BatchFile = "Fichier Batch",
        BinaryFile = "Fichier Binaire Compressé",
        BitmapFile = "Fichier Bitmap",
        Cab = "Fichier Windows Cabinet",
        Cda = "Fichier Piste Audio CD",
        Csv = "Fichier Csv",
        Dif = "Fichier de format d'échange de données de feuille de calcul",
        Dll = "Fichier de bibliothèque de liens dynamiques",
        Eml = "Courriel",
        Eps = "Fichier Postscript Encapsulé",
        Exe = "Ficher programme exécutable",
        Flv = "Fichier vidéo Flash",
        Gif = "Fichier Gif",
        Html = "Fichier Html",
        Ini = "Fichier de configuration d'initialisation Windows",
        Iso = "Image disque ISO-9660",
        Jar = "Fichier architecture JAVA",
        Jpeg = "Fichier Jpeg",
        Jpg = "Fichier Jpg",
        M4a = "Fichier audio MPEG-4",
        Mdb = "Base de données Microsoft Access",
        MicrosoftAccessDatabaseFile = "Fichier de base de données Microsoft Access",
        MicrosoftAccessDatabaseTemplate = "Template de base de données Microsoft Access",
        MicrosoftAccessExecuteOnlyFile = "Fichier exécutable de Microsoft Access",
        MicrosoftAccessRuntimeDatabase = "Base de données d'exécution Microsoft Access",
        MicrosoftExcelAddIn = "Fichier d'extension Excel de Microsoft",
        MicrosoftExcelDllAddIn = "Complément basé sur DLL de Microsoft Excel",
        MicrosoftExcelDocument = "Document Microsoft Excel",
        MicrosoftExcelMacroEnabled = "Classeur Excel avec macros activées",
        MicrosoftExcelTemplate = "Modèle Microsoft Excel",
        MicrosoftExcelWorkbook = "Classeur Microsoft Excel",
        MicrosoftPowerBiDocument = "Document Microsoft PowerBi",
        MicrosoftPowerPointAddIn = "Module complémentaire Microsoft PowerPoint",
        MicrosoftPowerPointDocument = "Document Microsoft PowerPoint",
        MicrosoftPowerPointMacroEnabled = "Modèle Microsoft PowerPoint avec macros activées",
        MicrosoftPowerPointSlide = "Microsoft PowerPoint Slide",
        MicrosoftPowerPointTemplate = "Modèle Microsoft Powerpoint",
        MicrosoftPublisherFile = "Fichier Microsoft Publisher",
        MicrosoftVisio = "Microsoft Visio",
        MicrosoftVisioDrawing = "Microsoft Visio Drawing",
        MicrosoftVisioMacroEnabled = "Microsoft Visio Drawing avec Macro activées",
        MicrosoftVisioTemplate = "Modèle Microsoft Visio",
        MicrosoftWindowsSystemSettings = "Paramètres système Microsoft Windows",
        MicrosoftWordDocument = "Document Microsoft Word",
        MicrosoftWordMacroEnabledDocument = "Document Microsoft Word avec macros activées",
        MicrosoftWordTemplate = "Modèle Microsoft Word",
        MicrosoftWorksFile = "Fichier Microsoft Works",
        Midi = "Fichier d'Interface Numérique d'Instrument Musical",
        Mov = "Fichier vidéo Apple QuickTime",
        Mp3 = "Fichier audio MPEG 3",
        Mp4 = "Fichier vidéo MPEG 4",
        Mpeg = "Fichier vidéo MPEG",
        Mpg = "Fichier MPEG 1",
        Msi = "Fichier d'installation Microsoft",
        Mui = "Fichier d'interface utilisateur multilingue",
        OutlookDataStore = "Fichier de magasin de données Outlook",
        Pdf = "Fichier au format PDF",
        Png = "Fichier Png",
        PowerShell = "Fichier Microsoft Powershell",
        RichTextFormatFile = "Fichier au format texte enrichi",
        ShockwaveFlashFile = "Fichier Shockwave Flash",
        SvgFile = "Ficher Svg",
        TaggedImageFormatFile = "Fichier d'image tagué",
        TemporaryDataFile = "Fichier de données temporaires",
        TextDocument = "Document texte",
        UnknownValue = "Fichier inconnu",
        VideoObjectFile = "Fichier vidéo",
        WaveAudioFile = "Fichier audio",
        WindowsAudioFile = "Fichier audio Windows",
        WindowsMediaAudioFile = "Fichier audio Windows Media",
        WindowsMediaDownloadFile = "Fichier de téléchargement Windows Media",
        WindowsMediaSkinsFile = "Fichier de skins Windows Media",
        WindowsMediaVideoFile = "Fichier vidéo Windows Media",
        WordPerfectDocument = "Document Word Perfect",
        Xps = "Fichier Xps"
    };

    public string WindowsAudioFile { get; set; } = "Windows Audio File";

    public string MicrosoftAccessDatabaseFile { get; set; } = "Microsoft Access Database File";

    public string MicrosoftAccessExecuteOnlyFile { get; set; } = "Microsoft Access Execute-Only File";

    public string MicrosoftAccessRuntimeDatabase { get; set; } = "Microsoft Access Runtime Database";

    public string MicrosoftAccessDatabaseTemplate { get; set; } = "Microsoft Access Database Template";

    public string AudioInterchangeFileFormatFile { get; set; } = "Audio Interchange File Format File";

    public string AspNetActiveServerPage { get; set; } = "ASP.NET Active Server Page";

    public string AspFile { get; set; } = "Active Server Page";

    public string AudioVideoInterleaveFile { get; set; } = "Audio Video Interleave Movie or Sound File";

    public string BatchFile { get; set; } = "Batch File";

    public string BinaryFile { get; set; } = "Binary Compressed File";

    public string BitmapFile { get; set; } = "Bitmap File";

    public string Cab { get; set; } = "Windows Cabinet File";

    public string Cda { get; set; } = "CD Audio Track";

    public string Csv { get; set; } = "Comma-separated Values File";

    public string Dif { get; set; } = "Spreadsheet Data Interchange Format File";

    public string Dll { get; set; } = "Dynamic Link Library File";

    public string MicrosoftWordDocument { get; set; } = "Microsoft Word Document";

    public string MicrosoftWordTemplate { get; set; } = "Microsoft Word Template";

    public string MicrosoftWordMacroEnabledDocument { get; set; } = "Microsoft Word Macro-Enabled Document";

    public string Eml { get; set; } = "Email Message";

    public string Eps { get; set; } = "Encapsulated Postscript File";

    public string Exe { get; set; } = "Executable Program File";

    public string Flv { get; set; } = "Flash Video File";

    public string Gif { get; set; } = "GIF File";

    public string Html { get; set; } = "HTML Document";

    public string Ini { get; set; } = "Windows Initialization Configuration File";

    public string Iso { get; set; } = "ISO-9660 Disc Image";

    public string Jar { get; set; } = "Java Architecture File";

    public string Jpg { get; set; } = "Jpg File";

    public string Jpeg { get; set; } = "Jpeg File";

    public string M4a { get; set; } = "MPEG-4 Audio File";

    public string Mdb { get; set; } = "Microsoft Access Database";

    public string Midi { get; set; } = "Musical Instrument Digital Interface File";

    public string Mov { get; set; } = "Apple QuickTime Movie File";

    public string Mp3 { get; set; } = "MPEG 3 Audio File";

    public string Mp4 { get; set; } = "MPEG 4 Video File";

    public string Mpeg { get; set; } = "MPEG Video File";

    public string Mpg { get; set; } = "MPEG 1 File";

    public string Msi { get; set; } = "Microsoft Installer File";

    public string Mui { get; set; } = "Multilingual User Interface File";

    public string Pdf { get; set; } = "Portable Document Format File";

    public string Png { get; set; } = "Png File";

    public string MicrosoftPowerPointAddIn { get; set; } = "Microsoft PowerPoint Add-In";

    public string MicrosoftPowerPointSlide { get; set; } = "Microsoft PowerPoint Slide";

    public string PowerShell { get; set; } = "Microsoft PowerShell File";

    public string AdobePhotoshopFile { get; set; } = "Adobe Photoshop File";

    public string OutlookDataStore { get; set; } = "Outlook data store File";

    public string MicrosoftPublisherFile { get; set; } = "Microsoft Publisher File";

    public string ArchiveCompressedFile { get; set; } = "Archive Compressed File";

    public string RichTextFormatFile { get; set; } = "Rich Text Format File";

    public string ShockwaveFlashFile { get; set; } = "Shockwave Flash File";

    public string MicrosoftWindowsSystemSettings { get; set; } = "Microsoft DOS and Windows System Settings and Variables File";

    public string TaggedImageFormatFile { get; set; } = "Tagged Image Format File";

    public string TemporaryDataFile { get; set; } = "Temporary Data File";

    public string TextDocument { get; set; } = "Text Document";

    public string VideoObjectFile { get; set; } = "Video Object File";

    public string MicrosoftVisio { get; set; } = "Microsoft Visio";

    public string MicrosoftVisioMacroEnabled { get; set; } = "Microsoft Visio Macro-Enabled Drawing";

    public string MicrosoftVisioDrawing { get; set; } = "Microsoft Visio Drawing";

    public string MicrosoftVisioTemplate { get; set; } = "Microsoft Visio Template";

    public string WaveAudioFile { get; set; } = "Wave Audio File";

    public string MicrosoftWorksFile { get; set; } = "Microsoft Works File";

    public string WindowsMediaAudioFile { get; set; } = "Windows Media Audio File";

    public string WindowsMediaDownloadFile { get; set; } = "Windows Media Download File";

    public string WindowsMediaVideoFile { get; set; } = "Windows Media Video File";

    public string WindowsMediaSkinsFile { get; set; } = "Windows Media Skins File";

    public string WordPerfectDocument { get; set; } = "Word Perfect Document";

    public string MicrosoftExcelAddIn { get; set; } = "Microsoft Excel Add-In File";

    public string MicrosoftExcelDocument { get; set; } = "Microsoft Excel Document";

    public string MicrosoftExcelTemplate { get; set; } = "Microsoft Excel Template";

    public string MicrosoftExcelWorkbook { get; set; } = "Microsoft Excel Workbook";

    public string MicrosoftExcelMacroEnabled { get; set; } = "Microsoft Excel Macro-Enabled Workbook";

    public string Xps { get; set; } = "Xml Paper Specification";

    public string UnknownValue { get; set; } = "Unknown file";

    public string MicrosoftPowerPointTemplate { get; set; } = "Microsoft PowerPoint Template";

    public string MicrosoftPowerPointMacroEnabled { get; set; } = "Microsoft PowerPoint Macro-Enabled Template";

    public string MicrosoftPowerPointDocument { get; set; } = "Microsoft PowerPoint Document";

    public string MicrosoftExcelDllAddIn { get; set; } = "Microsoft Excel DLL-based Add-In";

    public string MicrosoftPowerBiDocument { get; set; } = "Microsoft Power Bi Document";

    public string SvgFile { get; set; } = "Svg File";
}

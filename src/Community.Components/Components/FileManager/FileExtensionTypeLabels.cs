namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels for the file from its extension.
/// </summary>
public record FileExtensionTypeLabels
{
    /// <summary>
    /// Prevents the creation of an instance of the class <see cref="FileExtensionTypeLabels"/>.
    /// </summary>
    private FileExtensionTypeLabels() { }

    /// <summary>
    /// Gets the default labels.
    /// </summary>
    public static FileExtensionTypeLabels Default { get; } = new FileExtensionTypeLabels();

    /// <summary>
    /// Gets the french labels.
    /// </summary>
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
        Xps = "Fichier Xps",
        JsonFile = "Fichier Json"
    };

    /// <summary>
    /// Gets or sets the label for a Windows audio file.
    /// </summary>
    public string WindowsAudioFile { get; set; } = "Windows Audio File";

    /// <summary>
    /// Gets or sets the label for Microsoft Access database file.
    /// </summary>
    public string MicrosoftAccessDatabaseFile { get; set; } = "Microsoft Access Database File";

    /// <summary>
    /// Gets or sets the label for icrosoft Access execute-only file
    /// </summary>
    public string MicrosoftAccessExecuteOnlyFile { get; set; } = "Microsoft Access Execute-Only File";

    /// <summary>
    /// Gets or sets the label for Microsoft Access runtime database file.
    /// </summary>
    public string MicrosoftAccessRuntimeDatabase { get; set; } = "Microsoft Access Runtime Database";

    /// <summary>
    /// Gets or sets the label for Microsoft Access database template file.
    /// </summary>
    public string MicrosoftAccessDatabaseTemplate { get; set; } = "Microsoft Access Database Template";

    /// <summary>
    /// Gets or sets the label for audio interchange file format file.
    /// </summary>
    public string AudioInterchangeFileFormatFile { get; set; } = "Audio Interchange File Format File";

    /// <summary>
    /// Gets or sets the label for Asp.Net file.
    /// </summary>
    public string AspNetActiveServerPage { get; set; } = "ASP.NET Active Server Page";

    /// <summary>
    /// Gets or sets the label for Asp file.
    /// </summary>
    public string AspFile { get; set; } = "Active Server Page";

    /// <summary>
    /// Gets or sets the label for avi file.
    /// </summary>
    public string AudioVideoInterleaveFile { get; set; } = "Audio Video Interleave Movie or Sound File";

    /// <summary>
    /// Gets or sets the label for batch file.
    /// </summary>
    public string BatchFile { get; set; } = "Batch File";

    /// <summary>
    /// Gets or sets the label for binary file.
    /// </summary>
    public string BinaryFile { get; set; } = "Binary Compressed File";

    /// <summary>
    /// Gets or sets the label for bitmap file.
    /// </summary>
    public string BitmapFile { get; set; } = "Bitmap File";

    /// <summary>
    /// Gets or sets the label for Windows cabinet file.
    /// </summary>
    public string Cab { get; set; } = "Windows Cabinet File";

    /// <summary>
    /// Gets or sets the label for CD audio track.
    /// </summary>
    public string Cda { get; set; } = "CD Audio Track";

    /// <summary>
    /// Gets or sets the label for csv file.
    /// </summary>
    public string Csv { get; set; } = "Comma-separated Values File";

    /// <summary>
    /// Gets or sets the label for dif file.
    /// </summary>
    public string Dif { get; set; } = "Spreadsheet Data Interchange Format File";

    /// <summary>
    /// Gets or sets the label for dll file.
    /// </summary>
    public string Dll { get; set; } = "Dynamic Link Library File";

    /// <summary>
    /// Gets or sets the label for Microsoft Word file.
    /// </summary>
    public string MicrosoftWordDocument { get; set; } = "Microsoft Word Document";

    /// <summary>
    /// Gets or sets the label for Microsoft Word template file.
    /// </summary>
    public string MicrosoftWordTemplate { get; set; } = "Microsoft Word Template";

    /// <summary>
    /// Gets or sets the label for Microsoft Word macro enabled document.
    /// </summary>
    public string MicrosoftWordMacroEnabledDocument { get; set; } = "Microsoft Word Macro-Enabled Document";

    /// <summary>
    /// Gets or sets the label for eml file.
    /// </summary>
    public string Eml { get; set; } = "Email Message";

    /// <summary>
    /// Gets or sets the label for eps file.
    /// </summary>
    public string Eps { get; set; } = "Encapsulated Postscript File";

    /// <summary>
    /// Gets or sets the label for exe file.
    /// </summary>
    public string Exe { get; set; } = "Executable Program File";

    /// <summary>
    /// Gets or sets the label for flv file.
    /// </summary>
    public string Flv { get; set; } = "Flash Video File";

    /// <summary>
    /// Gets or sets the label for gif file.
    /// </summary>
    public string Gif { get; set; } = "GIF File";

    /// <summary>
    /// Gets or sets the label for html file.
    /// </summary>
    public string Html { get; set; } = "HTML Document";

    /// <summary>
    /// Gets or sets the label for ini file.
    /// </summary>
    public string Ini { get; set; } = "Windows Initialization Configuration File";

    /// <summary>
    /// Gets or sets the label for iso file.
    /// </summary>
    public string Iso { get; set; } = "ISO-9660 Disc Image";

    /// <summary>
    /// Gets or sets the label for jar file.
    /// </summary>
    public string Jar { get; set; } = "Java Architecture File";

    /// <summary>
    /// Gets or sets the label for jpg file.
    /// </summary>
    public string Jpg { get; set; } = "Jpg File";

    /// <summary>
    /// Gets or sets the label for jpeg file.
    /// </summary>
    public string Jpeg { get; set; } = "Jpeg File";

    /// <summary>
    /// Gets or sets the label for m4a file.
    /// </summary>
    public string M4a { get; set; } = "MPEG-4 Audio File";

    /// <summary>
    /// Gets or sets the label for Microsoft access database file.
    /// </summary>
    public string Mdb { get; set; } = "Microsoft Access Database";

    /// <summary>
    /// Gets or sets the label for midi file.
    /// </summary>
    public string Midi { get; set; } = "Musical Instrument Digital Interface File";

    /// <summary>
    /// Gets or sets the label for mov file.
    /// </summary>
    public string Mov { get; set; } = "Apple QuickTime Movie File";

    /// <summary>
    /// Gets or sets the label for mp3 file.
    /// </summary>
    public string Mp3 { get; set; } = "MPEG 3 Audio File";

    /// <summary>
    /// Gets or sets the label for mp4 file.
    /// </summary>
    public string Mp4 { get; set; } = "MPEG 4 Video File";

    /// <summary>
    /// Gets or sets the label for mpg file.
    /// </summary>
    public string Mpeg { get; set; } = "MPEG Video File";

    /// <summary>
    /// Gets or sets the label for mp1 file.
    /// </summary>
    public string Mpg { get; set; } = "MPEG 1 File";

    /// <summary>
    /// Gets or sets the label for msi file.
    /// </summary>
    public string Msi { get; set; } = "Microsoft Installer File";

    /// <summary>
    /// Gets or sets the label for mui file.
    /// </summary>
    public string Mui { get; set; } = "Multilingual User Interface File";

    /// <summary>
    /// Gets or sets the label for pdf file.
    /// </summary>
    public string Pdf { get; set; } = "Portable Document Format File";

    /// <summary>
    /// Gets or sets the label for png file.
    /// </summary>
    public string Png { get; set; } = "Png File";

    /// <summary>
    /// Gets or sets the label for Microsoft Powerpoint Add-in file.
    /// </summary>
    public string MicrosoftPowerPointAddIn { get; set; } = "Microsoft PowerPoint Add-In";

    /// <summary>
    /// Gets or sets the label for Microsoft Powerpoint slide file.
    /// </summary>
    public string MicrosoftPowerPointSlide { get; set; } = "Microsoft PowerPoint Slide";

    /// <summary>
    /// Gets or sets the label for Microsoft Powershell file.
    /// </summary>
    public string PowerShell { get; set; } = "Microsoft PowerShell File";

    /// <summary>
    /// Gets or sets the label for Adobe photoshop file.
    /// </summary>
    public string AdobePhotoshopFile { get; set; } = "Adobe Photoshop File";

    /// <summary>
    /// Gets or sets the label for Microsoft Outlook data store file.
    /// </summary>
    public string OutlookDataStore { get; set; } = "Outlook data store File";

    /// <summary>
    /// Gets or sets the label for Microsoft Publisher file.
    /// </summary>
    public string MicrosoftPublisherFile { get; set; } = "Microsoft Publisher File";

    /// <summary>
    /// Gets or sets the label for zip, rar, 7zip file.
    /// </summary>
    public string ArchiveCompressedFile { get; set; } = "Archive Compressed File";

    /// <summary>
    /// Gets or sets the label for rtf file.
    /// </summary>
    public string RichTextFormatFile { get; set; } = "Rich Text Format File";

    /// <summary>
    /// Gets or sets the label for Shockwave flash file.
    /// </summary>
    public string ShockwaveFlashFile { get; set; } = "Shockwave Flash File";

    /// <summary>
    /// Gets or sets the label for Microsoft Windows system settings file.
    /// </summary>
    public string MicrosoftWindowsSystemSettings { get; set; } = "Microsoft DOS and Windows System Settings and Variables File";

    /// <summary>
    /// Gets or sets the label for tiff file.
    /// </summary>
    public string TaggedImageFormatFile { get; set; } = "Tagged Image Format File";

    /// <summary>
    /// Gets or sets the label for temp data file.
    /// </summary>
    public string TemporaryDataFile { get; set; } = "Temporary Data File";

    /// <summary>
    /// Gets or sets the label for text file.
    /// </summary>
    public string TextDocument { get; set; } = "Text Document";

    /// <summary>
    /// Gets or sets the label for video object file.
    /// </summary>
    public string VideoObjectFile { get; set; } = "Video Object File";

    /// <summary>
    /// Gets or sets the label for Microsoft Visio file.
    /// </summary>
    public string MicrosoftVisio { get; set; } = "Microsoft Visio";

    /// <summary>
    /// Gets or sets the label for Microsoft Visio macro-enabled drawing file.
    /// </summary>
    public string MicrosoftVisioMacroEnabled { get; set; } = "Microsoft Visio Macro-Enabled Drawing";

    /// <summary>
    /// Gets or sets the label for Microsoft Visio drawing file.
    /// </summary>
    public string MicrosoftVisioDrawing { get; set; } = "Microsoft Visio Drawing";

    /// <summary>
    /// Gets or sets the label for Microsoft Visio template.
    /// </summary>
    public string MicrosoftVisioTemplate { get; set; } = "Microsoft Visio Template";

    /// <summary>
    /// Gets or sets the label for wave file.
    /// </summary>
    public string WaveAudioFile { get; set; } = "Wave Audio File";

    /// <summary>
    /// Gets or sets the label for Microsoft Works file.
    /// </summary>
    public string MicrosoftWorksFile { get; set; } = "Microsoft Works File";

    /// <summary>
    /// Gets or sets the label for Windows media audio file.
    /// </summary>
    public string WindowsMediaAudioFile { get; set; } = "Windows Media Audio File";

    /// <summary>
    /// Gets or sets the label for Windows media download file.
    /// </summary>
    public string WindowsMediaDownloadFile { get; set; } = "Windows Media Download File";

    /// <summary>
    /// Gets or sets the label for Windows media video file.
    /// </summary>
    public string WindowsMediaVideoFile { get; set; } = "Windows Media Video File";

    /// <summary>
    /// Gets or sets the label for Windows media skin file.
    /// </summary>
    public string WindowsMediaSkinsFile { get; set; } = "Windows Media Skins File";

    /// <summary>
    /// Gets or sets the label for Word perfect document file.
    /// </summary>
    public string WordPerfectDocument { get; set; } = "Word Perfect Document";

    /// <summary>
    /// Gets or sets the label for Microsoft Excel add-in file.
    /// </summary>
    public string MicrosoftExcelAddIn { get; set; } = "Microsoft Excel Add-In File";

    /// <summary>
    /// Gets or sets the label for Microsoft Excel file.
    /// </summary>
    public string MicrosoftExcelDocument { get; set; } = "Microsoft Excel Document";

    /// <summary>
    /// Gets or sets the label for Microsoft Excel template file.
    /// </summary>
    public string MicrosoftExcelTemplate { get; set; } = "Microsoft Excel Template";

    /// <summary>
    /// Gets or sets the label for Microsoft Excel workbook file.
    /// </summary>
    public string MicrosoftExcelWorkbook { get; set; } = "Microsoft Excel Workbook";

    /// <summary>
    /// Gets or sets the label for Microsoft Excel macro-enabled file.
    /// </summary>
    public string MicrosoftExcelMacroEnabled { get; set; } = "Microsoft Excel Macro-Enabled Workbook";

    /// <summary>
    /// Gets or sets the label for xps file.
    /// </summary>
    public string Xps { get; set; } = "Xml Paper Specification";

    /// <summary>
    /// Gets or sets the label for unknwon file.
    /// </summary>
    public string UnknownValue { get; set; } = "Unknown file";

    /// <summary>
    /// Gets or sets the label for Microsoft Powerpoint template file.
    /// </summary>
    public string MicrosoftPowerPointTemplate { get; set; } = "Microsoft PowerPoint Template";

    /// <summary>
    /// Gets or sets the label for Microsoft Powerpoint marcro-enabled file.
    /// </summary>
    public string MicrosoftPowerPointMacroEnabled { get; set; } = "Microsoft PowerPoint Macro-Enabled Template";

    /// <summary>
    /// Gets or sets the label for Microsoft Powerpoint Document file.
    /// </summary>
    public string MicrosoftPowerPointDocument { get; set; } = "Microsoft PowerPoint Document";

    /// <summary>
    /// Gets or sets the label for Microsoft Excel dll based add-in file.
    /// </summary>
    public string MicrosoftExcelDllAddIn { get; set; } = "Microsoft Excel DLL-based Add-In";

    /// <summary>
    /// Gets or sets the label for Microsoft PowerBi Document file.
    /// </summary>
    public string MicrosoftPowerBiDocument { get; set; } = "Microsoft Power Bi Document";

    /// <summary>
    /// Gets or sets the label for svg file.
    /// </summary>
    public string SvgFile { get; set; } = "Svg File";

    /// <summary>
    /// Gets or sets the label for json file.
    /// </summary>
    public string JsonFile { get; set; } = "Json File";
}

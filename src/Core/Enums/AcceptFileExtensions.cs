namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents precise file extensions that can be accepted by the
/// <see cref="FluentCxFileManager{TItem}"/>. This enumeration allows fine-grained
/// control over specific formats and can be combined with <see cref="AcceptFileCategory"/>
/// and custom extensions.
/// </summary>
[Flags]
public enum AcceptFileExtension : ulong
{
    /// <summary>
    /// Represents no extensions accepted.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents JPG image format.
    /// </summary>
    Jpg = 1,

    /// <summary>
    /// Represents JPEG image format.
    /// </summary>
    Jpeg = 2,

    /// <summary>
    /// Represents PNG image format.
    /// </summary>
    Png = 4,

    /// <summary>
    /// Represents GIF image format.
    /// </summary>
    Gif = 8,

    /// <summary>
    /// Represents BMP image format.
    /// </summary>
    Bmp = 16,

    /// <summary>
    /// Represents WEBP image format.
    /// </summary>
    Webp = 32,

    /// <summary>
    /// Represents SVG image format.
    /// </summary>
    Svg = 64,

    /// <summary>
    /// Represents TIFF image format.
    /// </summary>
    Tiff = 128,

    /// <summary>
    /// Represents HEIC image format.
    /// </summary>
    Heic = 256,

    /// <summary>
    /// Represents PDF document format.
    /// </summary>
    Pdf = 512,

    /// <summary>
    /// Represents DOC document format.
    /// </summary>
    Doc = 1024,

    /// <summary>
    /// Represents DOCX document format.
    /// </summary>
    Docx = 2048,

    /// <summary>
    /// Represents XLS document format.
    /// </summary>
    Xls = 4096,

    /// <summary>
    /// Represents XSLX document format.
    /// </summary>
    Xlsx = 8192,

    /// <summary>
    /// Represents PPT document format.
    /// </summary>
    Ppt = 16384,

    /// <summary>
    /// Represents PPTX document format.
    /// </summary>
    Pptx = 32768,

    /// <summary>
    /// Represents TXT document format.
    /// </summary>
    Txt = 65536,

    /// <summary>
    /// Represents CSV document format.
    /// </summary>
    Csv = 131072,

    /// <summary>
    /// Represents JSON document format.
    /// </summary>
    Json = 262144,

    /// <summary>
    /// Represents XML document format.
    /// </summary>
    Xml = 524288,

    /// <summary>
    /// Represents Markdown document format.
    /// </summary>
    Md = 1048576,

    /// <summary>
    /// Represents YAML document format.
    /// </summary>
    Yaml = 2097152,

    /// <summary>
    /// Represents ZIP archive format.
    /// </summary>
    Zip = 4194304,

    /// <summary>
    /// Represents RAR archive format.
    /// </summary>
    Rar = 8388608,

    /// <summary>
    /// Represents 7-Zip archive format.
    /// </summary>
    SevenZip = 16777216,

    /// <summary>
    /// Represents TAR archive format.
    /// </summary>
    Tar = 33554432,

    /// <summary>
    /// Represents GZ archive format.
    /// </summary>
    Gz = 67108864,

    /// <summary>
    /// Represents C# source code format.
    /// </summary>
    Cs = 134217728,

    /// <summary>
    /// Represents Javascript source code format.
    /// </summary>
    Js = 268435456,

    /// <summary>
    /// Represents Typescript source code format.
    /// </summary>
    Ts = 536870912,

    /// <summary>
    /// Represents Html source code format.
    /// </summary>
    Html = 1073741824,

    /// <summary>
    /// Represents Css source code format.
    /// </summary>
    Css = 2147483648,

    /// <summary>
    /// Represents Sql source code format.
    /// </summary>
    Sql = 4294967296,

    /// <summary>
    /// Represents Python source code format.
    /// </summary>
    Py = 8589934592,

    /// <summary>
    /// Represents Java source code format.
    /// </summary>
    Java = 17179869184,

    /// <summary>
    /// Represents OBJ 3D model format.
    /// </summary>
    Obj = 34359738368,

    /// <summary>
    /// Represents FBX 3D model format.
    /// </summary>
    Fbx = 68719476736,

    /// <summary>
    /// Represents STL 3D model format.
    /// </summary>
    Stl = 137438953472,

    /// <summary>
    /// Represents STEP 3D model format.
    /// </summary>
    Step = 274877906944,

    /// <summary>
    /// Represents TTF font format.
    /// </summary>
    Ttf = 549755813888,

    /// <summary>
    /// Represents OTF font format.
    /// </summary>
    Otf = 1099511627776,

    /// <summary>
    /// Represents WOFF font format.
    /// </summary>
    Woff = 2199023255552,

    /// <summary>
    /// Represents WOFF2 font format.
    /// </summary>
    Woff2 = 4398046511104,

    /// <summary>
    /// Represents BIN binary file format.
    /// </summary>
    Bin = 8796093022208,

    /// <summary>
    /// Represents DAT data file format.
    /// </summary>
    Dat = 17592186044416,

    /// <summary>
    /// Represents FcxSurf binary file format.
    /// </summary>
    FcxSurf = 35184372088832,

    /// <summary>
    /// Represents a combination of common image file formats, including JPEG, PNG, GIF, BMP, WebP, SVG, TIFF, and HEIC.
    /// </summary>
    /// <remarks>This value can be used to specify or filter operations that apply to widely used image
    /// formats. It is useful when an operation should target all standard image types supported by most
    /// platforms.</remarks>
    ImageCommon = Jpg | Jpeg | Png | Gif | Bmp | Webp | Svg | Tiff | Heic,

    /// <summary>
    /// Represents a combination of common document file formats, including PDF, Word, Excel, and PowerPoint variants.
    /// </summary>
    /// <remarks>This value can be used to specify or filter for standard office document types in scenarios
    /// such as file selection dialogs or document processing workflows.</remarks>
    DocumentCommon = Pdf | Doc | Docx | Xls | Xlsx | Ppt | Pptx,

    /// <summary>
    /// Specifies the set of structured text formats, including JSON, XML, CSV, Markdown, and YAML.
    /// </summary>
    /// <remarks>Use this enumeration to indicate that a value may represent any of the supported structured
    /// text formats. This is useful when handling data that can be serialized or deserialized in multiple standard
    /// text-based formats.</remarks>
    TextStructured = Json | Xml | Csv | Md | Yaml,

    /// <summary>
    /// Specifies a combination of common archive formats, including Zip, Rar, 7-Zip, Tar, and Gz.
    /// </summary>
    /// <remarks>This value can be used to represent operations or filters that apply to any of the listed
    /// archive types. It is typically used when an action should target all supported common archive formats.</remarks>
    ArchiveCommon = Zip | Rar | SevenZip | Tar | Gz,

    /// <summary>
    /// Sepecifies a combination of common source code file formats, including C#, JavaScript, TypeScript, HTML, CSS, SQL, Python, and Java.
    /// </summary>
    DevCommon = Cs | Js | Ts | Html | Css | Sql | Py | Java,

    /// <summary>
    /// Specifies common font file formats supported by the application.
    /// </summary>
    /// <remarks>This enumeration combines several widely used font formats, including TrueType (TTF),
    /// OpenType (OTF), Web Open Font Format (WOFF), and WOFF2. Use this value when working with APIs or components that
    /// accept or return multiple font types.</remarks>
    FontCommon = Ttf | Otf | Woff | Woff2,

    /// <summary>
    /// Specifies common 3D model formats supported by the application.
    /// </summary>
    /// <remarks>Use this enumeration to indicate or check the type of 3D model file being processed, such as
    /// OBJ, FBX, STL, or STEP formats.</remarks>
    ModelCommon = Obj | Fbx | Stl | Step,

    /// <summary>
    /// Specifies a combination of binary-related flags, including Bin, Dat, and FcxSurf.
    /// </summary>
    /// <remarks>This value represents a bitwise combination of the Bin, Dat, and FcxSurf flags. It can be
    /// used to indicate that all three binary formats are applicable or supported in a given context.</remarks>
    BinaryCommon = Bin | Dat | FcxSurf,

    /// <summary>
    /// Specifies that all defined file extensions are accepted.
    /// </summary>
    All = ImageCommon | DocumentCommon |
          TextStructured | ArchiveCommon |
          DevCommon | FontCommon |
        ModelCommon | BinaryCommon
}

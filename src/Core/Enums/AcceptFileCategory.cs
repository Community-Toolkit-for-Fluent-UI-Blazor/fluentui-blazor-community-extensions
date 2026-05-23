namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents high-level file categories that can be accepted by the
/// <see cref="FluentCxFileManager{TItem}"/>. These categories map to broad
/// MIME types such as "image/*" or "audio/*".
/// </summary>
[Flags]
public enum AcceptFileCategory : int
{
    /// <summary>
    /// No category allowed.
    /// </summary>
    None = 0,

    /// <summary>
    /// Image files (image/*).
    /// </summary>
    Image = 1,

    /// <summary>
    /// Audio files (audio/*).
    /// </summary>
    Audio = 2,

    /// <summary>
    /// Video files (video/*).
    /// </summary>
    Video = 4,

    /// <summary>
    /// Document files (PDF, Word, Excel, PowerPoint, etc.).
    /// </summary>
    Document = 8,

    /// <summary>
    /// Text-based files (TXT, CSV, JSON, XML, Markdown, etc.).
    /// </summary>
    Text = 16,

    /// <summary>
    /// Archive files (ZIP, RAR, 7Z, TAR, GZ, etc.).
    /// </summary>
    Archive = 32,

    /// <summary>
    /// Source code or development files (CS, JS, HTML, CSS, SQL, etc.).
    /// </summary>
    Code = 64,

    /// <summary>
    /// 3D or CAD files (OBJ, FBX, STL, STEP, etc.).
    /// </summary>
    ThreeD = 128,

    /// <summary>
    /// Font files (TTF, OTF, WOFF, WOFF2).
    /// </summary>
    Font = 256,

    /// <summary>
    /// Generic binary files (BIN, DAT, etc.).
    /// </summary>
    Binary = 512,

    /// <summary>
    /// Proprietary or application-specific formats (e.g., .fcxsign).
    /// </summary>
    Proprietary = 1024,

    /// <summary>
    /// All categories allowed.
    /// </summary>
    All = Image | Audio | Video | Document | Text | Archive | Code | ThreeD | Font | Binary | Proprietary
}


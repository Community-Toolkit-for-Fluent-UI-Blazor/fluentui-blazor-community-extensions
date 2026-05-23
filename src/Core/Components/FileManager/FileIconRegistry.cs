namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a registry for associating file extensions with icon keys and resolving the appropriate icon key for a
/// given file extension.
/// </summary>
/// <remarks>This class enables applications to map file extensions to specific icon representations, allowing for
/// consistent display of file types in user interfaces. The registry includes default mappings for common file types
/// and allows for custom associations to be registered at runtime. All operations are case-insensitive with respect to
/// file extensions.</remarks>
public static class FileIconRegistry
{
    /// <summary>
    /// Provides a mapping between file extensions and their corresponding file icon keys, using a case-insensitive
    /// string comparer.
    /// </summary>
    /// <remarks>This dictionary enables quick lookup of a file icon key based on a file's extension or type.
    /// The mapping includes common file types such as Excel documents, images, audio, video, PDF files, folders, and a
    /// default type for unrecognized extensions.</remarks>
    private static readonly Dictionary<string, FileIconKey> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        [".xls"] = FileIconKey.Excel,
        [".xlsx"] = FileIconKey.Excel,
        [".xlsm"] = FileIconKey.Excel,
        [".csv"] = FileIconKey.Excel,
        [".xlsb"] = FileIconKey.Excel,
        [".xlam"] = FileIconKey.Excel,
        [".xltx"] = FileIconKey.Excel,
        [".xltm"] = FileIconKey.Excel,
        [".xla"] = FileIconKey.Excel,
        [".xlm"] = FileIconKey.Excel,
        [".xlw"] = FileIconKey.Excel,
        [".odc"] = FileIconKey.Excel,
        [".ods"] = FileIconKey.Excel,
        [".pptx"] = FileIconKey.PowerPoint,
        [".pptm"] = FileIconKey.PowerPoint,
        [".ppt"] = FileIconKey.PowerPoint,
        [".potx"] = FileIconKey.PowerPoint,
        [".potm"] = FileIconKey.PowerPoint,
        [".pot"] = FileIconKey.PowerPoint,
        [".ppsx"] = FileIconKey.PowerPoint,
        [".ppsm"] = FileIconKey.PowerPoint,
        [".pps"] = FileIconKey.PowerPoint,
        [".ppam"] = FileIconKey.PowerPoint,
        [".ppa"] = FileIconKey.PowerPoint,
        [".odp"] = FileIconKey.PowerPoint,
        [".docx"] = FileIconKey.Word,
        [".doc"] = FileIconKey.Word,
        [".docm"] = FileIconKey.Word,
        [".dotx"] = FileIconKey.Word,
        [".dotm"] = FileIconKey.Word,
        [".dot"] = FileIconKey.Word,
        [".rtf"] = FileIconKey.Word,
        [".odt"] = FileIconKey.Word,
        [".jpg"] = FileIconKey.Image,
        [".jpeg"] = FileIconKey.Image,
        [".jfif"] = FileIconKey.Image,
        [".pjpeg"] = FileIconKey.Image,
        [".pjpg"] = FileIconKey.Image,
        [".png"] = FileIconKey.Image,
        [".bmp"] = FileIconKey.Image,
        [".apng"] = FileIconKey.Image,
        [".avif"] = FileIconKey.Image,
        [".gif"] = FileIconKey.Image,
        [".svg"] = FileIconKey.Image,
        [".webp"] = FileIconKey.Image,
        [".ico"] = FileIconKey.Image,
        [".cur"] = FileIconKey.Image,
        [".tif"] = FileIconKey.Image,
        [".tiff"] = FileIconKey.Image,
        [".3gp"] = FileIconKey.Audio,
        [".aa"] = FileIconKey.Audio,
        [".aac"] = FileIconKey.Audio,
        [".aax"] = FileIconKey.Audio,
        [".act"] = FileIconKey.Audio,
        [".aiff"] = FileIconKey.Audio,
        [".alac"] = FileIconKey.Audio,
        [".amr"] = FileIconKey.Audio,
        [".ape"] = FileIconKey.Audio,
        [".au"] = FileIconKey.Audio,
        [".awb"] = FileIconKey.Audio,
        [".dss"] = FileIconKey.Audio,
        [".dvf"] = FileIconKey.Audio,
        [".flac"] = FileIconKey.Audio,
        [".gsm"] = FileIconKey.Audio,
        [".iklax"] = FileIconKey.Audio,
        [".ivs"] = FileIconKey.Audio,
        [".m4a"] = FileIconKey.Audio,
        [".m4b"] = FileIconKey.Audio,
        [".m4p"] = FileIconKey.Audio,
        [".mmf"] = FileIconKey.Audio,
        [".movpkg"] = FileIconKey.Audio,
        [".mp1"] = FileIconKey.Audio,
        [".mp2"] = FileIconKey.Audio,
        [".mp3"] = FileIconKey.Audio,
        [".mpc"] = FileIconKey.Audio,
        [".msv"] = FileIconKey.Audio,
        [".nmf"] = FileIconKey.Audio,
        [".ogg"] = FileIconKey.Audio,
        [".oga"] = FileIconKey.Audio,
        [".mogg"] = FileIconKey.Audio,
        [".opus"] = FileIconKey.Audio,
        [".ra"] = FileIconKey.Audio,
        [".rm"] = FileIconKey.Audio,
        [".raw"] = FileIconKey.Audio,
        [".rf64"] = FileIconKey.Audio,
        [".tta"] = FileIconKey.Audio,
        [".voc"] = FileIconKey.Audio,
        [".vox"] = FileIconKey.Audio,
        [".wav"] = FileIconKey.Audio,
        [".wma"] = FileIconKey.Audio,
        [".wv"] = FileIconKey.Audio,
        [".weba"] = FileIconKey.Audio,
        [".8svx"] = FileIconKey.Audio,
        [".cda"] = FileIconKey.Audio,
        [".webm"] = FileIconKey.Video,
        [".mkv"] = FileIconKey.Video,
        [".flv"] = FileIconKey.Video,
        [".vob"] = FileIconKey.Video,
        [".ogv"] = FileIconKey.Video,
        [".drc"] = FileIconKey.Video,
        [".gifv"] = FileIconKey.Video,
        [".avi"] = FileIconKey.Video,
        [".mts"] = FileIconKey.Video,
        [".m2ts"] = FileIconKey.Video,
        [".ts"] = FileIconKey.Video,
        [".mov"] = FileIconKey.Video,
        [".qt"] = FileIconKey.Video,
        [".wmv"] = FileIconKey.Video,
        [".yuv"] = FileIconKey.Video,
        [".rmvb"] = FileIconKey.Video,
        [".viv"] = FileIconKey.Video,
        [".asf"] = FileIconKey.Video,
        [".amv"] = FileIconKey.Video,
        [".mp4"] = FileIconKey.Video,
        [".m4p"] = FileIconKey.Video,
        [".m4v"] = FileIconKey.Video,
        [".mpv"] = FileIconKey.Video,
        [".svi"] = FileIconKey.Video,
        [".3g2"] = FileIconKey.Video,
        [".mxf"] = FileIconKey.Video,
        [".roq"] = FileIconKey.Video,
        [".nsv"] = FileIconKey.Video,
        [".pdf"] = FileIconKey.Pdf,
        ["folder"] = FileIconKey.Folder,
        ["default"] = FileIconKey.Default
    };

    /// <summary>
    /// Registers a file extension with the specified icon key for use in file icon mapping.
    /// </summary>
    /// <remarks>If the provided extension does not begin with a period ('.'), one is automatically added. The
    /// mapping is case-insensitive.</remarks>
    /// <param name="extension">The file extension to associate with the icon key. The extension may include or omit the leading period ('.').</param>
    /// <param name="key">The icon key to associate with the specified file extension.</param>
    public static void Register(string extension, FileIconKey key)
    {
        if (!extension.StartsWith('.'))
        {
            extension = "." + extension;
        }

        _map[extension.ToLowerInvariant()] = key;
    }

    /// <summary>
    /// Resolves the appropriate file icon key for a given file extension.
    /// </summary>
    /// <remarks>If the provided extension does not begin with a period, one is automatically prepended. The
    /// method performs a case-insensitive lookup.</remarks>
    /// <param name="extension">The file extension to resolve, with or without a leading period. The comparison is case-insensitive.</param>
    /// <returns>A value of the FileIconKey enumeration that corresponds to the specified file extension. Returns
    /// FileIconKey.Default if the extension is not recognized.</returns>
    public static FileIconKey Resolve(string? extension)
    {
        if (string.IsNullOrEmpty(extension))
        {
            return FileIconKey.Default;
        }

        if (!extension.StartsWith('.'))
        {
            extension = "." + extension;
        }

        return _map.TryGetValue(extension.ToLowerInvariant(), out var key) ? key : FileIconKey.Default;
    }
}

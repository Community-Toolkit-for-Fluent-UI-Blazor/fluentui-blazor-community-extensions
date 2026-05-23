namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides utility methods for mapping file extension flags to their corresponding file extension strings.
/// </summary>
/// <remarks>This class is intended to assist in scenarios where a set of file types, represented by the
/// AcceptFileExtension flags, needs to be converted to their standard file extension representations. It is commonly
/// used in file upload or filtering components to generate file extension lists for accept attributes or
/// validation.</remarks>
internal static class AcceptFileExtensionMap
{
    /// <summary>
    /// Provides a mapping between file extension enumeration values and their corresponding string representations.
    /// </summary>
    /// <remarks>This dictionary is used to translate values of the AcceptFileExtension enumeration to their
    /// standard file extension strings. It supports a wide range of common file types, including images, documents,
    /// archives, code files, and fonts.</remarks>
    private static readonly Dictionary<AcceptFileExtension, string> _map = new(EqualityComparer<AcceptFileExtension>.Default)
    {
        { AcceptFileExtension.Jpg, ".jpg" },
        { AcceptFileExtension.Jpeg, ".jpeg" },
        { AcceptFileExtension.Png, ".png" },
        { AcceptFileExtension.Gif, ".gif" },
        { AcceptFileExtension.Bmp, ".bmp" },
        { AcceptFileExtension.Webp, ".webp" },
        { AcceptFileExtension.Svg, ".svg" },
        { AcceptFileExtension.Tiff, ".tiff" },
        { AcceptFileExtension.Heic, ".heic" },

        { AcceptFileExtension.Pdf, ".pdf" },
        { AcceptFileExtension.Doc, ".doc" },
        { AcceptFileExtension.Docx, ".docx" },
        { AcceptFileExtension.Xls, ".xls" },
        { AcceptFileExtension.Xlsx, ".xlsx" },
        { AcceptFileExtension.Ppt, ".ppt" },
        { AcceptFileExtension.Pptx, ".pptx" },

        { AcceptFileExtension.Txt, ".txt" },
        { AcceptFileExtension.Csv, ".csv" },
        { AcceptFileExtension.Json, ".json" },
        { AcceptFileExtension.Xml, ".xml" },
        { AcceptFileExtension.Md, ".md" },
        { AcceptFileExtension.Yaml, ".yaml" },

        { AcceptFileExtension.Zip, ".zip" },
        { AcceptFileExtension.Rar, ".rar" },
        { AcceptFileExtension.SevenZip, ".7z" },
        { AcceptFileExtension.Tar, ".tar" },
        { AcceptFileExtension.Gz, ".gz" },

        { AcceptFileExtension.Cs, ".cs" },
        { AcceptFileExtension.Js, ".js" },
        { AcceptFileExtension.Ts, ".ts" },
        { AcceptFileExtension.Html, ".html" },
        { AcceptFileExtension.Css, ".css" },
        { AcceptFileExtension.Sql, ".sql" },
        { AcceptFileExtension.Py, ".py" },
        { AcceptFileExtension.Java, ".java" },

        { AcceptFileExtension.Obj, ".obj" },
        { AcceptFileExtension.Fbx, ".fbx" },
        { AcceptFileExtension.Stl, ".stl" },
        { AcceptFileExtension.Step, ".step" },

        { AcceptFileExtension.Ttf, ".ttf" },
        { AcceptFileExtension.Otf, ".otf" },
        { AcceptFileExtension.Woff, ".woff" },
        { AcceptFileExtension.Woff2, ".woff2" },

        { AcceptFileExtension.Bin, ".bin" },
        { AcceptFileExtension.Dat, ".dat" },

        { AcceptFileExtension.FcxSurf, ".fcxsurf" }
    };

    /// <summary>
    /// Returns a collection of file extension strings that correspond to the specified set of accepted file extension
    /// flags.
    /// </summary>
    /// <param name="flags">A combination of <see cref="AcceptFileExtension"/> flags indicating which file extensions to include in the
    /// result.</param>
    /// <returns>An enumerable collection of strings representing the file extensions associated with the specified flags. The
    /// collection is empty if no flags are set.</returns>
    public static IEnumerable<string> Resolve(AcceptFileExtension flags)
    {
        foreach (var kvp in _map)
        {
            if (flags.HasFlag(kvp.Key))
            {
                yield return kvp.Value;
            }
        }
    }
}

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a mapping between file category flags and their corresponding MIME types or file extensions for use in file
/// selection dialogs.
/// </summary>
/// <remarks>This class is typically used to generate a list of accepted file types based on one or more
/// categories, such as images, documents, or archives. The resulting list can be used to set the 'accept' attribute of
/// file input elements in web applications, enabling users to filter selectable files by type.</remarks>
internal static class AcceptFileCategoryMap
{
    /// <summary>
    /// Returns a collection of file type filters corresponding to the specified file categories.
    /// </summary>
    /// <remarks>The returned collection can include both MIME types (such as "image/*") and file extensions
    /// (such as ".pdf"). This method is typically used to generate values for file input controls that restrict
    /// selectable files by type.</remarks>
    /// <param name="categories">A combination of file categories for which to resolve accepted file type filters.</param>
    /// <returns>An enumerable collection of strings representing file type filters for the specified categories. Each string is
    /// a MIME type or file extension suitable for use in file selection dialogs.</returns>
    public static IEnumerable<string> Resolve(AcceptFileCategory categories)
    {
        var list = new List<string>();

        if (categories.HasFlag(AcceptFileCategory.Image))
        {
            list.Add("image/*");
        }

        if (categories.HasFlag(AcceptFileCategory.Audio))
        {
            list.Add("audio/*");
        }

        if (categories.HasFlag(AcceptFileCategory.Video))
        {
            list.Add("video/*");
        }

        if (categories.HasFlag(AcceptFileCategory.Document))
        {
            list.AddRange([".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx"]);
        }

        if (categories.HasFlag(AcceptFileCategory.Text))
        {
            list.AddRange([".txt", ".csv", ".json", ".xml", ".md", ".yaml"]);
        }

        if (categories.HasFlag(AcceptFileCategory.Archive))
        {
            list.AddRange([".zip", ".rar", ".7z", ".tar", ".gz"]);
        }

        if (categories.HasFlag(AcceptFileCategory.Code))
        {
            list.AddRange([".cs", ".js", ".ts", ".html", ".css", ".sql", ".py", ".java"]);
        }

        if (categories.HasFlag(AcceptFileCategory.ThreeD))
        {
            list.AddRange([".obj", ".fbx", ".stl", ".step"]);
        }

        if (categories.HasFlag(AcceptFileCategory.Font))
        {
            list.AddRange([".ttf", ".otf", ".woff", ".woff2"]);
        }

        if (categories.HasFlag(AcceptFileCategory.Binary))
        {
            list.AddRange([".bin", ".dat"]);
        }

        if (categories.HasFlag(AcceptFileCategory.Proprietary))
        {
            list.Add(".fcxsurf");
        }

        return list;
    }
}


namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a provider to convert <see cref="AcceptFile"/> into html tag to be able to select files with <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public static class AcceptFileProvider
{
    /// <summary>
    /// Gets the html tags from <see cref="AcceptFile"/> value.
    /// </summary>
    /// <param name="value">Value to convert.</param>
    /// <returns>Returns a <see cref="string"/> which contains all tags to use in the file selector
    ///  inside <see cref="FluentCxFileManager{TItem}"/></returns>
    public static string Get(AcceptFile value)
    {
        if (value == AcceptFile.None)
        {
            return string.Empty;
        }

        List<string> values = [];

        if (value.HasFlag(AcceptFile.Audio))
        {
            values.Add("audio/*");
        }

        if (value.HasFlag(AcceptFile.Image))
        {
            values.Add("image/*");
        }

        if (value.HasFlag(AcceptFile.Video))
        {
            values.Add("video/*");
        }

        if (value.HasFlag(AcceptFile.Pdf))
        {
            values.AddRange(".pdf");
        }

        if (value.HasFlag(AcceptFile.Excel))
        {
            values.AddRange(".xls", ".xlsx");
        }

        if (value.HasFlag(AcceptFile.Word))
        {
            values.AddRange(".doc", ".docx");
        }

        if (value.HasFlag(AcceptFile.Powerpoint))
        {
            values.AddRange(".ppt", ".pptx");
        }

        return string.Join(", ", values);
    }
}


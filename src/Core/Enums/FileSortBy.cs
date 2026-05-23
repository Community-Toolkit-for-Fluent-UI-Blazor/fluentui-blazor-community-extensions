namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the available sorting keys for the
/// <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public enum FileSortBy
{
    /// <summary>
    /// Sorts files by their name.
    /// </summary>
    Name,

    /// <summary>
    /// Sorts files by their extension (e.g. .pdf, .jpg).
    /// </summary>
    Extension,

    /// <summary>
    /// Sorts files by their size in bytes.
    /// </summary>
    Size,

    /// <summary>
    /// Sorts files by their creation date.
    /// </summary>
    CreatedDate,

    /// <summary>
    /// Sorts files by their last modification date.
    /// </summary>
    ModifiedDate,

    /// <summary>
    /// Sorts files by their type (image, document, video, etc.).
    /// </summary>
    Type
}

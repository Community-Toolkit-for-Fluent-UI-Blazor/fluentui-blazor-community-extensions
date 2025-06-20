namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the way to sort the files.
/// </summary>
public enum FileSortBy
{
    /// <summary>
    /// Sort the file by its name.
    /// </summary>
    Name,

    /// <summary>
    /// Sort the file by its extension.
    /// </summary>
    Extension,

    /// <summary>
    /// Sort the file by its size.
    /// </summary>
    Size,

    /// <summary>
    /// Sort the file by its creation date.
    /// </summary>
    CreatedDate,

    /// <summary>
    /// Sort the file by its modification date.
    /// </summary>
    ModifiedDate
}

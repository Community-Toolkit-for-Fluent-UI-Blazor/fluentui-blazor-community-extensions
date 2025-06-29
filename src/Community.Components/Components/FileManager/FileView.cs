namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the available views for the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public enum FileView
{
    /// <summary>
    /// The items are rendered in a list.
    /// </summary>
    /// <remarks>Only the name of the file is rendered.</remarks>
    List,

    /// <summary>
    /// The items are rendered in a list.
    /// </summary>
    /// <remarks>Each item is rendered with : Name, Size, Creation date information.</remarks>
    Details,

    /// <summary>
    /// The items are rendered in a mosaic view.
    /// </summary>
    Mosaic,

    /// <summary>
    /// The items are rendered in a small icons view.
    /// </summary>
    /// <remarks>The icons are in 24x24 format</remarks>
    SmallIcons,

    /// <summary>
    /// The items are rendered in a medium icons view.
    /// </summary>
    /// <remarks>The icons are in 72x72 format</remarks>
    MediumIcons,

    /// <summary>
    /// The items are rendered in a large icons view.
    /// </summary>
    /// <remarks>The icons are in 96x96 format</remarks>
    LargeIcons,

    /// <summary>
    /// The items are rendered in a very large icons view.
    /// </summary>
    /// <remarks>The icons are in 128x128 format</remarks>
    VeryLargeIcons
}

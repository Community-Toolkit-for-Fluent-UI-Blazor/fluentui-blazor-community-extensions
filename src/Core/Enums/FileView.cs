namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the available visual layouts for the
/// <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public enum FileView
{
    /// <summary>
    /// Displays items in a minimal list layout.
    /// </summary>
    /// <remarks>
    /// Only the file name is shown. This view is optimized for mobile
    /// or compact scenarios where vertical space is limited.
    /// </remarks>
    List,

    /// <summary>
    /// Displays items in a detailed list layout.
    /// </summary>
    /// <remarks>
    /// Shows file name, size, type, and creation/modification dates.
    /// Ideal for desktop usage and precise file management.
    /// </remarks>
    Details,

    /// <summary>
    /// Displays items in a mosaic (grid) layout.
    /// </summary>
    /// <remarks>
    /// Suitable for visual content such as images or media previews.
    /// </remarks>
    Mosaic,

    /// <summary>
    /// Displays items using small icons.
    /// </summary>
    /// <remarks>
    /// Icons are rendered at 24x24 pixels. Useful for dense file lists.
    /// </remarks>
    SmallIcons,

    /// <summary>
    /// Displays items using medium icons.
    /// </summary>
    /// <remarks>
    /// Icons are rendered at 72x72 pixels. Balanced between density and clarity.
    /// </remarks>
    MediumIcons,

    /// <summary>
    /// Displays items using large icons.
    /// </summary>
    /// <remarks>
    /// Icons are rendered at 96x96 pixels. Ideal for visual browsing.
    /// </remarks>
    LargeIcons,

    /// <summary>
    /// Displays items using very large icons.
    /// </summary>
    /// <remarks>
    /// Icons are rendered at 128x128 pixels. Best for image-heavy folders
    /// or accessibility scenarios.
    /// </remarks>
    VeryLargeIcons
}

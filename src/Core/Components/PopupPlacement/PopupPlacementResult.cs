using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the result of a popup placement calculation, including the determined position.
/// </summary>
public sealed record PopupPlacementResult
{
    /// <summary>
    /// Gets the position represented by this instance.
    /// </summary>
    public Point Position { get; init; }

    /// <summary>
    /// Gets the preferred placement of the popup relative to its target element.
    /// </summary>
    public PopupPlacement Placement { get; init; }

    /// <summary>
    /// Gets the size of the popup.
    /// </summary>
    public Size PopupSize { get; init; }

    /// <summary>
    /// Gets the size of the anchor element.
    /// </summary>
    public Size AnchorSize { get; init; }
}

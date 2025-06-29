using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the event args when a component is resized.
/// </summary>
public record ResizedEventArgs
{
    /// <summary>
    /// Gets or sets the identifier of the component.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the resized.
    /// </summary>
    public ResizerHandler Orientation { get; set; }

    /// <summary>
    /// Gets or sets the original size of the component.
    /// </summary>
    public SizeF OriginalSize { get; set; }

    /// <summary>
    /// Gets or sets the new size of the component.
    /// </summary>
    public SizeF NewSize { get; set; }

    /// <summary>
    /// Gets or sets the column span of the component.
    /// </summary>
    public int ColumnSpan { get; set; }

    /// <summary>
    /// Gets or sets the row span of the component.
    /// </summary>
    public int RowSpan { get; set; }
}

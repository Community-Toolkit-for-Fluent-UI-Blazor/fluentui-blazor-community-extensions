namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Encapsulates the properties of a pen used for selection, including color, opacity, and width.
/// </summary>
public sealed class SelectionPayload : IEquatable<SelectionPayload>
{
    /// <summary>
    /// Gets or sets the collection of selected stroke payloads.
    /// </summary>
    public IReadOnlyList<string> StrokeIds { get; set; } = [];

    /// <summary>
    /// Gets or sets the color of the pen, specified as a string. This value determines the visual appearance of the selection outline.
    /// </summary>
    public string Color { get; set; } = "#00A2FF";

    /// <summary>
    /// Gets or sets the opacity of the pen, represented as a double between 0.0 and 1.0. A value of 1.0 indicates full opacity; 0.0
    /// </summary>
    public double Opacity { get; set; }

    /// <summary>
    /// Gets or sets the width of the pen.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the selection should be highlighted.
    /// </summary>
    public bool Highlight { get; set; }

    /// <inheritdoc />
    public bool Equals(SelectionPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (!Enumerable.SequenceEqual(StrokeIds, other.StrokeIds, StringComparer.OrdinalIgnoreCase))
        {
            return false;
        }

        return
            Color == other.Color &&
            Opacity == other.Opacity &&
            Width == other.Width &&
            Highlight == other.Highlight;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) =>
        Equals(obj as SelectionPayload);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(
            StrokeIds,
            Color,
            Opacity,
            Width,
            Highlight
        );
    }
}

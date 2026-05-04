namespace FluentUI.Blazor.Community.Components.Charts.Drawing;

/// <summary>
/// Represents the layout information for a single bar in a bar chart.
/// </summary>
internal sealed class ChartBar
{
    /// <summary>
    /// Gets the identifier of the payload.
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Gets the X coordinate of the column.
    /// </summary>
    public required double X { get; init; }

    /// <summary>
    /// Gets the Y coordinate of the column.
    /// </summary>
    public required double Y { get; init; }

    /// <summary>
    /// Gets the width of the column.
    /// </summary>
    public required double Width { get; init; }

    /// <summary>
    /// Gets the height of the column.
    /// </summary>
    public required double Height { get; init; }

    /// <summary>
    /// Gets the index of the category.
    /// </summary>
    public required int CategoryIndex { get; init; }

    /// <summary>
    /// Gets the value of the column.
    /// </summary>
    public required double Value { get; init; }
}

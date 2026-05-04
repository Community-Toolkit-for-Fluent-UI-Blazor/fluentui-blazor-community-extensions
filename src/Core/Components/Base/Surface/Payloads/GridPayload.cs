namespace FluentUI.Blazor.Community.Components.Surface.Payloads;

/// <summary>
/// Represents an immutable payload for grid-related operations or data transfers.
/// </summary>
public sealed class GridPayload : ILayerPayload, IEquatable<GridPayload>
{
    /// <summary>
    /// Gets the collection of horizontal grid lines to be rendered in the grid layout.
    /// </summary>
    public required IReadOnlyList<GridLinePayload> HorizontalLines { get; init; } = [];

    /// <summary>
    /// Gets the collection of vertical grid lines to be rendered.
    /// </summary>
    public required IReadOnlyList<GridLinePayload> VerticalLines { get; init; } = [];

    /// <summary>
    /// Gets the collection of points that define the grid dots payload.
    /// </summary>
    public required IReadOnlyList<GridPointPayload> Points { get; init; } = [];

    /// <summary>
    /// Gets or sets the display mode of the grid.
    /// </summary>
    public GridDisplayMode DisplayMode { get; set; } = GridDisplayMode.Lines;

    /// <summary>
    /// Gets or sets the size of each cell in the grid, in pixels.
    /// </summary>
    public double CellSize { get; set; } = 20.0;

    /// <summary>
    /// Gets or sets the color of the grid lines or dots.
    /// </summary>
    public string Color { get; set; } = "#cccccc";

    /// <summary>
    /// Gets or sets the opacity of the grid, ranging from 0.0 (fully transparent) to 1.0 (fully opaque).
    /// </summary>
    public double Opacity { get; set; } = 0.5;

    /// <summary>
    /// Gets or sets how often to bold the grid lines. For example, a value of 5 will bold every 5th line.
    /// </summary>
    public int BoldEvery { get; set; } = 5;

    /// <summary>
    /// Gets or sets the width of the grid lines, in pixels.
    /// </summary>
    public double StrokeWidth { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the dash array for the grid lines, allowing for dashed or dotted lines.
    /// </summary>
    public double[] DashArray { get; set; } = [];

    /// <summary>
    /// Gets or sets the radius of the points when the display mode is set to dots, in pixels.
    /// </summary>
    public double PointRadius { get; set; } = 1.5;

    /// <summary>
    /// Gets or sets the grid layer associated with this instance.
    /// </summary>
    public GridLayerOrder Layer { get; set; }

    /// <inheritdoc />
    public bool Equals(GridPayload? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return DisplayMode == other.DisplayMode &&
               CellSize == other.CellSize &&
               Color == other.Color &&
               Opacity == other.Opacity &&
               BoldEvery == other.BoldEvery &&
               StrokeWidth == other.StrokeWidth &&
               DashArray == other.DashArray &&
               PointRadius == other.PointRadius &&
               Layer == other.Layer;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is not GridPayload g)
        {
            return false;
        }

        return this == g;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hash = new HashCode();

        hash.Add(DisplayMode);
        hash.Add(CellSize);
        hash.Add(Color);
        hash.Add(Opacity);
        hash.Add(BoldEvery);
        hash.Add(StrokeWidth);
        hash.Add(DashArray);
        hash.Add(PointRadius);
        hash.Add(Layer);

        return hash.ToHashCode();
    }
}

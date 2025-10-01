namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the grid options for the signature component.
/// </summary>
public class SignatureGridOptions
{
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
    public string? DashArray { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show the X and Y axes in the grid.
    /// </summary>
    public bool ShowAxes { get; set; }

    /// <summary>
    /// Gets or sets the color of the background.
    /// </summary>
    public string BackgroundColor { get; set; } = "transparent";

    /// <summary>
    /// Gets or sets the margin around the grid, in pixels.
    /// </summary>
    public double Margin { get; set; }

    /// <summary>
    /// Gets or sets the radius of the points when the display mode is set to dots, in pixels.
    /// </summary>
    public double PointRadius { get; set; } = 1.5;

    /// <summary>
    /// Resets all properties to their default values.
    /// </summary>
    internal void Reset()
    {
        DisplayMode = GridDisplayMode.Lines;
        CellSize = 20.0;
        Color = "#cccccc";
        Opacity = 0.5;
        BoldEvery = 5;
        StrokeWidth = 1.0;
        DashArray = null;
        ShowAxes = false;
        BackgroundColor = "transparent";
        Margin = 0.0;
        PointRadius = 1.5;
    }
}

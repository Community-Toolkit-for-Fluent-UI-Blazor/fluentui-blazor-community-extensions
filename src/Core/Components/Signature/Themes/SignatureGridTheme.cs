namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the visual theme settings for a signature grid, including display mode, color, opacity, cell size, and
/// other appearance-related properties.
/// </summary>
/// <remarks>Use this class to configure the appearance of a signature grid component, such as customizing line
/// styles, grid spacing, and whether the grid scales with the canvas. The properties allow fine-tuning of the grid's
/// look to match application requirements or user preferences.</remarks>
public class SignatureGridTheme
{
    /// <summary>
    /// Gets or sets the display mode for the grid, determining how grid lines and content are visually presented.
    /// </summary>
    public GridDisplayMode DisplayMode { get; set; } = GridDisplayMode.Lines;

    /// <summary>
    /// Gets or sets the color value represented as a hexadecimal string.
    /// </summary>
    public string Color { get; set; } = "#cccccc";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    public double Opacity { get; set; } = 0.5;

    /// <summary>
    /// Gets or sets the size, in pixels, of each cell within the grid layout.
    /// </summary>
    public double CellSize { get; set; } = 20;

    /// <summary>
    /// Gets or sets the interval at which content is displayed in bold.
    /// </summary>
    public int BoldEvery { get; set; } = 5;

    /// <summary>
    /// Gets or sets the width of the stroke used to render the grid.
    /// </summary>
    public double StrokeWidth { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the dash pattern used to render the grid.
    /// </summary>
    public string? DashArray { get; set; }

    /// <summary>
    /// Gets or sets the radius of the points when the display mode is set to dots, in pixels.
    /// </summary>
    public double PointRadius { get; set; } = 1.5;
}

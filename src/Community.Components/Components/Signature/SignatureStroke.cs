using System.Drawing;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a stroke in a signature.
/// </summary>
public record SignatureStroke
{
    /// <summary>
    /// Gets the points that make up the stroke.
    /// </summary>
    public List<PointF> Points { get; set; } = [];

    /// <summary>
    /// Gets or sets the width of the stroke.
    /// </summary>
    public float Width { get; set; } = 3f;

    /// <summary>
    /// Gets or sets the color of the stroke in HEX format (e.g., "#000000" for black).
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity of the stroke (0.0 to 1.0).
    /// </summary>
    public float Opacity { get; set; } = 1f;

    /// <summary>
    /// Gets or sets a value indicating whether the stroke should be smoothed.
    /// </summary>
    public bool Smooth { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the stroke is an eraser stroke.
    /// </summary>
    public bool Eraser { get; set; }

    /// <summary>
    /// Gets or sets the line style of the stroke.
    /// </summary>
    public SignatureLineStyle LineStyle { get; set; } = SignatureLineStyle.Solid;
}

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the pen options for the signature component.
/// </summary>
public class SignaturePenOptions
{
    /// <summary>
    /// Gets or sets the color of the pen. This can be any valid CSS color value, such as a hex code, RGB, or RGBA.
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the base width of the pen stroke in pixels. This is the default width when pressure sensitivity is not applied.
    /// </summary>
    public double BaseWidth { get; set; } = 2.0;

    /// <summary>
    /// Gets or sets the minimum width of the pen stroke in pixels. This is the thinnest the stroke can be when pressure sensitivity is applied.
    /// </summary>
    public string? DashArray { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether pressure sensitivity is enabled. When true, the stroke width will vary based on the pressure applied (if supported by the input device).
    /// </summary>
    public bool PressureEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether smoothing is applied to the pen strokes. When true, the strokes will be smoothed to reduce jitter and create a more natural appearance.
    /// </summary>
    public bool Smoothing { get; set; } = true;

    /// <summary>
    /// Gets or sets the opacity of the pen strokes, ranging from 0.0 (fully transparent) to 1.0 (fully opaque).
    /// </summary>
    public double Opacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the shape of the ends of the pen strokes. Can be either Butt, Round, or Square.
    /// </summary>
    public LineCap LineCap { get; set; } = LineCap.Round;

    /// <summary>
    /// Gets or sets the shape of the corners where two pen strokes meet. Can be either Bevel, Round, or Miter.
    /// </summary>
    public LineJoin LineJoin { get; set; } = LineJoin.Round;

    /// <summary>
    /// Gets or sets the shadow options for the pen strokes.
    /// </summary>
    public ShadowOptions Shadow { get; set; } = new();

    /// <summary>
    /// Resets all properties to their default values.
    /// </summary>
    public void Reset()
    {
        Color = "#000000";
        BaseWidth = 2.0;
        DashArray = null;
        PressureEnabled = true;
        Smoothing = true;
        Opacity = 1.0;
        LineCap = LineCap.Round;
        LineJoin = LineJoin.Round;
        Shadow.Reset();
    }
}

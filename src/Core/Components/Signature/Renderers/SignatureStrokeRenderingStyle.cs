namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the style settings used to render a stroke, including color, width, opacity, tool type, blend mode, line
/// caps, joins, and advanced signature options.
/// </summary>
public sealed class SignatureStrokeRenderingStyle
{
    /// <summary>
    /// Gets or sets the color value represented as a hexadecimal string.
    /// </summary>
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    public int Opacity { get; set; } = 100;

    /// <summary>
    /// Gets or sets the blend mode used to determine how the stroke's color and opacity are combined with the background and other strokes.
    /// </summary>
    public StrokeBlendMode BlendMode { get; set; } = StrokeBlendMode.Normal;

    /// <summary>
    /// Gets or sets the style used to draw the ends of lines or strokes.
    /// </summary>
    public LineCap LineCap { get; set; } = LineCap.Round;

    /// <summary>
    /// Gets or sets the style used to join the ends of two lines or segments at a corner.
    /// </summary>
    public LineJoin LineJoin { get; set; } = LineJoin.Round;

    /// <summary>
    /// Gets or sets the dash pattern used to render the outline of the shape.
    /// </summary>
    public string? DashArray { get; set; }

    /// <summary>
    /// Gets or sets the shadow options applied to the signature component.
    /// </summary>
    public SignatureShadowOptions Shadow { get; set; } = new();

    /// <summary>
    /// Creates a new instance of the SignatureStrokeRenderingStyle class that is a copy of the current instance.
    /// </summary>
    /// <remarks>The returned object is a deep copy, meaning that changes to the properties of the cloned
    /// instance do not affect the original instance.</remarks>
    /// <returns>A new SignatureStrokeRenderingStyle object with the same property values as the current instance.</returns>
    public SignatureStrokeRenderingStyle Clone() => new()
    {
        Color = Color,
        Opacity = Opacity,
        BlendMode = BlendMode,
        LineCap = LineCap,
        LineJoin = LineJoin,
        DashArray = DashArray,
        Shadow = Shadow.Clone()
    };
}

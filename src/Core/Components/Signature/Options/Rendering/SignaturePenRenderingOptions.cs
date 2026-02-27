namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents rendering options for a signature pen, including color, opacity, shadow, and blend mode settings.
/// </summary>
/// <remarks>Use this class to configure the visual appearance of strokes drawn with a signature pen. These
/// options control how the pen's lines are rendered, allowing customization of color, transparency, shadow effects, and
/// blending behavior. All properties have default values suitable for standard black ink rendering.</remarks>
public sealed class SignaturePenRenderingOptions
{
    /// <summary>
    /// Represents the color of the pen.
    /// </summary>
    private string _color = "#000000";

    /// <summary>
    /// Represents the opacity level of the pen, where 1.0 is fully opaque and 0.0 is fully transparent.
    /// </summary>
    private int _opacity = 100;

    /// <summary>
    /// Represents the blend mode used when rendering strokes with the pen, determining how the pen's strokes interact.
    /// </summary>
    private StrokeBlendMode _blendMode;

    /// <summary>
    /// Represents the linecap.
    /// </summary>
    private LineCap _lineCap = LineCap.Round;

    /// <summary>
    /// Represents the linejoin.
    /// </summary>
    private LineJoin _lineJoin = LineJoin.Round;

    /// <summary>
    /// Represents the dash pattern used to render the outline of a shape, specified as a comma-separated string of numbers.
    /// </summary>
    private string? _dashArray;

    /// <summary>
    /// Represents the dash style used for rendering lines.
    /// </summary>
    private LineDash _lineDash;

    /// <summary>
    /// Occurs when the options associated with this instance have changed.
    /// </summary>
    public event EventHandler? OptionsChanged;

    /// <summary>
    /// Gets or sets the color value represented as a hexadecimal string.
    /// </summary>
    /// <remarks>The color should be specified in standard hexadecimal format (for example,
    /// "#RRGGBB").</remarks>
    public string Color
    {
        get => _color;
        set
        {
            if (!string.Equals(_color, value, StringComparison.OrdinalIgnoreCase))
            {
                _color = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the opacity level of the component.
    /// </summary>
    /// <remarks>The opacity value determines the transparency of the component, where 100 is fully opaque and
    /// 0 is fully transparent. Values outside the range of 0 to 100 may not be supported and could result in    /// undefined behavior.</remarks>
    public int Opacity
    {
        get => _opacity;
        set
        {
            if (_opacity != value)
            {
                _opacity = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the shadow options applied to the component.
    /// </summary>
    /// <remarks>Use this property to configure visual shadow effects, such as elevation or blur, for the
    /// component. The settings in the ShadowOptions object determine how the shadow is rendered.</remarks>
    public SignatureShadowOptions Shadow { get; set; } = new();

    /// <summary>
    /// Gets or sets the blend mode used when rendering strokes with the pen.
    /// </summary>
    public StrokeBlendMode BlendMode
    {
        get => _blendMode;
        set
        {
            if (_blendMode != value)
            {
                _blendMode = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the style used to cap the ends of lines when rendering.
    /// </summary>
    /// <remarks>The line cap style determines how the endpoints of a line are drawn. Common values include
    /// round, square, and flat. Changing this property affects the visual appearance of lines rendered by the
    /// component.</remarks>
    public LineCap LineCap
    {
        get => _lineCap;
        set
        {
            if (_lineCap != value)
            {
                _lineCap = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the style used to join lines at their intersection points.
    /// </summary>
    /// <remarks>The line join style determines how the corners are rendered when two lines meet. Common
    /// values include round, bevel, and miter. Changing this property affects the visual appearance of line corners in
    /// the rendered output.</remarks>
    public LineJoin LineJoin
    {
        get => _lineJoin;
        set
        {
            if (_lineJoin != value)
            {
                _lineJoin = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the dash pattern used to render the outline of a shape.
    /// </summary>
    /// <remarks>The dash pattern is specified as a comma-separated string of numbers, where each number
    /// represents the length of a dash or gap. For example, "5,2" creates a pattern of 5 units dash followed by 2 units
    /// gap. If the value is null or empty, a solid line is rendered.</remarks>
    public string? DashArray
    {
        get => _dashArray;
        set
        {
            if (!string.Equals(_dashArray, value, StringComparison.OrdinalIgnoreCase))
            {
                _dashArray = value;
                Notify();
            }
        }
    }

    /// <summary>
    /// Gets or sets the dash style used when rendering lines.
    /// </summary>
    /// <remarks>Use this property to specify whether lines should be solid, dashed, or use a custom dash
    /// pattern. Changing this property will update the appearance of rendered lines accordingly.</remarks>
    public LineDash LineDash
    {
        get => _lineDash;
        set
        {
            if (_lineDash != value)
            {
                _lineDash = value;
                DashArray = value switch
                {
                    LineDash.Solid => null,
                    LineDash.DashDot => "6,3,1,3",
                    LineDash.Dots => "1,3",
                    LineDash.Dashes => "6,3",
                    _ => null
                };
            }
        }
    }

    /// <summary>
    /// Resets all stroke settings to their default values.
    /// </summary>
    /// <remarks>Use this method to restore the stroke configuration to its initial state. This is useful when
    /// you want to clear any customizations and revert to the standard settings for drawing operations.</remarks>
    public void Reset()
    {
        DashArray = null;
        LineCap = LineCap.Round;
        LineJoin = LineJoin.Round;
        BlendMode = StrokeBlendMode.Normal;
        Opacity = 100;
        Color = "#000000";
        Shadow.Reset();
    }

    /// <summary>
    /// Raises the OptionsChanged event to notify subscribers of changes to the options.
    /// </summary>
    /// <remarks>Call this method to signal that the options have been updated. Subscribers to the
    /// OptionsChanged event will be notified. This method does not perform any validation or state changes beyond
    /// raising the event.</remarks>
    private void Notify()
    {
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }
}


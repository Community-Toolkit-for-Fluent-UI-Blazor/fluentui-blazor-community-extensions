namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the layout configuration for an individual item in a sleek dial control, including its position,
/// visibility, and animation settings.
/// </summary>
/// <remarks>Use this class to specify the screen coordinates, rotation angle, radius, and animation properties
/// for a dial item. Adjusting these properties enables dynamic arrangement and visual effects for dial-based user
/// interfaces. This class is typically used in conjunction with a dial component to control the appearance and behavior
/// of its items.</remarks>
public class SleekDialItemLayout
{
    /// <summary>
    /// Gets or sets the X coordinate value.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the point in two-dimensional space.
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Gets or sets the angle, in degrees, measured clockwise from the positive x-axis.
    /// </summary>
    public double Angle { get; set; }

    /// <summary>
    /// Gets or sets the radius of the circle.
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// Gets or sets the duration to wait before the animation begins.
    /// </summary>
    public TimeSpan AnimationDelay { get; set; }

    /// <summary>
    /// Gets or sets the transformation string applied to the data.
    /// </summary>
    public string Transform { get; set; } = "";

    /// <summary>
    /// Gets or sets the transition effect applied during state changes.
    /// </summary>
    public string Transition { get; set; } = "";

    /// <summary>
    /// Gets or sets the final transformation string to be applied.
    /// </summary>
    public string FinalTransform { get; set; } = "";

    /// <summary>
    /// Gets or sets the initial opacity level of the element, ranging from 0 (completely transparent) to 1 (completely
    /// opaque).
    /// </summary>
    /// <remarks>The default value is 1, indicating full opacity. Adjusting this property affects the
    /// visibility of the element when rendered.</remarks>
    public double InitialOpacity { get; set; }

    /// <summary>
    /// Gets or sets the final opacity level of the element, ranging from 0 (completely transparent) to 1 (completely
    /// opaque).
    /// </summary>
    /// <remarks>This property determines the visual transparency of the element when rendered. Adjusting the
    /// FinalOpacity can affect the appearance of overlapping elements and animations.</remarks>
    public double FinalOpacity { get; set; } = 1;

    /// <summary>
    /// Gets or sets the CSS class to apply to the element.
    /// </summary>
    /// <remarks>This property allows for the customization of the element's appearance through CSS. The
    /// default value is an empty string, indicating no CSS class is applied.</remarks>
    public string CssClass { get; set; } = "";
}

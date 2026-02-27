using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the configuration options for the pen tool component.
/// </summary>
/// <remarks>Use this class to specify settings and behaviors for the pen tool within the Fluent UI Blazor
/// extension. The options are initialized based on the provided library configuration, which must not be
/// null.</remarks>
public partial class PenToolOptions
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the PenToolOptions class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the pen tool options. Cannot be null.</param>
    public PenToolOptions(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the color value for the component, specified as a CSS color string.
    /// </summary>
    /// <remarks>The color can be provided in any valid CSS color format, such as hexadecimal, RGB, RGBA, HSL,
    /// or named colors. The default value is "#000000" (black).</remarks>
    [Parameter]
    public string Color { get; set; } = "#000000";

    /// <summary>
    /// Gets or sets the callback that is invoked when the color value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the selected color. The callback receives the new
    /// color value as a string, typically in a standard color format such as hexadecimal.</remarks>
    [Parameter]
    public EventCallback<string> ColorChanged { get; set; }

    /// <summary>
    /// Gets or sets the opacity level of the component as a percentage.
    /// </summary>
    /// <remarks>Valid values range from 0 (completely transparent) to 1 (fully opaque). Values outside this
    /// range may not be rendered as expected.</remarks>
    [Parameter]
    public int Opacity { get; set; } = 100;

    /// <summary>
    /// Gets or sets the callback that is invoked when the opacity value changes.
    /// </summary>
    /// <remarks>Use this property to handle changes to the opacity value in the parent component. The
    /// callback receives the new opacity as an integer parameter.</remarks>
    [Parameter]
    public EventCallback<int> OpacityChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the settings button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnSettingsClick { get; set; }

    /// <summary>
    /// Gets or sets the style used to cap the ends of lines when rendering the component.
    /// </summary>
    /// <remarks>Use this property to specify how the line endings should appear, such as flat, round, or
    /// square. The value determines the visual appearance of the line's endpoints.</remarks>
    [Parameter]
    public LineCap LineCap { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the line cap value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the line cap style, such as updating related UI
    /// elements or triggering additional logic when the value is modified.</remarks>
    [Parameter]
    public EventCallback<LineCap> LineCapChanged { get; set; }

    /// <summary>
    /// Gets or sets the style used to join consecutive line segments when rendering shapes.
    /// </summary>
    /// <remarks>Use this property to control the appearance of corners where two lines meet. The value
    /// determines whether corners are rendered with a miter, bevel, or round join, depending on the selected option in
    /// the LineJoin enumeration.</remarks>
    [Parameter]
    public LineJoin LineJoin { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the line join value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the line join property, such as updating UI elements
    /// or triggering additional logic when the value is modified.</remarks>
    [Parameter]
    public EventCallback<LineJoin> LineJoinChanged { get; set; }

    /// <summary>
    /// Gets or sets the dash style to use when rendering the line.
    /// </summary>
    [Parameter]
    public LineDash LineDash { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the line dash value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the line dash pattern, such as updating related UI
    /// elements or synchronizing state with other components.</remarks>
    [Parameter]
    public EventCallback<LineDash> LineDashChanged { get; set; }

    /// <summary>
    /// Gets or sets the width of the component, in device-independent units (pixels).
    /// </summary>
    [Parameter]
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the width value changes.
    /// </summary>
    [Parameter]
    public EventCallback<double> WidthChanged { get; set; }

    /// <summary>
    /// Gets or sets the mode used to determine the stroke width for rendering the component.
    /// </summary>
    /// <remarks>Use this property to control how the stroke width is calculated or applied. The selected mode
    /// may affect the visual appearance and scaling behavior of strokes within the component.</remarks>
    [Parameter]
    public StrokeWidthMode StrokeWidthMode { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the stroke width mode changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the stroke width mode, such as updating UI elements
    /// or triggering additional logic when the mode is modified.</remarks>
    [Parameter]
    public EventCallback<StrokeWidthMode> StrokeWidthModeChanged { get; set; }
}

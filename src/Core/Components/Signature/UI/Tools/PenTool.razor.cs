using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a pen tool component used within a signature input context, providing configuration and activation logic
/// for pen-based input.
/// </summary>
/// <remarks>This component is designed to be used as part of a signature capture interface, typically within a
/// FluentCxSignature parent component. It exposes options for customization and integrates with the parent to manage
/// tool selection and activation. The pen tool can be extended with custom options via the CustomOptions parameter, and
/// provides a default configuration through the DefaultOptions parameter.</remarks>
public partial class PenTool
    : FluentComponentBase, ISignatureTool
{
    /// <summary>
    /// Gets or sets the parent signature component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a parent FluentCxSignature component. It enables child components to access shared context or data
    /// from their parent signature component.</remarks>
    [CascadingParameter]
    private FluentCxSignature? Parent { get; set; }

    /// <summary>
    /// Gets or sets a custom render fragment that defines additional options to display for the pen tool.
    /// </summary>
    [Parameter]
    public RenderFragment? CustomOptions { get; set; }

    /// <summary>
    /// Gets or sets the default options that define the standard configuration for the pen tool.
    /// </summary>
    [Parameter]
    public RenderFragment DefaultOptions { get; set; }

    /// <inheritdoc />
    [Parameter]
    public bool IsActive { get; set; }

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
    /// <remarks>Valid values range from 0 (completely transparent) to 100 (fully opaque). Values outside this
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
    /// Gets or sets the style of line endings to use when rendering lines.
    /// </summary>
    /// <remarks>Use this property to specify how the ends of lines are drawn, such as flat, round, or square
    /// caps. The selected value affects the visual appearance of line terminations in the rendered component.</remarks>
    [Parameter]
    public LineCap LineCap { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the line cap value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the line cap selection, such as updating related UI
    /// elements or handling business logic when the user selects a different line cap style.</remarks>
    [Parameter]
    public EventCallback<LineCap> LineCapChanged { get; set; }

    /// <summary>
    /// Gets or sets the style of line join to use when rendering lines.
    /// </summary>
    [Parameter]
    public LineJoin LineJoin { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the line join value changes.
    /// </summary>
    [Parameter]
    public EventCallback<LineJoin> LineJoinChanged { get; set; }

    /// <summary>
    /// Gets or sets the dash style to use when rendering the line.
    /// </summary>
    /// <remarks>Use this property to specify whether the line should be solid, dashed, or use a custom dash
    /// pattern. The appearance of the line is determined by the selected value.</remarks>
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
    /// Gets or sets the callback that is invoked when the pen settings are clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnSettingsClicked { get; set; }

    /// <summary>
    /// Gets or sets the width of the component, in device-independent units (DIPs).
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

    /// <summary>
    /// Activates the current instance, enabling its primary functionality.
    /// </summary>
    public void Activate()
    {
        Parent?.Active(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.Active(this);
    }
}

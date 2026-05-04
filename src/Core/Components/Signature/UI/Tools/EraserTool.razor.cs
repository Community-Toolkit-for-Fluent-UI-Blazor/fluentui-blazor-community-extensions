using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an eraser tool component used within a signature input context, providing configuration and activation logic
/// for pen-based input.
/// </summary>
/// <remarks>This component is designed to be used as part of a signature capture interface, typically within a
/// FluentCxSignature parent component. It exposes options for customization and integrates with the parent to manage
/// tool selection and activation. The pen tool can be extended with custom options via the CustomOptions parameter, and
/// provides a default configuration through the DefaultOptions parameter.</remarks>
public partial class EraserTool
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
    /// Gets or sets the size of the eraser.
    /// </summary>
    [Parameter]
    public int Size { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the size changes.
    /// </summary>
    [Parameter]
    public EventCallback<int> SizeChanged { get; set; }

    /// <summary>
    /// Gets or sets the shape of the eraser.
    /// </summary>
    [Parameter]
    public EraserShape Shape { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the shape changes.
    /// </summary>
    [Parameter]
    public EventCallback<EraserShape> ShapeChanged { get; set; }

    /// <summary>
    /// Gets or sets the mode of the eraser.
    /// </summary>
    [Parameter]
    public EraserMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the mode changes.
    /// </summary>
    [Parameter]
    public EventCallback<EraserMode> ModeChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the settings button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnSettingsClick { get; set; }

    /// <summary>
    /// Activates the current instance, enabling its primary functionality.
    /// </summary>
    public void Activate()
    {
        Parent?.Active(this);
    }
}

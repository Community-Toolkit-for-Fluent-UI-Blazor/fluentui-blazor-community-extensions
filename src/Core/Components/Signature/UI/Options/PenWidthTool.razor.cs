using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a UI component that provides functionality for selecting or adjusting pen width in drawing or annotation
/// scenarios.
/// </summary>
/// <remarks>Use this component within Fluent UI Blazor-based applications to allow users to choose or modify the
/// width of a pen or brush. This tool is typically used in drawing, sketching, or signature input interfaces.</remarks>
public partial class PenWidthTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the PenWidthTool class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the tool. Cannot be null.</param>
    public PenWidthTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent signature component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set by the Blazor framework to provide access to the nearest
    /// enclosing FluentCxSignature component. It enables child components to interact with or obtain information from
    /// their parent signature component within the component tree.</remarks>
    [CascadingParameter]
    private FluentCxSignature? Parent { get; set; }

    /// <summary>
    /// Gets or sets the width of the pen.
    /// </summary>
    [Parameter]
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the minimum allowed value.
    /// </summary>
    private double Min => Parent?.GetPenEngineOptions().MinWidth ?? 0.5;

    /// <summary>
    /// Gets or sets the increment value used when adjusting the component's value.
    /// </summary>
    [Parameter]
    public double Step { get; set; } = 0.1;

    /// <summary>
    /// Gets or sets the maximum allowable value for the component.
    /// </summary>
    private double Max => Parent?.GetPenEngineOptions().MaxWidth ?? 4;

    /// <summary>
    /// Gets or sets the callback that is invoked when the width value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the width, such as updating related UI elements or
    /// triggering additional logic when the width is modified.</remarks>
    [Parameter]
    public EventCallback<double> WidthChanged { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Width = Parent?.GetPenEngineOptions().BaseWidth ?? 2.0;
    }

    /// <summary>
    /// Invokes the width changed event asynchronously when the value changes.
    /// </summary>
    /// <remarks>The event is only invoked if a delegate is assigned to the width changed event.</remarks>
    /// <param name="value">The new width value to be passed to the event handler.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnValueChangedAsync(double value)
    {
        Width = value;

        if (WidthChanged.HasDelegate)
        {
            await WidthChanged.InvokeAsync(value);
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that allows users to select the shape of the eraser tool in a Fluent UI Blazor application.
/// </summary>
/// <remarks>Use this component to provide eraser shape selection functionality within drawing or annotation
/// interfaces. The component exposes parameters for the current eraser shape and a callback for shape changes, enabling
/// integration with parent components that need to respond to user selections.</remarks>
public partial class EraserShapeTool
     : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SizeTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public EraserShapeTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the shape of the eraser used in the component.
    /// </summary>
    [Parameter]
    public EraserShape Shape { get; set; } = EraserShape.Square;

    /// <summary>
    /// Gets or sets the callback that is invoked when the eraser shape changes.
    /// </summary>
    /// <remarks>Use this callback to respond to changes in the eraser shape selection. The callback receives
    /// the new shape as an argument, allowing the parent component to update its state or perform additional actions
    /// when the shape changes.</remarks>
    [Parameter]
    public EventCallback<EraserShape> ShapeChanged { get; set; }

    /// <summary>
    /// Handles changes to the shape size and notifies subscribers of the updated value asynchronously.
    /// </summary>
    /// <remarks>If there are subscribers to the size change event, this method invokes the event
    /// asynchronously with the new value.</remarks>
    /// <param name="value">The new size value to apply. Represents the updated shape size.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnShapeChangedAsync(EraserShape value)
    {
        Shape = value;

        if (ShapeChanged.HasDelegate)
        {
            await ShapeChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// Handles the selection event for a menu item asynchronously.
    /// </summary>
    /// <param name="e">The event data associated with the selected menu item. Contains information such as the checked state and the
    /// text of the item.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnItemSelectedAsync(MenuItemEventArgs e)
    {
        if (e.Checked == true)
        {
            if (e.Text == "Circle")
            {
                await OnShapeChangedAsync(EraserShape.Circle);
            }
            else if (e.Text == "Square")
            {
                await OnShapeChangedAsync(EraserShape.Square);
            }
        }
    }
}

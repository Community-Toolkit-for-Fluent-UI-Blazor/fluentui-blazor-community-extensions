using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a tool component that allows users to select and change the eraser shape in the UI.
/// </summary>
/// <remarks>This component is intended for use within Fluent UI Blazor-based applications where eraser shape
/// selection is required. It provides parameters for the current eraser shape and a callback for shape changes,
/// enabling parent components to react to user selections.</remarks>
public sealed partial class EraserModeTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EraserModeTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public EraserModeTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the mode of the eraser used in the component.
    /// </summary>
    [Parameter]
    public EraserMode Mode { get; set; } = EraserMode.Pixel;

    /// <summary>
    /// Gets or sets the callback that is invoked when the eraser shape changes.
    /// </summary>
    /// <remarks>Use this callback to respond to changes in the eraser shape selection. The callback receives
    /// the new shape as an argument, allowing the parent component to update its state or perform additional actions
    /// when the shape changes.</remarks>
    [Parameter]
    public EventCallback<EraserMode> ModeChanged { get; set; }

    /// <summary>
    /// Handles changes to the shape size and notifies subscribers of the updated value asynchronously.
    /// </summary>
    /// <remarks>If there are subscribers to the size change event, this method invokes the event
    /// asynchronously with the new value.</remarks>
    /// <param name="value">The new size value to apply. Represents the updated shape size.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnModeChangedAsync(EraserMode value)
    {
        Mode = value;

        if (ModeChanged.HasDelegate)
        {
            await ModeChanged.InvokeAsync(value);
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
            if (e.Text == "Pixel")
            {
                await OnModeChangedAsync(EraserMode.Pixel);
            }
            else if (e.Text == "Stroke")
            {
                await OnModeChangedAsync(EraserMode.Stroke);
            }
            else if (e.Text == "Hybrid")
            {
                await OnModeChangedAsync(EraserMode.Hybrid);
            }
        }
    }
}

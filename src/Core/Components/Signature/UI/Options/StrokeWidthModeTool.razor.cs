using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a tool for managing stroke width modes in drawing or design applications.
/// </summary>
/// <remarks>Use this class to configure or interact with stroke width settings when customizing drawing tools.
/// The specific behavior and available modes depend on the implementation and context in which the tool is
/// used.</remarks>
public partial class StrokeWidthModeTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrokeWidthModeTool"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the pen tool options. Cannot be null.</param>
    public StrokeWidthModeTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the stroke width mode used for rendering.
    /// </summary>
    /// <remarks>Use this property to specify how stroke widths are interpreted or applied. The selected mode
    /// may affect the visual appearance of rendered elements.</remarks>
    [Parameter]
    public StrokeWidthMode Mode { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the stroke width mode changes.
    /// </summary>
    /// <remarks>Use this property to handle changes in the stroke width mode, typically in response to user
    /// interaction. The callback receives the new mode as its argument.</remarks>
    [Parameter]
    public EventCallback<StrokeWidthMode> ModeChanged { get; set; }

    /// <summary>
    /// Handles changes to the stroke width mode and notifies listeners of the updated mode.
    /// </summary>
    /// <remarks>This method updates the current mode and triggers the associated event to inform subscribers
    /// of the change. Use this method to ensure mode changes are properly communicated within the component.</remarks>
    /// <param name="value">The new stroke width mode to apply. Specifies the mode that will be set and propagated to listeners.</param>
    /// <returns></returns>
    private async Task OnModeChangedAsync(StrokeWidthMode value)
    {
        Mode = value;
        await ModeChanged.InvokeAsync(value);
    }

    /// <summary>
    /// Handles the click event for a menu item and triggers a line join change if the item is checked.
    /// </summary>
    /// <remarks>The method only processes the event if the menu item is checked. If the item's identifier
    /// corresponds to a valid line join value, the line join change is initiated asynchronously.</remarks>
    /// <param name="e">The event arguments containing information about the clicked menu item, including its checked state and
    /// identifier.</param>
    /// <returns>A task that represents the asynchronous operation of handling the menu item click.</returns>
    private async Task OnMenuItemClickAsync(MenuItemEventArgs e)
    {
        if (e.Checked == true)
        {
            var split = e.Id?.Split('-');
            var first = split?.FirstOrDefault();

            if (Enum.TryParse<StrokeWidthMode>(first, out var mode))
            {
                await OnModeChangedAsync(mode);
            }
            else
            {
                await OnModeChangedAsync(StrokeWidthMode.Default);
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a tool for configuring line cap styles in Fluent UI Blazor components.
/// </summary>
/// <remarks>Use this component to select or modify the appearance of line endpoints in drawing scenarios. This
/// tool is intended for integration with Fluent UI Blazor extensions and follows Fluent UI design principles.</remarks>
public partial class LineJoinTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LineCapTool"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the pen tool options. Cannot be null.</param>
    public LineJoinTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the style used to join lines at their intersection points.
    /// </summary>
    /// <remarks>Use this property to specify how the corners between connected lines are rendered. The value
    /// determines whether the join is mitered, beveled, or rounded, affecting the visual appearance of line
    /// connections.</remarks>
    [Parameter]
    public LineJoin LineJoin { get; set; } 

    /// <summary>
    /// Gets or sets the callback that is invoked when the line join value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the line join property, such as updating UI or
    /// triggering additional logic when the value is modified.</remarks>
    [Parameter]
    public EventCallback<LineJoin> LineJoinChanged { get; set; }

    /// <summary>
    /// Handles changes to the line join style and notifies subscribers of the updated value.
    /// </summary>
    /// <remarks>This method updates the line join property and triggers the associated change event. Use this
    /// method to respond to user input or programmatic changes affecting line join rendering.</remarks>
    /// <param name="value">The new line join style to apply. Specifies how the corners between lines are rendered.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLineJoinChanged(LineJoin value)
    {
        LineJoin = value;
        await LineJoinChanged.InvokeAsync(value);
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
            if (Enum.TryParse<LineJoin>(e.Id, out var lineJoin))
            {
                await OnLineJoinChanged(lineJoin);
            }
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a tool for configuring line cap styles in Fluent UI Blazor components.
/// </summary>
/// <remarks>Use this component to select or modify the appearance of line endpoints in drawing scenarios. This
/// tool is intended for integration with Fluent UI Blazor extensions and follows Fluent UI design principles.</remarks>
public partial class LineCapTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LineCapTool"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the pen tool options. Cannot be null.</param>
    public LineCapTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the style of the line cap used when rendering the component.
    /// </summary>
    /// <remarks>Use this property to specify how the ends of lines are drawn. The value determines whether
    /// the line ends are flat, rounded, or squared, which can affect the visual appearance of strokes in the
    /// component.</remarks>
    [Parameter]
    public LineCap LineCap { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the line cap value changes.
    /// </summary>
    /// <remarks>Use this event to respond to changes in the line cap selection, such as updating related UI
    /// elements or triggering additional logic.</remarks>
    [Parameter]
    public EventCallback<LineCap> LineCapChanged { get; set; }

    /// <summary>
    /// Handles changes to the line cap style and notifies subscribers of the updated value.
    /// </summary>
    /// <remarks>Use this method to update the line cap style and trigger any associated event handlers. The
    /// operation is asynchronous to support event-driven scenarios.</remarks>
    /// <param name="value">The new line cap style to apply. Specifies how the ends of lines are rendered.</param>
    /// <returns>A task that represents the asynchronous operation of notifying subscribers about the line cap change.</returns>
    private async Task OnLineCapChanged(LineCap value)
    {
        LineCap = value;

        if (LineCapChanged.HasDelegate)
        {
            await LineCapChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// Handles the click event for a menu item and triggers a line cap change if the item is checked.
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
            if (Enum.TryParse<LineCap>(e.Id, out var lineCap))
            {
                await OnLineCapChanged(lineCap);
            }
        }
    }
}

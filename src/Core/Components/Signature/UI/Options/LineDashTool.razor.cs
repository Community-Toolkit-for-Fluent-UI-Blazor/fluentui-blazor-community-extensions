using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a tool for configuring line dash styles in Fluent UI Blazor components.
/// </summary>
/// <remarks>Use this component to select or modify the appearance of line endpoints in drawing scenarios. This
/// tool is intended for integration with Fluent UI Blazor extensions and follows Fluent UI design principles.</remarks>
public partial class LineDashTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LineDashTool"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the pen tool options. Cannot be null.</param>
    public LineDashTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the style of the line dash used when rendering the component.
    /// </summary>
    [Parameter]
    public LineDash LineDash { get; set; } 

    /// <summary>
    /// Gets or sets the callback that is invoked when the line dash value changes.
    /// </summary>
    [Parameter]
    public EventCallback<LineDash> LineDashChanged { get; set; }

    /// <summary>
    /// Handles changes to the line dash style and notifies listeners of the updated value.
    /// </summary>
    /// <param name="value">The new line dash style to apply. Represents the updated value that will be propagated to listeners.</param>
    /// <returns>A task that represents the asynchronous operation of notifying listeners about the line dash change.</returns>
    private async Task OnLineDashChangedAsync(LineDash value)
    {
        LineDash = value;

        if (LineDashChanged.HasDelegate)
        {
            await LineDashChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// Handles the menu item click event asynchronously and updates the line dash style if the item is checked.
    /// </summary>
    /// <remarks>This method only triggers a line dash change when the menu item is checked. It is intended to
    /// be used in response to user interactions with menu items.</remarks>
    /// <param name="e">The event arguments containing information about the clicked menu item, including its checked state and
    /// identifier.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnMenuItemClickAsync(MenuItemEventArgs e)
    {
        if (e.Checked == true)
        {
            var lineDash = GetLineDash(e.Id);
            await OnLineDashChangedAsync(lineDash);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    private static LineDash GetLineDash(string? id)
    {
        var split = id?.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (Enum.TryParse<LineDash>(split?.FirstOrDefault(), true, out var lineDash))
        {
            return lineDash;
        }

        return LineDash.Solid;
    }
}

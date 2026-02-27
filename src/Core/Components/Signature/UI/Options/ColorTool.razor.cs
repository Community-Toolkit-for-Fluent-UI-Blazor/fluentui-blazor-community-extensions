using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a color selection tool component that allows users to choose and update a color value within a Fluent UI
/// Blazor application.
/// </summary>
/// <remarks>This component is designed to integrate with the Fluent UI Blazor library and supports two-way
/// binding for the selected color value. Use the Color property to specify or retrieve the current color, and handle
/// the ColorChanged event to respond to user changes.</remarks>
public partial class ColorTool
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ColorTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public ColorTool(LibraryConfiguration configuration)
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
    /// Gets or sets the CSS width of the component.
    /// </summary>
    /// <remarks>Specify a valid CSS width value, such as "120px", "50%", or "auto". The default value is
    /// "120px".</remarks>
    [Parameter]
    public string Width { get; set; } = "120px";

    /// <summary>
    /// Invokes the color changed event asynchronously if a delegate is assigned.
    /// </summary>
    /// <remarks>Use this method to notify subscribers when the color value changes. The event is only invoked
    /// if a delegate has been assigned to handle the color change.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnColorChangedAsync()
    {
        if (ColorChanged.HasDelegate)
        {
            await ColorChanged.InvokeAsync(Color);
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that allows users to adjust and respond to opacity changes as a percentage value.
/// </summary>
/// <remarks>Use this component to provide interactive opacity control within Fluent UI Blazor applications. The
/// component exposes an opacity value and a callback for handling changes, enabling integration with parent components
/// or other UI logic.</remarks>
public partial class OpacityTool
     : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpacityTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public OpacityTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the opacity level of the component as a percentage.
    /// </summary>
    /// <remarks>Valid values range from 0 (completely transparent) to 1 (fully opaque). Values outside this
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
    /// Invokes the color changed event asynchronously if a delegate is assigned.
    /// </summary>
    /// <param name="value">The new opacity value as an integer percentage (0-100).</param>
    /// <remarks>Use this method to notify subscribers when the color value changes. The event is only invoked
    /// if a delegate has been assigned to handle the color change.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnOpacityChangedAsync(int value)
    {
        Opacity = value;

        if (OpacityChanged.HasDelegate)
        {
            await OpacityChanged.InvokeAsync(value);
        }
    }
}

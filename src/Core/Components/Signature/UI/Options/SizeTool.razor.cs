using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that allows users to adjust and respond to size changes.
/// </summary>
/// <remarks>Use this component to provide interactive opacity control within Fluent UI Blazor applications. The
/// component exposes an opacity value and a callback for handling changes, enabling integration with parent components
/// or other UI logic.</remarks>
public partial class SizeTool
     : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SizeTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public SizeTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the size value used by the component.
    /// </summary>
    /// <remarks>The size determines the number of items or the extent of the component, depending on the
    /// specific usage context. The default value is 10.</remarks>
    [Parameter]
    public int Size { get; set; } = 10;

    /// <summary>
    /// Gets or sets the callback that is invoked when the size value changes.
    /// </summary>
    /// <remarks>Use this property to handle changes to the size value in the parent component. The
    /// callback receives the new size as an integer parameter.</remarks>
    [Parameter]
    public EventCallback<int> SizeChanged { get; set; }

    /// <summary>
    /// Invokes the color changed event asynchronously if a delegate is assigned.
    /// </summary>
    /// <param name="value">The new opacity value as an integer percentage (0-100).</param>
    /// <remarks>Use this method to notify subscribers when the color value changes. The event is only invoked
    /// if a delegate has been assigned to handle the color change.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnSizeChangedAsync(int value)
    {
        Size = value;

        if (SizeChanged.HasDelegate)
        {
            await SizeChanged.InvokeAsync(value);
        }
    }
}

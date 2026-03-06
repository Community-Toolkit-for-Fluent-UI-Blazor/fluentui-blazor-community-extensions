using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a button that displays overflow items in a trail menu, allowing users to access additional options.
/// </summary>
/// <remarks>The <see cref="TrailOverflowButton"/> must be used within a <see cref="FluentCxTrailMenu"/> to
/// function correctly. It manages a collection of menu items that can be displayed when the button is
/// activated.</remarks>
public partial class TrailOverflowButton
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrailOverflowButton"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration object used to apply default values to the component.</param>
    public TrailOverflowButton(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the items inside the overflow.
    /// </summary>
    [Parameter]
    public List<ITrailMenuItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the parent of the button.
    /// </summary>
    [CascadingParameter]
    private FluentCxTrailMenu? Parent { get; set; }

    /// <summary>
    /// Handles the tap event for a menu trail item and updates the parent trail asynchronously.
    /// </summary>
    /// <remarks>Call this method when a user taps a menu item to ensure the parent trail reflects the new
    /// selection.</remarks>
    /// <param name="item">The menu trail item that was tapped. This item is used to update the trail state.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnItemTappedAsync(ITrailMenuItem item)
    {
        await Parent!.UpdateTrailAsync(item);
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("TrailOverflowButton must be used within a FluentCxTrailMenu.");
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a menu list for the FluentCxTrailMenu.
/// </summary>
public partial class TrailMenuList
    : FluentComponentBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="TrailMenuList"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration object used to apply default values to the component.</param>
    public TrailMenuList(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the collection of menu items to display in the trail menu.
    /// </summary>
    /// <remarks>The collection can be updated at runtime to reflect changes in the available menu options.
    /// Modifying this property will update the items rendered in the trail menu component.</remarks>
    [Parameter]
    public IEnumerable<ITrailMenuItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the parent menu component associated with the current menu item.
    /// </summary>
    /// <remarks>This property is used to establish a hierarchical relationship between menu items. It is
    /// essential for managing the menu structure and ensuring proper navigation within the application.</remarks>
    [CascadingParameter]
    private FluentCxTrailMenu Parent { get; set; } = null!;

    /// <summary>
    /// Handles the tap event for a trail menu item and updates the trail selection asynchronously.
    /// </summary>
    /// <remarks>Call this method in response to a user tapping a trail menu item to ensure the trail is
    /// updated accordingly. The operation is performed asynchronously and may involve UI updates or
    /// navigation.</remarks>
    /// <param name="item">The trail menu item that was tapped. This item determines which trail will be updated.</param>
    /// <returns>A task that represents the asynchronous operation of updating the trail selection.</returns>
    private async Task OnItemTappedAsync(ITrailMenuItem item)
    {
        await Parent.UpdateTrailAsync(item);
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

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a dot indicator for the <see cref="FluentCxSlideshow{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class DotIndicator<TItem>
    : FluentComponentBase
{
    /// <summary>
    /// Gets the aria-label.
    /// </summary>
    private string AriaLabel => $"Item {Index}";

    /// <summary>
    /// Gets or sets the index of the dot.
    /// </summary>
    [Parameter]
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets the slide show index.
    /// </summary>
    [Parameter]
    public int CurrentSlideshowIndex { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the dot.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; }

    /// <summary>
    /// Gets the parent component.
    /// </summary>
    [CascadingParameter]
    private FluentCxSlideshow<TItem> Parent { get; set; } = default!;

    /// <summary>
    /// Gets the css of the indicator.
    /// </summary>
    private string? Css => new CssBuilder(Class)
        .AddClass("dot-indicator")
        .AddClass("dot-indicator-active", CurrentSlideshowIndex == Index + 1)
        .AddClass("dot-indicator-vertical", Orientation == Orientation.Vertical)
        .Build();

    /// <summary>
    /// Occurs when the dot is clicked.
    /// </summary>
    /// <returns>Returns the task which moves to the dot index when completed.</returns>
    private async Task MoveToIndexAsync()
    {
        if (Parent is not null)
        {
            await Parent.MoveToIndexAsync(Index);
        }
    }
}

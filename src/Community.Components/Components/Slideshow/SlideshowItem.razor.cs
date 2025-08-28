using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an item for the <see cref="FluentCxSlideshow{TItem}"/>.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class SlideshowItem<TItem>
    : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SlideshowItem{TItem}"/> class.
    /// </summary>
    public SlideshowItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the child content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the aria label of the component.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the parent of the component.
    /// </summary>
    [CascadingParameter]
    private FluentCxSlideshow<TItem> Parent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the interval to show the image.
    /// </summary>
    [Parameter]
    public TimeSpan? Interval { get; set; }

    /// <summary>
    /// Gets the css of the component.
    /// </summary>
    private string? Css => new CssBuilder(Class)
        .AddClass("slideshow-item")
        .AddClass("slideshow-item-vertical", Parent?.InternalOrientation == Orientation.Vertical)
        .Build();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.Add(this);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.Remove(this);

        GC.SuppressFinalize(this);
    }
}

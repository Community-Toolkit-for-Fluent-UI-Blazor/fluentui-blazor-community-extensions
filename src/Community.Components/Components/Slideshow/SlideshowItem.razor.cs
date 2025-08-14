using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

public partial class SlideshowItem<TItem>
    : FluentComponentBase, IDisposable
{
    public SlideshowItem()
    {
        Id = Identifier.NewId();
    }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [CascadingParameter]
    public FluentCxSlideshow<TItem> Parent { get; set; } = default!;

    private string? Css => new CssBuilder(Class)
        .AddClass("slideshow-item")
        .Build();

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("aria-hidden", Parent.GetAriaHiddenValue(this), Parent.Contains(this))
        .Build();

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.Remove(this);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.Add(this);
    }
}

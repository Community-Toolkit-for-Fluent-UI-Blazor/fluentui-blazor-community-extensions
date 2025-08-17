using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

public partial class DotIndicator<TItem>
    : FluentComponentBase
{
    private string AriaLabel => $"Item {Index}";

    [Parameter]
    public int Index { get; set; }

    [Parameter]
    public int CurrentSlideshowIndex { get; set; }

    [Parameter]
    public Orientation Orientation { get; set; }

    [CascadingParameter]
    private FluentCxSlideshow<TItem> Parent { get; set; } = default!;

    internal string? Css => new CssBuilder(Class)
        .AddClass("dot-indicator")
        .AddClass("dot-indicator-active", CurrentSlideshowIndex == Index + 1)
        .AddClass("dot-indicator-vertical", Orientation == Orientation.Vertical)
        .Build();

    private async Task MoveToIndexAsync()
    {
        if (Parent is not null)
        {
            await Parent.MoveToIndexAsync(Index);
        }
    }
}

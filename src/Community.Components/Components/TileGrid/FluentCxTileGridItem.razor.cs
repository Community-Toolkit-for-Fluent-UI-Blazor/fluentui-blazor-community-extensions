using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxTileGridItem<TItem>
    : FluentComponentBase, IItemValue<TItem>, ITileGridItemDropZoneComponent<TItem>, IDisposable
{
    [Inject]
    public required DropZoneState<TItem> DropZoneState { get; set; }

    [Parameter]
    public TItem Value { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public RenderFragment? Component { get; private set; }

    [Parameter]
    public int RowSpan { get; set; } = 1;

    [Parameter]
    public int ColumnSpan { get; set; } = 1;

    [Parameter]
    public EventCallback<MouseEventArgs> OnTapped { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleTapped { get; set; }

    [CascadingParameter]
    private FluentCxTileGrid<TItem>? Parent { get; set; }

    public void Dispose()
    {
        Parent?._dropContainer?.Remove(this);

        GC.SuppressFinalize(this);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?._dropContainer?.Add(this);
    }
}

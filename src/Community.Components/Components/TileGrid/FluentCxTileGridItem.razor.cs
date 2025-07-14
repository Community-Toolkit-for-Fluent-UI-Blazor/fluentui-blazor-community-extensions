using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a Tile Grid item.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
public partial class FluentCxTileGridItem<TItem>
    : FluentComponentBase, IItemValue<TItem>, ITileGridItemDropZoneComponent<TItem>, IDisposable
{
    /// <summary>
    /// Gets or sets the value of the item.
    /// </summary>
    [Parameter]
    public TItem Value { get; set; } = default!;

    /// <summary>
    /// Gets or sets the content of the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the render component. (when used inside other components) 
    /// </summary>
    public RenderFragment? Component { get; private set; }

    /// <summary>
    /// Gets or sets the row span of the component.
    /// </summary>
    [Parameter]
    public int RowSpan { get; set; } = 1;

    /// <summary>
    /// Gets or sets the column span of the component.
    /// </summary>
    [Parameter]
    public int ColumnSpan { get; set; } = 1;

    /// <summary>
    /// Event callback occurs when the component is tapped.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnTapped { get; set; }

    /// <summary>
    /// Event callback occurs when the component is double tapped.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleTapped { get; set; }

    /// <summary>
    /// Gets or sets the parent of the component.
    /// </summary>
    [CascadingParameter]
    private FluentCxTileGrid<TItem>? Parent { get; set; }

    /// <summary>
    /// Occurs when the tile is resized in an asynchronous way.
    /// </summary>
    /// <param name="e">Event args which contains the new size of the tile.</param>
    /// <returns>Returns a task which resizes the tile when completed.</returns>
    private async Task OnResizedAsync(ResizedEventArgs e)
    {
        ColumnSpan = e.ColumnSpan;
        RowSpan = e.RowSpan;

        if (Parent is not null)
        {
            await Parent.UpdateLayoutAsync(this);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?._dropContainer?.Remove(this);
        Parent?.RemoveLayoutItem(Parent?._dropContainer?.IndexOf(Value));
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?._dropContainer?.Add(this);
        Parent?.AddLayoutItem(this, Parent?._dropContainer?.IndexOf(Value));
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if (Parent is not null)
        {
            await Parent.UpdateLayoutAsync(this);
        }
    }

    /// <inheritdoc />
    public void SetSpan(int columnSpan, int rowSpan)
    {
        ColumnSpan = columnSpan;
        RowSpan = rowSpan;
        StateHasChanged();
    }
}

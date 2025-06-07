// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

[CascadingTypeParameter(nameof(TItem))]
public partial class FluentCxDropZoneContainer<TItem>
    : FluentComponentBase
{
    #region Fields

    private readonly List<FluentComponentBase> _children = [];
    private readonly RenderFragment<TItem> _renderItem;

    #endregion Fields

    #region Properties

    [Inject]
    public required DropZoneState<TItem> State { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool Virtualize { get; set; }

    [Parameter]
    public float ItemSize { get; set; } = 50f;

    [Parameter]
    public int? MaxItems { get; set; }

    private string? InternalCss => new CssBuilder()
        .AddClass(Class)
        .AddClass("fluentcx-drop-zone")
        .Build();

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle(GridSettings?.ToString())
        .Build();

    [Parameter]
    public Func<TItem?, TItem?, bool>? IsDropAllowed { get; set; }

    [Parameter]
    public RenderFragment<TItem>? ItemContent { get; set; }

    [Parameter]
    public Func<TItem, bool>? IsDragAllowed { get; set; }

    [Parameter]
    public bool Immediate { get; set; }

    [Parameter]
    public Func<TItem, TItem>? CloneItem { get; set; }

    [Parameter]
    public Func<TItem, string>? ItemCss { get; set; }

    [Parameter]
    public bool IsDragEnabled { get; set; } = true;

    [Parameter]
    public IList<TItem> Items { get; set; } = [];

    [Parameter]
    public EventCallback<TItem> Overflow { get; set; }

    [Parameter]
    public EventCallback<TItem> DragEnd { get; set; }

    [Parameter]
    public EventCallback<TItem> OnItemDropRejected { get; set; }

    [Parameter]
    public EventCallback<TItem> OnItemDrop { get; set; }

    [Parameter]
    public EventCallback<TItem> OnReplacedItemDrop { get; set; }

    [Parameter]
    public IGridSettings? GridSettings { get; set; }

    #endregion Properties

    #region Methods

    private async Task OnDropAsync(DragEventArgs e)
    {
        if (!await IsDropAllowedAsync())
        {
            State.Reset();
            await InvokeAsync(StateHasChanged);
            return;
        }

        var activeItem = State.ActiveItem;

        if (activeItem is null)
        {
            return;
        }

        if (State.TargetItem is null)
        {
            if (!Items.Contains(activeItem))
            {
                if (CloneItem is null)
                {
                    Items.Insert(Items.Count, activeItem);
                    State.Items?.Remove(activeItem);
                }
                else
                {
                    Items.Insert(Items.Count, CloneItem(activeItem));
                }
            }
        }
        else
        {
            if (!Immediate)
            {
                if (!Items.Contains(activeItem) &&
                    CloneItem is not null)
                {
                    await SwapAsync(State.TargetItem, CloneItem.Invoke(activeItem) ?? activeItem);
                }
                else
                {
                    await SwapAsync(State.TargetItem, activeItem);
                }
            }
        }

        State.Reset();
        await OnItemDrop.InvokeAsync(activeItem);
        await InvokeAsync(StateHasChanged);
    }

    private bool IsItemDraggable(TItem? item)
    {
        if (!IsDragEnabled)
        {
            return false;
        }

        if (IsDragAllowed is null)
        {
            return true;
        }

        if (item is null)
        {
            return false;
        }

        return IsDragAllowed(item);
    }

    internal bool IsOverflow()
    {
        var activeItem = State.ActiveItem;

        if (activeItem is null)
        {
            return false;
        }

        return (!Items.Contains(activeItem) && MaxItems.HasValue && MaxItems == Items.Count);
    }

    internal bool IsItemDropAllowed(TItem? item)
    {
        if (IsDropAllowed is null)
        {
            return true;
        }

        return IsDropAllowed(State.ActiveItem, item);
    }

    internal int IndexOf(TItem? item)
    {
        if (item is null)
        {
            return -1;
        }

        if (Items is null)
        {
            return -1;
        }

        return Items.IndexOf(item);
    }

    internal void RemoveAt(int index)
    {
        Items?.RemoveAt(index);
    }

    internal void Insert(int index, TItem item)
    {
        Items?.Insert(index, item);
    }

    internal async Task<bool> IsDropAllowedAsync()
    {
        var activeItem = State.ActiveItem;

        if (activeItem is null)
        {
            return false;
        }

        if (IsOverflow())
        {
            if (Overflow.HasDelegate)
            {
                await Overflow.InvokeAsync(activeItem);
            }

            return false;
        }

        if (!IsItemDropAllowed(State.TargetItem))
        {
            if (OnItemDropRejected.HasDelegate)
            {
                await OnItemDropRejected.InvokeAsync(activeItem);
            }

            return false;
        }

        return true;
    }

    internal async Task OnItemDropAsync(TItem? item)
    {
        if (OnItemDrop.HasDelegate &&
            item is not null)
        {
            await OnItemDrop.InvokeAsync(item);
        }

        await InvokeAsync(StateHasChanged);
    }

    internal async Task OnDragEndAsync()
    {
        if (DragEnd.HasDelegate)
        {
            await DragEnd.InvokeAsync(State.ActiveItem);
        }

        State.Reset();
        await InvokeAsync(StateHasChanged);
    }

    internal void UpdateItems()
    {
        State.Items = Items;
    }

    internal void Add(FluentComponentBase child)
    {
        if (child is not IItemValue<TItem> t)
        {
            throw new InvalidOperationException("The child component must implement IItemValue<TItem>");
        }

        if (!_children.Contains(child, ChildComponentValueEqualityComparer<TItem>.Default))
        {
            _children.Add(child);
            Items.Add(t.Value);
            StateHasChanged();
        }
    }

    internal void Remove(FluentComponentBase child)
    {
        if (child is not IItemValue<TItem> t)
        {
            throw new InvalidOperationException("The child component must implement IItemValue<TItem>");
        }

        _children.Remove(child);
        Items.Remove(t.Value);
        StateHasChanged();
    }

    internal async Task SwapAsync(TItem? overItem, TItem? activeItem)
    {
        if (overItem is null || activeItem is null)
        {
            return;
        }

        var indexDraggedOverItem = Items.IndexOf(overItem);
        var indexActiveItem = Items.IndexOf(activeItem);

        if (indexActiveItem == -1)
        {
            Items.Insert(indexDraggedOverItem + 1, activeItem);
            State.Items?.Remove(activeItem);
        }
        else if (Immediate)
        {
            if (indexDraggedOverItem == indexActiveItem)
            {
                return;
            }

            (Items[indexActiveItem], Items[indexDraggedOverItem]) = (Items[indexDraggedOverItem], Items[indexActiveItem]);

            if (OnReplacedItemDrop.HasDelegate)
            {
                await OnReplacedItemDrop.InvokeAsync(Items[indexActiveItem]);
            }
        }
        else
        {
            if (indexDraggedOverItem == indexActiveItem)
            {
                return;
            }

            var tmp = Items[indexActiveItem];
            Items.RemoveAt(indexActiveItem);
            Items.Insert(indexDraggedOverItem, tmp);
        }

        // If ChildContent is used, we need to reorder the children list.
        if (ChildContent is not null)
        {
            (_children[indexDraggedOverItem], _children[indexActiveItem]) = (_children[indexActiveItem], _children[indexDraggedOverItem]);
        }
    }

    internal void Refresh()
    {
        if (ChildContent is not null)
        {
            for (var i = 0; i < Items.Count; ++i)
            {
                var index = _children.FindIndex(x => x is IItemValue<TItem> t && Equals(t.Value, Items[i]));
                var temp = _children[index];
                _children.RemoveAt(index);
                _children.Insert(i, temp);
            }
        }

        StateHasChanged();
    }

    internal async Task OnDropItemPlaceholderAsync(int index)
    {
        if (!await IsDropAllowedAsync())
        {
            State.Reset();
            await InvokeAsync(Refresh);
            return;
        }

        if (State.ActiveItem is null)
        {
            return;
        }

        var activeItem = State.ActiveItem;
        var oldIndex = IndexOf(activeItem);
        var isInSameDropZone = false;

        if (oldIndex == -1)
        {
            if (CloneItem is null)
            {
                State.RemoveActiveItem();
            }
        }
        else
        {
            isInSameDropZone = true;
            RemoveAt(oldIndex);

            if (index > oldIndex)
            {
                index--;
            }
        }

        if (CloneItem is null)
        {
            Insert(index, activeItem);
        }
        else
        {
            Insert(index, isInSameDropZone ? activeItem : CloneItem(activeItem));
        }

        State.Reset();
        await InvokeAsync(Refresh);
        await OnItemDropAsync(activeItem);
    }

    internal string? GetStyle(TItem? value)
    {
        if (_children.Count == 0 || value is null)
        {
            return null;
        }

        var child = _children.Find(x => x is IItemValue<TItem> t && EqualityComparer<TItem>.Default.Equals(t.Value, value));

        if (child is ITileGridItemDropZoneComponent<TItem> c)
        {
            return new StyleBuilder(Style)
               .AddStyle("grid-column-end", $"span {c.ColumnSpan}")
               .AddStyle("grid-row-end", $"span {c.RowSpan}")
               .AddStyle("padding", "calc(var(--design-unit) * 3px)")
               .Build();
        }

        return null;
    }

    #endregion Methods
}

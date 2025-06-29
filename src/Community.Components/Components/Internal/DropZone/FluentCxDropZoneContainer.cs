using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace FluentUI.Blazor.Community.Components.Internal;

[CascadingTypeParameter(nameof(TItem))]
internal sealed class FluentCxDropZoneContainer<TItem>
    : FluentComponentBase
{
    #region Fields

    private readonly List<FluentComponentBase> _children = [];
    private readonly RenderFragment<TItem> _renderItem;

    #endregion Fields

    #region Constructors

    public FluentCxDropZoneContainer()
    {
        _renderItem = value => __builder =>
        {
            __builder.OpenComponent<FluentCxDropZone<TItem>>(32);
            __builder.AddComponentParameter(33, nameof(FluentCxDropZone<TItem>.ItemCss), RuntimeHelpers.TypeCheck(ItemCss?.Invoke(value)));
            __builder.AddComponentParameter(34, nameof(FluentCxDropZone<TItem>.ForceRender), RuntimeHelpers.TypeCheck(ChildContent is not null));
            __builder.AddComponentParameter(35, nameof(FluentCxDropZone<TItem>.AddInContainer), RuntimeHelpers.TypeCheck(false));
            __builder.AddComponentParameter(36, nameof(FluentCxDropZone<TItem>.Id), RuntimeHelpers.TypeCheck(Identifier.NewId()));
            __builder.AddComponentParameter(37, nameof(FluentCxDropZone<TItem>.IsDragAllowed), RuntimeHelpers.TypeCheck(IsItemDraggable(value)));
            __builder.AddComponentParameter(38, nameof(FluentCxDropZone<TItem>.IsItemDropAllowed), RuntimeHelpers.TypeCheck((IsDropAllowed?.Invoke(value, State!.ActiveItem)) ?? true));
            __builder.AddComponentParameter(39, nameof(FluentCxDropZone<TItem>.Value), RuntimeHelpers.TypeCheck(value));
            __builder.AddComponentParameter(40, nameof(FluentCxDropZone<TItem>.Style), RuntimeHelpers.TypeCheck(GetStyle(value)));
            __builder.AddAttribute(41, "ChildContent", (RenderFragment)((__builder2) =>
            {
                if (ItemContent is not null)
                {
                    __builder2.AddContent(42, ItemContent(value));
                }
                else
                {
                    var content = _children.Find(x => x is IItemValue<TItem> t && Equals(t.Value, value));

                    if (content is not null && content is IDropZoneComponent<TItem> t)
                    {
                        __builder2.AddContent(43, t.Component);
                    }
                }
            }
            ));

            __builder.CloseComponent();
        };
    }

    #endregion Constructors

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
        .AddStyle(TileGridSettings?.ToString())
        .AddStyle("overflow-y", "auto", CanOverflow)
        .Build();

    [Parameter]
    public bool CanOverflow { get; set; }

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
    public ITileGridSettings? TileGridSettings { get; set; }

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

        if (t.Value is null)
        {
            throw new InvalidOperationException("The Value parameter must have a value.");
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

        if (t.Value is null)
        {
            throw new InvalidOperationException("The Value parameter must have a value.");
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
               .AddStyle("display", "grid")
               .Build();
        }

        return null;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.OpenComponent<CascadingValue<FluentCxDropZoneContainer<TItem>>>(0);
        builder.AddComponentParameter(1, nameof(CascadingValue<FluentCxDropZoneContainer<TItem>>.Value), this);
        builder.AddComponentParameter(2, nameof(CascadingValue<FluentCxDropZoneContainer<TItem>>.IsFixed), true);
        builder.AddChildContent(3, (builder2) =>
        {
            builder2.OpenElement(4, "div");
            builder2.AddAttribute(5, "id", GetId());
            builder2.AddAttribute(6, "class", InternalCss);
            builder2.AddAttribute(7, "style", InternalStyle);
            builder2.AddAttribute(8, "role", "list");
            builder2.AddEventPreventDefaultAttribute(9, "ondragover", true);
            builder2.AddAttribute(10, "ondragover", EventCallback.Factory.Create<DragEventArgs>(this, () => { }));
            builder2.AddEventPreventDefaultAttribute(11, "ondragenter", true);
            builder2.AddAttribute(12, "ondragenter", EventCallback.Factory.Create<DragEventArgs>(this, () => { }));
            builder2.AddAttribute(13, "ondrop", EventCallback.Factory.Create<DragEventArgs>(this, OnDropAsync));
            builder2.AddEventPreventDefaultAttribute(14, "ondrop", true);
            builder2.AddAttribute(15, "ondragstart", "event.dataTransfer.setData(\'text\', event.target.id);");
            builder2.AddEventStopPropagationAttribute(16, "ondrop", true);
            builder2.AddEventStopPropagationAttribute(17, "ondragenter", true);
            builder2.AddEventStopPropagationAttribute(18, "ondragend", true);
            builder2.AddEventStopPropagationAttribute(19, "ondragover", true);
            builder2.AddEventStopPropagationAttribute(20, "ondragleave", true);
            builder2.AddEventStopPropagationAttribute(21, "ondragstart", true);
            builder2.AddMultipleAttributes(22, RuntimeHelpers.TypeCheck<IEnumerable<KeyValuePair<string, object>>>(AdditionalAttributes!));

            if (ChildContent is not null)
            {
                builder2.AddContent(24, ChildContent);

                if (_children.All(x => x is FluentCxDropZone<TItem>))
                {
                    foreach (var child in _children.OfType<FluentCxDropZone<TItem>>())
                    {
                        child.RenderInternal();
                    }
                }
                else
                {
                    foreach (var item in _children.OfType<IDropZoneComponent<TItem>>())
                    {
                        if (item.Value is not null)
                        {
                            builder2.AddContent(25, _renderItem(item.Value));
                        }
                    }
                }
            }
            else if (Items is not null &&
                    Items.Count > 0)
            {
                if (Virtualize)
                {
                    builder2.OpenComponent<Virtualize<TItem>>(26);
                    builder2.AddComponentParameter(27, nameof(Virtualize<TItem>.Items), RuntimeHelpers.TypeCheck(Items));
                    builder2.AddComponentParameter(28, nameof(Virtualize<TItem>.ItemSize), RuntimeHelpers.TypeCheck(ItemSize));
                    builder2.AddAttribute(29, "ChildContent", (RenderFragment<TItem>)((context) => (__builder3) =>
                    {
                        __builder3.AddContent(30, _renderItem(context));
                    }
                    ));
                    builder2.CloseComponent();
                }
                else
                {
                    foreach (var item in Items)
                    {
                        builder2.AddContent(31, _renderItem(item));
                    }
                }
            }

            builder2.CloseElement();
        });

        builder.CloseComponent();
    }

    #endregion Methods
}

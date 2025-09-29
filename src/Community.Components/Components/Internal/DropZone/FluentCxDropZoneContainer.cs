using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.CompilerServices;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Internal;

/// <summary>
/// Represents a drop zone container.
/// </summary>
/// <typeparam name="TItem">Type of the item.</typeparam>
[CascadingTypeParameter(nameof(TItem))]
internal sealed class FluentCxDropZoneContainer<TItem>
    : FluentComponentBase
{
    #region Fields

    /// <summary>
    /// Represents the children inside the component.
    /// </summary>
    private readonly List<FluentComponentBase> _children = [];

    /// <summary>
    /// Represents the fragment to render an item.
    /// </summary>
    private readonly RenderFragment<TItem> _renderItem;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxDropZoneContainer{TItem}"/>.
    /// </summary>
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
                    var content = _children.Find(x => x is IItemValue<TItem> t &&
                                                      CheckEquality(t.Value, value));

                    if (content is not null && content is IDropZoneComponent<TItem> t)
                    {
                        if (t is ITileGridItemDropZoneComponent<TItem> k)
                        {
                            var item = Layout?.Items.FirstOrDefault(x => x.Key == ItemKey!(t.Value!));

                            if (item is TileGridLayoutItem layoutItem)
                            {
                                k.SetSpan(layoutItem.ColumnSpan, layoutItem.RowSpan);
                            }
                        }

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

    /// <summary>
    /// Gets or sets the state of the component.
    /// </summary>
    [Inject]
    public required DropZoneState<TItem> State { get; set; }

    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets if the component is virtualized.
    /// </summary>
    [Parameter]
    public bool Virtualize { get; set; }

    /// <summary>
    /// Gets or sets the size of the item.
    /// </summary>
    [Parameter]
    public float ItemSize { get; set; } = 50f;

    /// <summary>
    /// Gets or sets the maximum items which can be dragged.
    /// </summary>
    [Parameter]
    public int? MaxItems { get; set; }

    /// <summary>
    /// Gets or sets the function to extract a key from an item.
    /// </summary>
    [Parameter]
    public Func<TItem, string>? ItemKey { get; set; }

    /// <summary>
    /// Gets or sets the layout of the grid.
    /// </summary>
    [Parameter]
    public GridLayoutBase? Layout { get; set; }

    /// <summary>
    /// Gets the css of the component.
    /// </summary>
    private string? InternalCss => new CssBuilder()
        .AddClass(Class)
        .AddClass("fluentcx-drop-zone")
        .Build();

    /// <summary>
    /// Gets the style of the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle(TileGridSettings?.ToString())
        .AddStyle("overflow-y", "auto", CanOverflow)
        .Build();

    /// <summary>
    /// Gets or sets a value indicating if the component can overflow.
    /// </summary>
    [Parameter]
    public bool CanOverflow { get; set; }

    /// <summary>
    /// Gets or sets a function which indicates if a component can be dropped.
    /// </summary>
    [Parameter]
    public Func<TItem?, TItem?, bool>? IsDropAllowed { get; set; }

    /// <summary>
    /// Gets or sets the content of an item.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? ItemContent { get; set; }

    /// <summary>
    /// Gets or sets a function which indicates if the component can be dragged.
    /// </summary>
    [Parameter]
    public Func<TItem, bool>? IsDragAllowed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the drag and drop is immediate.
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; }

    /// <summary>
    /// Gets or sets a function to clone an item.
    /// </summary>
    [Parameter]
    public Func<TItem, TItem>? CloneItem { get; set; }

    /// <summary>
    /// Gets or sets a function to set the css of an item.
    /// </summary>
    [Parameter]
    public Func<TItem, string>? ItemCss { get; set; }

    /// <summary>
    /// Gets or sets if the drag is enabled.
    /// </summary>
    [Parameter]
    public bool IsDragEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the list of the items.
    /// </summary>
    [Parameter]
    public IList<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the event callback to raise when the component overflows.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> Overflow { get; set; }

    /// <summary>
    /// Gets or sets the event callback to raise when the drag ends.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> DragEnd { get; set; }

    /// <summary>
    /// Gets or sets the event callback to raise when the drop is rejected for an item.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnItemDropRejected { get; set; }

    /// <summary>
    /// Gets or sets the event callback to raise when an item is dropped.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnItemDrop { get; set; }

    /// <summary>
    /// Gets or sets the event callback to raise when an item is replaced after drop.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> OnReplacedItemDrop { get; set; }

    /// <summary>
    /// Gets or sets the settings of the tile grid.
    /// </summary>
    [Parameter]
    public ITileGridSettings? TileGridSettings { get; set; }

    /// <summary>
    /// Gets or sets if the layout is persisted.
    /// </summary>
    [Parameter]
    public bool PersistenceEnabled { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Checks equality between two items.
    /// </summary>
    /// <param name="left">Left item to compare.</param>
    /// <param name="right">Right item to compare.</param>
    /// <returns>Returns <see langword="true" /> if the items are equal, <see langword="false" /> otherwise.</returns>
    private bool CheckEquality(TItem? left, TItem? right)
    {
        if (ItemKey is null)
        {
            return Equals(left, right);
        }

        if (left is null)
        {
            return right is null;
        }

        if (right is null)
        {
            return left is null;
        }

        return string.Equals(ItemKey(left), ItemKey(right));
    }

    /// <summary>
    /// Occurs when an item is dropped into the component.
    /// </summary>
    /// <param name="e">Event args associated to the drop.</param>
    /// <returns>Returns a task which manages the drop when completed.</returns>
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

        if (PersistenceEnabled)
        {
            Layout?.Update(ItemKey, Items);
            Layout?.RequestSave();
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Gets a value indicating if an item is draggable or not.
    /// </summary>
    /// <param name="item">Item to check.</param>
    /// <returns>Returns <see langword="true" /> if the item is draggable, <see langword="false" /> otherwise.</returns>
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

    /// <summary>
    /// Gets if the component overflows.
    /// </summary>
    /// <returns>Returns <see langword="true" /> if the component overflows, <see langword="false" /> otherwise.</returns>
    internal bool IsOverflow()
    {
        var activeItem = State.ActiveItem;

        if (activeItem is null)
        {
            return false;
        }

        return (!Items.Contains(activeItem) && MaxItems.HasValue && MaxItems == Items.Count);
    }

    /// <summary>
    /// Gets a value indicating if the item is allowed to drop.
    /// </summary>
    /// <param name="item">Item to check.</param>
    /// <returns>Returns <see langword="true" /> if the item is allowed to drop, <see langword="false" /> otherwise.</returns>
    internal bool IsItemDropAllowed(TItem? item)
    {
        if (IsDropAllowed is null)
        {
            return true;
        }

        return IsDropAllowed(State.ActiveItem, item);
    }

    /// <summary>
    /// Gets the index of the item.
    /// </summary>
    /// <param name="item">Item to get the index.</param>
    /// <returns>Returns the index of the item.</returns>
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

    /// <summary>
    /// Remove the item at the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index">Index of the item.</param>
    internal void RemoveAt(int index)
    {
        Items?.RemoveAt(index);
    }

    /// <summary>
    /// Inserts the item at the specified index.
    /// </summary>
    /// <param name="index">Index of the item.</param>
    /// <param name="item">Item to insert.</param>
    internal void Insert(int index, TItem item)
    {
        Items?.Insert(index, item);
    }

    /// <summary>
    /// Gets a value indicating if the drop is allowed.
    /// </summary>
    /// <returns>Returns a task which contains <see langword="true"/> if the drop is allowed, <see langword="false" /> otherwise
    ///  when completed.</returns>
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

    /// <summary>
    /// Drops an item in an asynchronous way.
    /// </summary>
    /// <param name="item">Item to drop.</param>
    /// <returns>Returns a task which drops an item when completed.</returns>
    internal async Task OnItemDropAsync(TItem? item)
    {
        if (OnItemDrop.HasDelegate &&
            item is not null)
        {
            await OnItemDrop.InvokeAsync(item);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Finish the drag.
    /// </summary>
    /// <returns>Returns a task which completes the drag when completed.</returns>
    internal async Task OnDragEndAsync()
    {
        if (DragEnd.HasDelegate)
        {
            await DragEnd.InvokeAsync(State.ActiveItem);
        }

        State.Reset();
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Updates the items.
    /// </summary>
    internal void UpdateItems()
    {
        State.Items = Items;
    }

    /// <summary>
    /// Adds a child into the container.
    /// </summary>
    /// <param name="child">Child to add.</param>
    /// <exception cref="InvalidOperationException">Occurs when the child is not <see cref="IItemValue{TItem}"/> and
    ///  when the value of the child is <see langword="null" /></exception>
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

    /// <summary>
    /// Removes a child from the container.
    /// </summary>
    /// <param name="child">Child to remove.</param>
    /// <exception cref="InvalidOperationException">Occurs when the child is not <see cref="IItemValue{TItem}"/> and
    ///  when the value of the child is <see langword="null" /></exception>
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

    /// <summary>
    /// Swap the <paramref name="overItem"/> with the <paramref name="activeItem"/>.
    /// </summary>
    /// <param name="overItem">First item to swap.</param>
    /// <param name="activeItem">Second item to swap.</param>
    /// <returns>Returns a task which swap the items when completed.</returns>
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

    /// <summary>
    /// Refresh the component.
    /// </summary>
    internal void Refresh()
    {
        if (ChildContent is not null)
        {
            ReorderChildrenFromLayout();
        }

        StateHasChanged();
    }

    /// <summary>
    /// Drops the item inside a placeholder in an asynchronous way.
    /// </summary>
    /// <param name="index">Index of the placeholder.</param>
    /// <returns>Returns a task which drops the item in the placeholder when completed.</returns>
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

    /// <summary>
    /// Gets the style of the <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Value to get the style.</param>
    /// <returns>Returns the style of the item.</returns>
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

    /// <summary>
    /// Reorders the items using a layout.
    /// </summary>
    /// <returns>Returns the ordered items.</returns>
    private IEnumerable<TItem> ReorderItemsFromLayout()
    {
        if (!PersistenceEnabled || Layout is null || !Layout.Items.Any() || Layout.IsDirty || ItemKey is null)
        {
            return Items;
        }

        SortedList<int, TItem> ordered = [];

        foreach (var layoutItem in Layout)
        {
            var item = Items.FirstOrDefault(x => string.Equals(ItemKey(x), layoutItem.Key, StringComparison.OrdinalIgnoreCase));

            if (item is not null)
            {
                ordered.Add(layoutItem.Index, item);
            }
        }

        return ordered.Values;
    }

    /// <summary>
    /// Reorders the children using a layout.
    /// </summary>
    /// <returns>Returns the ordered components.</returns>
    private List<FluentComponentBase> ReorderChildrenFromLayout()
    {
        if (!PersistenceEnabled ||
            Layout is null ||
            !Layout.Items.Any() ||
            Layout.IsDirty ||
            ItemKey is null)
        {
            return [.. _children];
        }

        SortedList<int, FluentComponentBase> reorderChildren = [];
        var sortedItems = new SortedList<int, TItem>();

        foreach (var layoutItem in Layout)
        {
            var index = _children.FindIndex(x => x is IItemValue<TItem> t &&
                                           t.Value is not null &&
                                           string.Equals(ItemKey(t.Value), layoutItem.Key, StringComparison.OrdinalIgnoreCase));

            if (index == -1)
            {
                continue;
            }

            var item = _children[index];

            reorderChildren.Add(layoutItem.Index, item);

            if (item is FluentCxTileGridItem<TItem> tg &&
                layoutItem is TileGridLayoutItem tgli)
            {
                tg.SetSpan(tgli.ColumnSpan, tgli.RowSpan);
            }

            var orderedItem = Items.FirstOrDefault(x => string.Equals(ItemKey(x), layoutItem.Key, StringComparison.OrdinalIgnoreCase));

            if (orderedItem is not null)
            {
                sortedItems.Add(layoutItem.Index, orderedItem);
            }
        }

        Items.Clear();

        foreach (var item in sortedItems)
        {
            Items.Add(item.Value);
        }

        return [.. reorderChildren.Values];
    }

    /// <inheritdoc />
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

                var reorderedChildren = ReorderChildrenFromLayout();
                _children.Clear();
                _children.AddRange(reorderedChildren);

                foreach(var child in reorderedChildren)
                {
                    if (child is FluentCxDropZone<TItem> c)
                    {
                        c.RenderInternal();
                    }
                    else if (child is IDropZoneComponent<TItem> dzc && dzc.Value is not null)
                    {
                        builder2.AddContent(25, _renderItem(dzc.Value));
                    }
                }
            }
            else if (Items is not null &&
                    Items.Count > 0)
            {
                var items = ReorderItemsFromLayout();

                if (Virtualize)
                {
                    builder2.OpenComponent<Virtualize<TItem>>(26);
                    builder2.AddComponentParameter(27, nameof(Virtualize<TItem>.Items), RuntimeHelpers.TypeCheck(items));
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
                    foreach (var item in items)
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

using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

internal class FluentCxDropZone<TItem>
    : FluentComponentBase, IDisposable, IItemValue<TItem>
{
    internal readonly RenderFragment _renderDropZone;

    public FluentCxDropZone()
    {
        Id = Identifier.NewId();

        _renderDropZone = __builder =>
        {
            if (DropZoneContainer.TileGridSettings is null)
            {
                __builder.OpenComponent<FluentStack>(5);
                __builder.AddComponentParameter(6, nameof(FluentStack.Width), "100%");
                __builder.AddComponentParameter(7, nameof(FluentStack.Style), Style);
                __builder.AddComponentParameter(8, nameof(FluentStack.Id), Id);
                __builder.AddComponentParameter(9, nameof(FluentStack.HorizontalAlignment), HorizontalAlignment.Stretch);
                __builder.AddComponentParameter(10, nameof(FluentStack.VerticalAlignment), VerticalAlignment.Stretch);
                __builder.AddComponentParameter(11, nameof(FluentStack.VerticalGap), 0);
                __builder.AddComponentParameter(12, nameof(FluentStack.Orientation), Orientation.Vertical);
                __builder.AddAttribute(13, "ChildContent", (RenderFragment)((__builder2) =>
                {
                    if (Index == 0 && !DropZoneContainer.MaxItems.HasValue || DropZoneContainer.MaxItems > 1)
                    {
                        __builder2.OpenElement(14, "div");
                        __builder2.AddAttribute(15, "ondrop", EventCallback.Factory.Create<DragEventArgs>(this, () => OnDropItemOnPlaceholderAsync(0)));
                        __builder2.AddEventStopPropagationAttribute(16, "ondrop", true);
                        __builder2.AddAttribute(17, "ondragenter", EventCallback.Factory.Create<DragEventArgs>(this, () => State!.ActivePlaceholderId = 0));
                        __builder2.AddAttribute(18, "ondragleave", EventCallback.Factory.Create<DragEventArgs>(this, () => State!.ActivePlaceholderId = null));
                        __builder2.AddAttribute(19, "class", GetPlaceholderCss(0));
                        __builder2.CloseElement();
                    }


                    __builder2.OpenElement(20, "div");
                    __builder2.AddAttribute(21, "draggable", IsDragAllowed.ToString());
                    __builder2.AddAttribute(22, "ondragstart", EventCallback.Factory.Create<DragEventArgs>(this, OnDragStart));
                    __builder2.AddAttribute(23, "ondragend", EventCallback.Factory.Create<DragEventArgs>(this, OnDragEndAsync));
                    __builder2.AddAttribute(24, "role", "listitem");
                    __builder2.AddAttribute(25, "ondragenter", EventCallback.Factory.Create<DragEventArgs>(this, () => OnDragEnterAsync()));
                    __builder2.AddAttribute(26, "ondragleave", EventCallback.Factory.Create<DragEventArgs>(this, () => OnDragLeave()));
                    __builder2.AddAttribute(27, "class", GetItemCss());
                    __builder2.AddAttribute(28, "style", GetItemStyle());
                    __builder2.AddContent(29, ChildContent);
                    __builder2.CloseElement();

                    if (DropZoneContainer.MaxItems.HasValue || DropZoneContainer.MaxItems > 1)
                    {
                        var currentIndex = Index + 1;

                        __builder2.OpenElement(30, "div");
                        __builder2.AddAttribute(31, "ondrop", EventCallback.Factory.Create<DragEventArgs>(this, () => OnDropItemOnPlaceholderAsync(currentIndex)));
                        __builder2.AddEventStopPropagationAttribute(32, "ondrop", true);
                        __builder2.AddAttribute(33, "ondragenter", EventCallback.Factory.Create<DragEventArgs>(this, () => State!.ActivePlaceholderId = currentIndex));
                        __builder2.AddAttribute(34, "ondragleave", EventCallback.Factory.Create<DragEventArgs>(this, () => State!.ActivePlaceholderId = null));
                        __builder2.AddAttribute(37, "class", GetPlaceholderCss(currentIndex));
                        __builder2.CloseElement();
                    }
                }));

                __builder.CloseComponent();
            }
            else
            {
                __builder.OpenElement(5, "div");
                __builder.AddAttribute(6, "draggable", IsDragAllowed.ToString());
                __builder.AddAttribute(7, "ondragstart", EventCallback.Factory.Create<DragEventArgs>(this, OnDragStart));
                __builder.AddAttribute(8, "ondragend", EventCallback.Factory.Create<DragEventArgs>(this, OnDragEndAsync));
                __builder.AddAttribute(9, "role", "listitem");
                __builder.AddAttribute(10, "ondragenter", EventCallback.Factory.Create<DragEventArgs>(this, () => OnDragEnterAsync()));
                __builder.AddAttribute(11, "ondragleave", EventCallback.Factory.Create<DragEventArgs>(this, () => OnDragLeave()));
                __builder.AddAttribute(12, "class", GetItemCss());
                __builder.AddAttribute(13, "style", GetItemStyle());
                __builder.AddContent(14, ChildContent);
                __builder.CloseElement();
            }

        };
    }

    [CascadingParameter]
    private FluentCxDropZoneContainer<TItem> DropZoneContainer { get; set; } = default!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? ItemCss { get; set; }

    [Parameter]
    public bool IsDragAllowed { get; set; }

    [Parameter]
    public bool IsItemDropAllowed { get; set; }

    private DropZoneState<TItem> State => DropZoneContainer.State;

    [Parameter]
    public TItem? Value { get; set; }

    /// <summary>
    /// Gets or sets a value indicating wether the component will render. 
    /// </summary>
    /// <remarks>
    /// This value is used internally by <see cref="FluentCxDropZone{TItem}"/>.
    /// You mustn't use it.
    /// </remarks>
    [Parameter]
    public bool ForceRender { get; set; }

    /// <summary>
    /// Gets or sets a value indicating wether the component will be inserted into the container. 
    /// </summary>
    /// <remarks>
    /// This value is used internally by <see cref="FluentCxDropZone{TItem}"/>.
    /// You mustn't use it.
    /// </remarks>
    [Parameter]
    public bool AddInContainer { get; set; } = true;

    private int Index => DropZoneContainer?.IndexOf(Value) ?? -1;

    public RenderFragment? Component => _renderDropZone;

    private string? GetItemStyle()
    {
        var style = new StyleBuilder(Style)
            .AddStyle("cursor", "grab", IsDragAllowed && !Equals(State.ActiveItem))
            .AddStyle("cursor", "grabbing", IsDragAllowed && Equals(State.ActiveItem));

        return style.Build();
    }

    private string? GetItemCss()
    {
        var css = new CssBuilder()
                .AddClass("fluentcx-drop-zone-noselect", !IsDragAllowed)
                .AddClass("fluentcx-drop-zone-moving", Value?.Equals(State.ActiveItem) ?? false)
                .AddClass("no-pointer-events", Value?.Equals(State.ActiveItem) ?? false)
                .AddClass("fluentcx-drop-zone-draggable")
                .AddClass("fluentcx-drop-zone-progress", Value?.Equals(State.ActiveItem) ?? false)
                .AddClass("fluentcx-drop-zone-dragged-over", GetIsDragTarget())
                .AddClass("fluentcx-drop-zone-dragged-over-denied", GetIsDragTargetDenied());

        if (!string.IsNullOrEmpty(ItemCss))
        {
            css.AddClass(ItemCss);
        }

        return css.Build();
    }

    private bool GetIsDragTargetDenied()
    {
        if (Value?.Equals(State.ActiveItem) ?? false)
        {
            return false;
        }

        if (Value?.Equals(State.TargetItem) ?? false)
        {
            return !IsItemDropAllowed;
        }

        return false;
    }

    private bool GetIsDragTarget()
    {
        if (Value?.Equals(State.ActiveItem) ?? false)
        {
            return false;
        }

        if (Value?.Equals(State.TargetItem) ?? false)
        {
            return IsItemDropAllowed;
        }

        return false;
    }

    private string? GetPlaceholderCss(int index)
    {
        return new CssBuilder()
                .AddClass("fluentcx-drop-zone-placeholder")
                .AddClass("fluentcx-drop-zone-placeholder-drag-over", State.ActivePlaceholderId == index && DropZoneContainer.IndexOf(State.ActiveItem) == -1)
                .AddClass("fluentcx-drop-zone-placeholder-drag-over", State.ActivePlaceholderId == index && (index != DropZoneContainer.IndexOf(State.ActiveItem)) && (index != DropZoneContainer.IndexOf(State.ActiveItem) + 1))
                .AddClass("fluentcx-drop-zone-progress", State.ActiveItem is not null)
                .Build();
    }

    private void OnDragLeave()
    {
        State.TargetItem = default!;
        DropZoneContainer.Refresh();
        Console.WriteLine($"Leave {Value}");
    }

    private async Task OnDragEndAsync()
    {
        await DropZoneContainer.OnDragEndAsync();
        await InvokeAsync(DropZoneContainer.Refresh);
    }

    private async Task OnDropItemOnPlaceholderAsync(int index)
    {
        await DropZoneContainer.OnDropItemPlaceholderAsync(index);
    }

    private void OnDragStart()
    {
        State.ActiveItem = Value;
        DropZoneContainer?.UpdateItems();
        DropZoneContainer?.Refresh();
        StateHasChanged();
    }

    private async Task OnDragEnterAsync()
    {
        var activeItem = State.ActiveItem;

        if (activeItem is null)
        {
            return;
        }

        if (Value?.Equals(activeItem) ?? false)
        {
            return;
        }

        if (DropZoneContainer.IsOverflow())
        {
            return;
        }

        if (!DropZoneContainer.IsItemDropAllowed(Value))
        {
            return;
        }

        State.TargetItem = Value;
        Console.WriteLine($"Enter {Value}");

        if (DropZoneContainer.Immediate)
        {
            await DropZoneContainer.SwapAsync(State.TargetItem, activeItem);
        }

        await InvokeAsync(DropZoneContainer.Refresh);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (AddInContainer)
        {
            DropZoneContainer.Add(this);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (AddInContainer)
        {
            DropZoneContainer.Remove(this);
        }

        GC.SuppressFinalize(this);
    }

    internal void RenderInternal()
    {
        ForceRender = true;
        StateHasChanged();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.OpenComponent<CascadingValue<FluentCxDropZone<TItem>>>(0);
        builder.AddComponentParameter(1, nameof(CascadingValue<FluentCxDropZone<TItem>>.Value), this);
        builder.AddComponentParameter(2, nameof(CascadingValue<FluentCxDropZone<TItem>>.IsFixed), true);
        builder.AddChildContent(3, __builder2 =>
        {
            if (DropZoneContainer.ChildContent is null || ForceRender)
            {
                __builder2.AddContent(4, _renderDropZone);
            }
        });
        builder.CloseComponent();
    }
}

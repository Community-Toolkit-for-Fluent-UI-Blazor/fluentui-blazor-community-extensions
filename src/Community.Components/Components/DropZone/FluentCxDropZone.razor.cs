



using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxDropZone<TItem>
    : FluentComponentBase, IDisposable, IItemValue<TItem>
{
    internal readonly RenderFragment _renderDropZone;

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
}

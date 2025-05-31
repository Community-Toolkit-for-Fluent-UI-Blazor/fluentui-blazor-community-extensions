// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxTileGridItem<TItem>
    : FluentComponentBase where TItem : class, new()
{
    private bool _isRendered;
    private bool _hasParameterChanged;
    private readonly DotNetObjectReference<FluentCxTileGridItem<TItem>>? _dotNetReference;
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Blazor.Community.Components/Components/TileGrid/FluentCxTileGridItem.razor.js";
    private IJSObjectReference? _module;

    [CascadingParameter]
    private FluentCxTileGrid<TItem> Parent { get; set; } = default!;

    [Parameter]
    public bool IsVisible { get; set; } = true;

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleClick { get; set; }

    [Parameter]
    public EventCallback<TouchEventArgs> OnTouchEnter { get; set; }

    [Parameter]
    public EventCallback<TouchEventArgs> OnTouchLeave { get; set; }

    [Parameter]
    public int RowSpan { get; set; } = 1;

    [Parameter]
    public int ColumnSpan { get; set; } = 1;

    [Parameter]
    public string? Header { get; set; }

    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private string? InternalStyle => $"display: grid; grid-column-end: span {ColumnSpan}; grid-row-end: span {RowSpan}; {Style}";

    internal int Order { get; set; }

    private string HeaderClass => !Parent.CanReorder && !Parent.CanResize ? string.Empty : "touch-action-none";

    private bool HasHeader => HeaderTemplate is not null || !string.IsNullOrEmpty(Header);

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.Add(this);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            _isRendered = true;
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender &&
            Parent.CanResize)
        {
            _module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            await _module.InvokeVoidAsync("initialize", Id, _dotNetReference);
        }
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Parent is not null &&
            _isRendered &&
            _hasParameterChanged)
        {
            Parent.OnItemParemetersChanged(this);
        }
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasParameterChanged = parameters.HasValueChanged(nameof(RowSpan), RowSpan) ||
                               parameters.HasValueChanged(nameof(ColumnSpan), ColumnSpan) ||
                               parameters.HasValueChanged(nameof(Header), Header) ||
                               parameters.HasValueChanged(nameof(Class), Class) ||
                               parameters.HasValueChanged(nameof(IsVisible), IsVisible);

        return base.SetParametersAsync(parameters);
    }

    [JSInvokable]
    public async Task UpdateColumnAndRowSpan(FluentCxTileGridItemResizeEventArgs e)
    {
        if (Parent is not null)
        {
            var columnWidth = e.Parent.Width / Parent.Columns;
            var rowHeight = e.Original.Height / RowSpan;

            void UpdateColumnCount()
            {
                if (e.Original.Width - e.NewSize.Width < 0)
                {
                    var newSpan = e.NewSize.Width / columnWidth;
                    ColumnSpan = (int)Math.Round(newSpan);
                    ColumnSpan = Math.Min(ColumnSpan, Parent.Columns);
                }
                else
                {
                    var newSpan = e.NewSize.Width / columnWidth;
                    ColumnSpan = Math.Max(1, (int)Math.Round(newSpan));
                }
            }

            void UpdateRowCount()
            {
                var newSpan = e.NewSize.Height / rowHeight;

                if (e.Original.Height - e.NewSize.Height < 0)
                {
                    RowSpan = (int)Math.Round(newSpan);
                }
                else
                {
                    RowSpan = Math.Max(1, (int)Math.Round(newSpan));
                }
            }

            switch (e.Orientation)
            {
                case TileGridItemResizeHandle.Horizontally:
                    {
                        UpdateColumnCount();
                    }

                    break;

                case TileGridItemResizeHandle.Vertically:
                    {
                        UpdateRowCount();
                    }

                    break;

                case TileGridItemResizeHandle.Both:
                    {
                        UpdateColumnCount();
                        UpdateRowCount();
                    }

                    break;
            }

            if (_module is not null)
            {
                await _module.InvokeVoidAsync("columnAndRowSpanUpdated", Id, ColumnSpan, RowSpan);
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        Parent?.Remove(this);

        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("destroy", Id);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {

        }
    }
}

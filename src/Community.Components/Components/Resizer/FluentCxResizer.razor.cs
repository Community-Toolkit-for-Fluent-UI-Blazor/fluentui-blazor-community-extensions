using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxResizer
    : FluentComponentBase, IAsyncDisposable
{
    private IJSObjectReference? _module;
    private readonly string JavaScriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/Resizer/FluentCxResizer.razor.js";
    private readonly DotNetObjectReference<FluentCxResizer> _dotNetReferenceResizer;
    private bool _resetInitialization;

    public FluentCxResizer() : base()
    {
        Id = Identifier.NewId();
        _dotNetReferenceResizer = DotNetObjectReference.Create(this);
    }

    [Parameter]
    public bool IsResizeEnabled { get; set; } = true;

    [Parameter]
    public EventCallback<ResizedEventArgs> OnResized { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    [Parameter]
    public LocalizationDirection LocalizationDirection { get; set; } = LocalizationDirection.LeftToRight;

    private Dictionary<ResizerHandler, string> ResizeHandlers => ResizerHelper.GetFromLocalizationDirection(LocalizationDirection);

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string? SpanGridId { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnTapped { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleTapped { get; set; }

    private string? InternalClass => new CssBuilder(Class)
        .AddClass("fluentcx-resizer")
        .Build();

    private string? InternalStyle => new StyleBuilder(Style).Build();

    private async Task OnTappedAsync(MouseEventArgs e)
    {
        if (OnTapped.HasDelegate)
        {
            await OnTapped.InvokeAsync(e);
        }
    }

    private async Task OnDoubleTappedAsync(MouseEventArgs e)
    {
        if (OnDoubleTapped.HasDelegate)
        {
            await OnDoubleTapped.InvokeAsync(e);
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavaScriptFile);
            await _module.InvokeVoidAsync("initialize", Id, _dotNetReferenceResizer, SpanGridId);
        }

        if (_resetInitialization &&
            _module is not null)
        {
            _resetInitialization = false;
            await _module.InvokeVoidAsync("setTileGrid", Id, SpanGridId);
        }
    }

    [JSInvokable]
    public async Task Resized(ResizedEventArgs e)
    {
        if (OnResized.HasDelegate)
        {
            await OnResized.InvokeAsync(e);
        }
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(SpanGridId), SpanGridId))
        {
            _resetInitialization = true;
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("destroy", Id);
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException)
        {

        }
    }
}

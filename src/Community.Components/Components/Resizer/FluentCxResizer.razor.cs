using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a resizer component.
/// </summary>
public partial class FluentCxResizer
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents the javascript module instance to use for resizing the component.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the javascript file to use for resizing the component.
    /// </summary>
    private const string JavaScriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/Resizer/FluentCxResizer.razor.js";

    /// <summary>
    /// Represents the reference of the <see cref="FluentCxResizer"/> inside the javascript functions.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxResizer> _dotNetReferenceResizer;

    /// <summary>
    /// Flag indicating if we need to reset the initialization of the component.
    /// </summary>
    private bool _resetInitialization;

    /// <summary>
    /// Initialize a new instance of the <see cref="FluentCxResizer"/> class.
    /// </summary>
    public FluentCxResizer() : base()
    {
        Id = Identifier.NewId();
        _dotNetReferenceResizer = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or set a value indicating if the resize is enabled.
    /// </summary>
    [Parameter]
    public bool IsResizeEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets an event callback which occurs when a resize operation is performed.
    /// </summary>
    [Parameter]
    public EventCallback<ResizedEventArgs> OnResized { get; set; }

    /// <summary>
    /// Gets or sets the JSRuntime.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the reading direction of the objects in the UI.
    /// </summary>
    [Parameter]
    public LocalizationDirection LocalizationDirection { get; set; } = LocalizationDirection.LeftToRight;

    /// <summary>
    /// Gets the resized handler to use regarding the reading direction.
    /// </summary>
    private Dictionary<ResizerHandler, string> ResizeHandlers => ResizerHelper.GetFromLocalizationDirection(LocalizationDirection);

    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the id of the <see cref="FluentCxTileGrid{TItem}"/>
    /// </summary>
    [Parameter]
    public string? SpanGridId { get; set; }

    /// <summary>
    /// Gets or sets an event callback when the component is tapped.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnTapped { get; set; }

    /// <summary>
    /// Gets or sets an event callback when the component is double tapped.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnDoubleTapped { get; set; }

    /// <summary>
    /// Gets the internal class the component use.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("fluentcx-resizer")
        .Build();

    /// <summary>
    /// Gets the internal style the component use.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style).Build();

    /// <summary>
    /// Occurs when the component is tapped.
    /// </summary>
    /// <param name="e">Event args which contains information about the mouse.</param>
    /// <returns>Returns a task which invokes <see cref="OnTapped"/> when completed.</returns>
    private async Task OnTappedAsync(MouseEventArgs e)
    {
        if (OnTapped.HasDelegate)
        {
            await OnTapped.InvokeAsync(e);
        }
    }

    /// <summary>
    /// Occurs when the component is double tapped.
    /// </summary>
    /// <param name="e">Event args which contains information about the mouse.</param>
    /// <returns>Returns a task which invokes <see cref="OnDoubleTapped"/> when completed.</returns>
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

    /// <summary>
    /// Occurs when the javascript completes the resize.
    /// </summary>
    /// <param name="e">Event args which contains the data of the resize.</param>
    /// <returns>Returns a task which invokes <see cref="OnResized"/> when completed.</returns>
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

        GC.SuppressFinalize(this);
    }
}

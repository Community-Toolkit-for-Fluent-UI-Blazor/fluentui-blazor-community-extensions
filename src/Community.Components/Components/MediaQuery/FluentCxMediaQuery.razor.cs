using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a Media Query detector.
/// </summary>
public partial class FluentCxMediaQuery
    : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents the javascript file to manage the media query.
    /// </summary>
    private const string JavascriptFile = "./_content/FluentUI.Blazor.Community.Components/Components/MediaQuery/FluentCxMediaQuery.razor.js";

    /// <summary>
    /// Represents the javascript module.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents a value indicating if the component was first rendered.
    /// </summary>
    private bool _hasRender;

    /// <summary>
    /// Represents a value to dispose the module each time the <see cref="Query"/> changes.
    /// </summary>
    private bool _disposeQueryModule;

    public FluentCxMediaQuery()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the query to manage.
    /// </summary>
    [Parameter]
    public string? Query { get; set; }

    /// <summary>
    /// Gets or sets the callback to raise when the media changed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnMediaChanged { get; set; }

    /// <summary>
    /// Gets or sets the Javascript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("dispose", Id);
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException)
        {
        }

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _hasRender = true;
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFile);
            await InitializeMediaQueryAsync();
        }

        if (_disposeQueryModule)
        {
            await InitializeMediaQueryAsync(true);
        }
    }

    private async Task InitializeMediaQueryAsync(bool dispose = false)
    {
        if (_module is not null)
        {
            if (dispose)
            {
                _disposeQueryModule = false;
                await _module.InvokeVoidAsync("dispose", Id);
            }

            var match = await _module.InvokeAsync<bool>("initialize", Id, DotNetObjectReference.Create(this), Query);
            await OnMediaQueryChangedAsync(match);
        }
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Query), Query) && _hasRender)
        {
            _disposeQueryModule = true;
        }
    }

    /// <summary>
    /// Occurs when the media changes.
    /// </summary>
    /// <param name="change">Value indicating if the media has changed.</param>
    /// <returns>Returns a task which raises the event callback <see cref="OnMediaChanged"/> when completed.</returns>
    [JSInvokable]
    public async Task OnMediaQueryChangedAsync(bool change)
    {
        if (OnMediaChanged.HasDelegate)
        {
            await OnMediaChanged.InvokeAsync(change);
        }
    }
}

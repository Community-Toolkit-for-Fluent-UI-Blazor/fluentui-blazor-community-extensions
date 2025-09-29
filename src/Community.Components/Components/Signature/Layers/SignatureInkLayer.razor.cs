using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an ink layer component for capturing and displaying user signatures.
/// </summary>
public partial class SignatureInkLayer
    : FluentComponentBase
{
    /// <summary>
    /// Reference to the grid layer HTML element.
    /// </summary>
    private ElementReference _inkLayerElement;

    /// <summary>
    /// Represents the JavaScript module for the ink layer.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the width of the ink layer.
    /// </summary>
    private int _width;

    /// <summary>
    /// Represents the height of the ink layer.
    /// </summary>
    private int _height;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureInkLayer"/> class.
    /// </summary>
    public SignatureInkLayer()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Get or sets the pen options for the ink layer.
    /// </summary>
    [Parameter]
    public SignaturePenOptions PenOptions { get; set; } = new SignaturePenOptions();

    /// <summary>
    /// Get or sets the eraser options for the ink layer.
    /// </summary>
    [Parameter]
    public SignatureEraserOptions EraserOptions { get; set; } = new SignatureEraserOptions();

    /// <summary>
    /// Gets the internal CSS class for the component.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("signature-ink-layer")
        .Build();

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Signature/Layers/SignatureInkLayer.razor.js");
            await _module.InvokeVoidAsync("init", _inkLayerElement, PenOptions);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
                _module = null;
            }
        }
        catch (JSDisconnectedException)
        {
        }

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Updates the viewbox of the grid layer based on the provided width and height.
    /// </summary>
    /// <param name="width">Width of the component.</param>
    /// <param name="height">Height of the component.</param>
    /// <returns>Returns a task which updates the viewbox when completed.</returns>
    internal async Task UpdateViewboxAsync(int width, int height)
    {
        _width = width;
        _height = height;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Clears the current signature from the ink layer.
    /// </summary>
    /// <returns>Returns a task which clear the ink layer when completed.</returns>
    internal async Task ClearAsync() => await _module!.InvokeVoidAsync("clear", _inkLayerElement);

    /// <summary>
    /// Undoes the last action on the ink layer.
    /// </summary>
    /// <returns>Returns a task which undo the last action on the ink layer when completed.</returns>
    internal async Task UndoAsync() => await _module!.InvokeVoidAsync("undo", _inkLayerElement);

    /// <summary>
    /// Redos the last undone action on the ink layer.
    /// </summary>
    /// <returns>Returns a task which redo the last action on the ink layer when completed.</returns>
    internal async Task RedoAsync() => await _module!.InvokeVoidAsync("redo", _inkLayerElement);

    /// <summary>
    /// Sets the erase mode on the ink layer.
    /// </summary>
    /// <param name="enabled">Value indicating if the erase mode is enabled.</param>
    /// <returns>Returns a task which set the erase mode when completed.</returns>
    internal async Task SetEraseModeAsync(bool enabled) => await _module!.InvokeVoidAsync("setEraseMode", _inkLayerElement, enabled, EraserOptions);

    /// <summary>
    /// Refreshes the ink layer with the current pen options.
    /// </summary>
    /// <returns>Returns a task which updates the pen options.</returns>
    internal async Task RefreshAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("setInkOptions", _inkLayerElement, PenOptions);
            await InvokeAsync(StateHasChanged);
        }
    }
}

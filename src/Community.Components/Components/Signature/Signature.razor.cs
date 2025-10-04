using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a signature component that allows users to draw and capture signatures.
/// </summary>
public partial class Signature
    : FluentComponentBase
{
    /// <summary>
    /// Represents the JavaScript module reference used for interacting with JavaScript functions.
    /// </summary>
    /// <remarks>This field holds a reference to a JavaScript module loaded via the JavaScript interop system.
    /// It may be <see langword="null"/> if the module has not been initialized or has been disposed.</remarks>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the grid layer component used for displaying a grid background.
    /// </summary>
    private SignatureGridLayer? _gridLayer;

    /// <summary>
    /// Represents the ink layer component used for capturing and displaying user-drawn signatures.
    /// </summary>
    private SignatureInkLayer? _inkLayer;

    /// <summary>
    /// Represents the overlay layer component used for displaying additional overlays on top of the signature.
    /// </summary>
    private SignatureOverlayLayer? _overlayLayer;

    /// <summary>
    /// Initializes a new instance of the <see cref="Signature"/> class.
    /// </summary>
    public Signature()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Gets or sets the signature options for configuring the signature component.
    /// </summary>
    [Parameter]
    public SignatureOptions Options { get; set; } = new();

    /// <summary>
    /// Gets the internal CSS class for the component.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("signature-container")
        .Build();

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Signature/Signature.razor.js");
        }
    }

    /// <summary>
    /// Exports the current signature as an SVG string.
    /// </summary>
    /// <returns>Returns a task which exports the svg as a string when completed.</returns>
    public async Task<string> ExportSvgAsync()
    {
        if (_module is not null)
        {
            return await _module.InvokeAsync<string>("exportSvg", _gridLayer!.Id, _inkLayer!.Id, _overlayLayer!.Id);
        }

        return string.Empty;
    }

    /// <summary>
    /// Clears the current signature from the ink layer.
    /// </summary>
    /// <returns>Returns a task which clear the signature.</returns>
    public async Task ClearAsync() => await _inkLayer!.ClearAsync();

    /// <summary>
    /// Undoes the last action on the ink layer.
    /// </summary>
    /// <returns>Returns a task which undo the last action on the ink layer when completed.</returns>
    public async Task UndoAsync() => await _inkLayer!.UndoAsync();

    /// <summary>
    /// Redos the last undone action on the ink layer.
    /// </summary>
    /// <returns>Returns a task which redo the last action on the ink layer when completed.</returns>
    public async Task RedoAsync() => await _inkLayer!.RedoAsync();

    /// <summary>
    /// Sets the erase mode on the ink layer.
    /// </summary>
    /// <param name="enabled">Value indicating if the erase mode is enabled.</param>
    /// <returns>Returns a task which set the erase mode when completed.</returns>
    public async Task SetEraseModeAsync(bool enabled) => await _inkLayer!.SetEraseModeAsync(enabled);

    /// <summary>
    /// Refreshes the grid layer to reflect any changes in options or state.
    /// </summary>
    /// <returns>Returns a task which refreshs the grid when completed.</returns>
    internal async Task RefreshGridAsync()
    {
        if (_gridLayer is not null)
        {
            await _gridLayer.RefreshAsync();
        }
    }

    /// <summary>
    /// Refreshes the ink layer to reflect any changes in options or state.
    /// </summary>
    /// <returns>Returns a task which refreshs the ink layer when completed.</returns>
    internal async Task RefreshInkAsync()
    {
        if (_inkLayer is not null)
        {
            await _inkLayer.RefreshAsync();
        }
    }

    /// <summary>
    /// Refreshes the overlay layer to reflect any changes in options or state.
    /// </summary>
    /// <returns>Returns a task which refreshs the overlay layer when completed.</returns>
    internal async Task RefreshOverlayAsync()
    {
        if (_overlayLayer is not null)
        {
            await _overlayLayer.RefreshAsync();
        }
    }

    /// <inheritdoc />
    internal async ValueTask OnResizedAsync(int width, int height)
    {
        if (_gridLayer is not null)
        {
            await _gridLayer.UpdateViewboxAsync(width, height);
        }

        if (_inkLayer is not null)
        {
            await _inkLayer.UpdateViewboxAsync(width, height);
        }

        if (_overlayLayer is not null)
        {
            await _overlayLayer.UpdateViewboxAsync(width, height);
        }
    }
}

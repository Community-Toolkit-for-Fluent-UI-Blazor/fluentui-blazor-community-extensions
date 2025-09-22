using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an overlay layer component for capturing and displaying user signatures.
/// </summary>
public partial class SignatureOverlayLayer
    : FluentComponentBase
{
    /// <summary>
    /// Reference to the grid layer HTML element.
    /// </summary>
    private ElementReference _overlayLayerElement;

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
    /// Initializes a new instance of the <see cref="SignatureOverlayLayer"/> class.
    /// </summary>
    public SignatureOverlayLayer()
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
    public SignatureWatermarkOptions Options { get; set; } = new SignatureWatermarkOptions();

    /// <summary>
    /// Gets the internal CSS class for the component.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("signature-overlay-layer")
        .AddClass("watermark-image", !string.IsNullOrEmpty(Options.ImageUrl))
        .Build();

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Signature/Layers/SignatureOverlayLayer.razor.js");
            await _module.InvokeVoidAsync("updateOverlay", _overlayLayerElement, Options);
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
        await RefreshAsync();
    }

    /// <summary>
    /// Refreshes the overlay layer by invoking the JavaScript function to update the overlay with the current options.
    /// </summary>
    /// <returns>Returns a task which updates the overlayer when completed.</returns>
    internal async Task RefreshAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("updateOverlay", _overlayLayerElement, Options);
        }
    }
}

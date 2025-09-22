using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a grid layer component for displaying a grid on a signature component.
/// </summary>
public partial class SignatureGridLayer
    : FluentComponentBase
{
    /// <summary>
    /// Reference to the grid layer HTML element.
    /// </summary>
    private ElementReference _gridLayerElement;

    /// <summary>
    /// Represents the JavaScript module for the grid layer.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the width of the grid layer.
    /// </summary>
    private int _width;

    /// <summary>
    /// Represents the height of the grid layer.
    /// </summary>
    private int _height;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureGridLayer"/> class.
    /// </summary>
    public SignatureGridLayer()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Get or sets the grid options for the grid layer.
    /// </summary>
    [Parameter]
    public SignatureGridOptions Options { get; set; } = new SignatureGridOptions();

    /// <summary>
    /// Gets the internal CSS class for the component.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class)
        .AddClass("signature-grid-layer")
        .Build();

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Signature/Layers/SignatureGridLayer.razor.js");
            await _module.InvokeVoidAsync("updateGrid", _gridLayerElement, Options);
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
    /// Refreshes the grid layer by invoking the JavaScript function to update the grid.
    /// </summary>
    /// <returns></returns>
    internal async Task RefreshAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("updateGrid", _gridLayerElement, Options);
        }
    }
}

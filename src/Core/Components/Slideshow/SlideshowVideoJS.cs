using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.States;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to initialize and dispose of a JavaScript slideshow video component for use in Blazor
/// applications.
/// </summary>
/// <remarks>This class manages the lifecycle of a JavaScript-based slideshow image component, ensuring that
/// resources are properly initialized and disposed. It is intended for internal use within Blazor components that
/// require integration with JavaScript slideshow functionality.</remarks>
/// <param name="fluentJSModule">The JavaScript module loader used to import the slideshow image JavaScript module required for interoperation.</param>
/// <param name="state">The state management object that maintains the state of the slideshow images.</param>
internal sealed class SlideshowVideoJS(
    FluentJSModule fluentJSModule,
    SlideshowState state)
{
    private IJSObjectReference? _module;
    private const string JavascriptModulePath = FluentCxConstants.JAVASCRIPT_ROOT + "Slideshow/SlideshowVideo.razor.js";

    public async Task Initialize(string? id)
    {
        _module = await fluentJSModule.ImportJavaScriptModuleAsync(JavascriptModulePath);
        await _module.InvokeVoidAsync("FluentUI.Blazor.Community.SlideshowVideo.Initialize", id, DotNetObjectReference.Create(this));
    }

    public async ValueTask DisposeAsync(string? id)
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.SlideshowVideo.Dispose", id);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
    }

    /// <summary>
    /// Handles the event triggered when an video's dimensions have been measured and updates the internal state with
    /// the new size information.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript via interop when a video measurement
    /// occurs, ensuring that the component's state remains synchronized with the latest video dimensions.</remarks>
    /// <param name="e">The event arguments containing the identifier of the measured video and its width and height values.</param>
    [JSInvokable]
    public void OnVideoMeasured(SlideshowMeasuredEventArgs e)
    {
        state.AddOrUpdateSize(e.Id, e.Width, e.Height);
    }
}

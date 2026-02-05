using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.States;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to initialize and dispose of a JavaScript slideshow image component for use in Blazor
/// applications.
/// </summary>
/// <remarks>This class manages the lifecycle of a JavaScript-based slideshow image component, ensuring that
/// resources are properly initialized and disposed. It is intended for internal use within Blazor components that
/// require integration with JavaScript slideshow functionality.</remarks>
/// <typeparam name="TItem">Specifies the type of items displayed by the slideshow component.</typeparam>
/// <param name="fluentJSModule">The JavaScript module loader used to import the slideshow image JavaScript module required for interoperation.</param>
/// <param name="state">The state management object that maintains the state of the slideshow images.</param>
internal sealed class SlideshowImageJS<TItem>(
    FluentJSModule fluentJSModule,
    SlideshowState state)
{
    private IJSObjectReference? _module;
    private const string JavascriptModulePath = FluentCxConstants.JAVASCRIPT_ROOT + "Slideshow/SlideshowImage.razor.js";

    public async Task Initialize(string? id)
    {
        _module = await fluentJSModule.ImportJavaScriptModuleAsync(JavascriptModulePath);
        await _module.InvokeVoidAsync("FluentUI.Blazor.Community.SlideshowImage.Initialize", id, DotNetObjectReference.Create(this));
    }

    public async ValueTask DisposeAsync(string? id)
    {
        try
        {
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.SlideshowImage.Dispose", id);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException) { }
    }

    /// <summary>
    /// Handles the event triggered when an image's dimensions have been measured and updates the internal state with
    /// the new size information.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript via interop when an image measurement
    /// occurs, ensuring that the component's state remains synchronized with the latest image dimensions.</remarks>
    /// <param name="e">The event arguments containing the identifier of the measured image and its width and height values.</param>
    [JSInvokable]
    public void OnImageMeasured(SlideshowMeasuredEventArgs e)
    {
        state.AddOrUpdateSize(e.Id, e.Width, e.Height);
    }
}

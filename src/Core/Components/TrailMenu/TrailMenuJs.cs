using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the javaScript interop layer for the <see cref="FluentCxTrailMenu"/> component.
/// </summary>
internal sealed class TrailMenuJs
{
    /// <summary>
    /// Represents the JavaScript module used for interop operations.
    /// </summary>
    private readonly FluentJSModule _js;

    /// <summary>
    /// Represents the javascript object reference.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Repreesnts the name of the javascript file to load.
    /// </summary>
    private const string JavascriptModule = FluentCxConstants.JAVASCRIPT_ROOT + "TrailMenu/FluentCxTrailMenu.razor.js";

    /// <summary>
    /// Represents the dot net reference inside the javascript.
    /// </summary>
    private DotNetObjectReference<TrailMenuJs>? _dotNetRef;

    /// <summary>
    /// Represents the instance of the trail menu.
    /// </summary>
    private readonly FluentCxTrailMenu _trailMenu;

    /// <summary>
    /// Initializes a new instance of the TrailMenuJs class, enabling integration of JavaScript functionality with a
    /// specified trail menu component.
    /// </summary>
    /// <remarks>Use this constructor to associate a FluentCxTrailMenu instance with a FluentJSModule,
    /// allowing the trail menu to leverage JavaScript features for enhanced user interaction.</remarks>
    /// <param name="js">The JavaScript module used to provide client-side functionality required by the trail menu.</param>
    /// <param name="trailMenu">The trail menu component to be managed and enhanced with JavaScript interactions.</param>
    public TrailMenuJs(FluentJSModule js, FluentCxTrailMenu trailMenu)
    {
        _trailMenu = trailMenu;
        _js = js;
    }

    /// <summary>
    /// Initializes the component asynchronously by importing the required JavaScript module and invoking its
    /// initialization function.
    /// </summary>
    /// <remarks>Call this method before using the component to ensure that the JavaScript module is loaded
    /// and properly configured. The method must be awaited to guarantee that initialization completes before further
    /// interaction.</remarks>
    /// <param name="id">The unique identifier for the component instance. This value may be null if an identifier is not required.</param>
    /// <param name="config">A dictionary containing configuration options that determine the behavior of the component during
    /// initialization.</param>
    /// <returns>A task that represents the asynchronous operation of initializing the component.</returns>
    public async Task InitializeAsync(
        string? id,
        Dictionary<string, object> config)
    {
        _dotNetRef ??= DotNetObjectReference.Create(this);
        _module ??= await _js.ImportJavaScriptModuleAsync(JavascriptModule);
        await _module.InvokeVoidAsync("FluentUI.Blazor.Community.TrailMenu.Initialize", id, _dotNetRef, config);
    }

    /// <summary>
    /// Asynchronously retrieves the width value associated with the specified identifier.
    /// </summary>
    /// <remarks>This method invokes a remote service to obtain the width. An exception may be thrown if the
    /// service is unavailable or if the identifier is invalid.</remarks>
    /// <param name="id">The unique identifier for which to obtain the width. This parameter cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the width as a double-precision
    /// floating-point value.</returns>
    public async Task<double> GetWidthAsync(string id)
    {
        return await _module!.InvokeAsync<double>("FluentUI.Blazor.Community.TrailMenu.GetWidth", id);
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance, including any associated JavaScript resources.
    /// </summary>
    /// <remarks>This method ensures that all managed and JavaScript resources are properly released. If the
    /// JavaScript runtime is disconnected, any exceptions thrown during disposal are ignored.</remarks>
    /// <param name="id">The identifier used to dispose of specific resources associated with the instance. Specify <see
    /// langword="null"/> to dispose all resources without targeting a specific identifier.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation of disposing resources.</returns>
    public async ValueTask DisposeAsync(string? id)
    {
        try
        {
            _dotNetRef?.Dispose();
            _dotNetRef = null;
            if (_module is not null)
            {
                await _module.InvokeVoidAsync("FluentUI.Blazor.Community.TrailMenu.Dispose", id);
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // The JS runtime is already disconnected, so we can safely ignore this exception.
        }
    }

    /// <summary>
    /// Invokes the asynchronous resize operation for the trail menu with the specified width.
    /// </summary>
    /// <remarks>This method is marked with <see cref="JSInvokableAttribute"/>, allowing it to be called from
    /// JavaScript code. Ensure that the <paramref name="width"/> parameter is valid before invoking this
    /// method.</remarks>
    /// <param name="width">The new width, in pixels, to which the trail menu should be resized. Must be a positive value.</param>
    /// <returns>A task that represents the asynchronous operation of resizing the trail menu.</returns>
    [JSInvokable("OnResize")]
    public Task OnResizeAsync(double width)
    {
        return _trailMenu.OnResizeAsync(width); 
    }

    /// <summary>
    /// Invokes the mutation handler asynchronously to update the trail menu state when called from JavaScript.
    /// </summary>
    /// <remarks>This method is marked with <see cref="JSInvokableAttribute"/>, allowing it to be invoked from
    /// JavaScript code in Blazor applications. It delegates the mutation handling to the underlying trail menu
    /// component.</remarks>
    /// <returns>A task that represents the asynchronous operation of the mutation handler.</returns>
    [JSInvokable("OnMutated")]
    public Task OnMutatedAsync()
    {
        return _trailMenu.OnMutatedAsync();
    }
}


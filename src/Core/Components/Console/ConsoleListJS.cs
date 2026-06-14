using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality for interacting with JavaScript code in a Blazor application using asynchronous operations.
/// </summary>
internal class ConsoleListJS(
    ElementReference elementReference,
    ConsoleList instance,
    FluentJSModule module) : IAsyncDisposable
{
    /// <summary>
    /// Gets or sets the JavaScript object reference used for interop with the client-side JavaScript.
    /// </summary>
    /// <remarks>This field holds a reference to a JavaScript object that can be used to call JavaScript
    /// functions from .NET code. It is initialized when the JavaScript object is created and should be disposed of
    /// properly to avoid memory leaks.</remarks>
    private IJSObjectReference? _jSObjectReference;

    /// <summary>
    /// Holds a reference to the current ConsoleListJS instance for JavaScript interop operations.
    /// </summary>
    /// <remarks>This field is used to enable JavaScript code to invoke .NET methods on the ConsoleListJS
    /// instance via Blazor's JavaScript interop. The reference should be properly initialized before use to prevent
    /// null reference exceptions, and disposed of when no longer needed to avoid memory leaks.</remarks>
    private DotNetObjectReference<ConsoleListJS>? _dotNetObjectReference;

    /// <summary>
    /// Gets the path to the JavaScript file required for the console list component.
    /// </summary>
    /// <remarks>This constant specifies the location of the JavaScript file that enables client-side
    /// functionality for the console list. Ensure that the file exists at the specified path to prevent runtime
    /// errors.</remarks>
    private const string JavascriptFileName = FluentCxConstants.JAVASCRIPT_ROOT + "Console/ConsoleList.razor.js";

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jSObjectReference is not null)
            {
                await _jSObjectReference.DisposeAsync();
                _dotNetObjectReference = null;
            }
        }
        catch (JSDisconnectedException) { }

        _dotNetObjectReference?.Dispose();
        _dotNetObjectReference = null;
    }

    /// <summary>
    /// Asynchronously initializes the component by importing the required JavaScript module and establishing interop
    /// references.
    /// </summary>
    /// <remarks>This method must be called before interacting with the component to ensure that JavaScript
    /// interop is properly set up. Failing to initialize may result in runtime errors when invoking
    /// JavaScript-dependent functionality.</remarks>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    internal async Task InitializeAsync()
    {
        _dotNetObjectReference ??= DotNetObjectReference.Create(this);
        _jSObjectReference = await module.ImportJavaScriptModuleAsync(JavascriptFileName);
        await _jSObjectReference.InvokeVoidAsync("FluentUI.Blazor.Community.Components.ConsoleList.Initialize", elementReference, _dotNetObjectReference);
    }

    /// <summary>
    /// Asynchronously scrolls the console list to the bottom, ensuring that the most recent entries are visible to the
    /// user.
    /// </summary>
    /// <remarks>This method requires a valid JavaScript object reference to function. If the reference is
    /// null, the method does not perform any action.</remarks>
    /// <returns>A task that represents the asynchronous scroll operation.</returns>
    internal async Task ScrollToBottomAsync()
    {
        if (_jSObjectReference is not null)
        {
            await _jSObjectReference.InvokeVoidAsync("FluentUI.Blazor.Community.Components.ConsoleList.ScrollToBottom", elementReference);
        }
    }

    /// <summary>
    /// Determines whether the console list is currently scrolled to the bottom.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to check the scroll position of the console list. A
    /// valid JavaScript object reference is required for this operation to succeed.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains <see langword="true"/> if the
    /// console list is at the bottom; otherwise, <see langword="false"/>.</returns>
    internal async Task<bool> IsAtBottomAsync()
    {
        if (_jSObjectReference is not null)
        {
            return await _jSObjectReference.InvokeAsync<bool>("FluentUI.Blazor.Community.Components.ConsoleList.IsAtBottom", elementReference, 5);
        }

        return false;
    }

    /// <summary>
    /// Invokes the auto-scrolling behavior for the console list if the specified condition is met.
    /// </summary>
    /// <remarks>This method requires a valid JavaScript object reference to function correctly. It is
    /// intended for use in scenarios where dynamic content updates may necessitate scrolling.</remarks>
    /// <param name="autoScroll">A value indicating whether auto-scrolling should be enabled. If set to <see langword="true"/>, the console list
    /// will automatically scroll to the most recent entry.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task AutoScrollIfNeededAsync(bool autoScroll)
    {
        if (_jSObjectReference is not null)
        {
            await _jSObjectReference.InvokeVoidAsync("FluentUI.Blazor.Community.Components.ConsoleList.AutoScrollIfNeeded", elementReference, autoScroll);
        }
    }

    /// <summary>
    /// Invoked when the scroll position changes to indicate whether the view is currently at the bottom of the
    /// scrollable content.
    /// </summary>
    /// <remarks>This method is typically used to trigger actions when the user reaches the bottom of a
    /// scrollable area, such as loading additional content or updating the user interface.</remarks>
    /// <param name="isAtBottom">true if the scroll position is at the bottom of the content; otherwise, false.</param>
    [JSInvokable]
    public void OnScrollChanged(bool isAtBottom)
    {
        instance.OnScrollChanged(isAtBottom);
    }
}

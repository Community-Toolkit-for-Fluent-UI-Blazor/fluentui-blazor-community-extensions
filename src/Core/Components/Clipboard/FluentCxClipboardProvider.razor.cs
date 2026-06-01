using FluentUI.Blazor.Community.Components.Clipboard;
using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a provider component for clipboard functionality within the Fluent UI Blazor Community library.
/// </summary>
public partial class FluentCxClipboardProvider : FluentComponentBase
{
    /// <summary>
    /// Represents the relative path to the JavaScript file used by the FluentCx clipboard provider component.
    /// </summary>
    /// <remarks>This constant is intended for internal use when referencing the associated JavaScript file
    /// required for clipboard functionality in FluentCx components.</remarks>
    private const string JavascriptFileName = FluentCxConstants.JAVASCRIPT_ROOT + "Clipboard/FluentCxClipboardProvider.razor.js";

    /// <summary>
    /// Represents a reference to the JavaScript module used by the clipboard provider for interop operations.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Initializes a new instance of the FluentCxClipboardProvider class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings used to initialize the clipboard provider. Cannot be null.</param>
    public FluentCxClipboardProvider(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the service used to initialize clipboard functionality within the component.
    /// </summary>
    /// <remarks>This property is typically provided by dependency injection and enables clipboard
    /// functionality. Assigning a custom implementation allows customization of clipboard behavior.</remarks>
    [Inject]
    private IClipboardInitializer ClipboardInitializer { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSModule.ImportJavaScriptModuleAsync(JavascriptFileName);
            ClipboardInitializer.Initialize(_module);
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        { }

        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}

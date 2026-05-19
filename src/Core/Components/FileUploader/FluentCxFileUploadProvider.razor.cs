using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality for managing file uploads within Fluent UI Blazor community extensions.
/// </summary>
/// <remarks>This provider serves as an integration point for file upload features that are not included in the
/// core Fluent UI Blazor library. It is intended for use within applications that require extended or custom file
/// upload capabilities.</remarks>
public partial class FluentCxFileUploadProvider : FluentComponentBase
{
    /// <summary>
    /// Represents the relative path to the JavaScript file used by the FluentCx file upload provider component.
    /// </summary>
    /// <remarks>This constant is intended for internal use when referencing the associated JavaScript file
    /// required for file upload functionality in FluentCx components.</remarks>
    private const string JavascriptFileName = FluentCxConstants.JAVASCRIPT_ROOT + "FileUploader/FluentCxFileUploadProvider.razor.js";

    /// <summary>
    /// Represents a reference to the JavaScript module used by the file upload provider for interop operations.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Initializes a new instance of the FluentCxFileUploadProvider class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings used to initialize the file upload provider. Cannot be null.</param>
    public FluentCxFileUploadProvider(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the service used to upload files within the component.
    /// </summary>
    /// <remarks>This property is typically provided by dependency injection and enables file upload
    /// functionality. Assigning a custom implementation allows customization of file upload behavior.</remarks>
    [Inject]
    private IFileUploader FileUploader { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSModule.ImportJavaScriptModuleAsync(JavascriptFileName);
            FileUploader.Initialize(_module);
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
    }
}

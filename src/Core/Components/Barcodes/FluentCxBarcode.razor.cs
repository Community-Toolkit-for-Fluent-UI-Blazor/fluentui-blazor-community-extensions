using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a barcode component that integrates with Fluent UI Blazor, enabling the display of barcodes within Blazor
/// applications.
/// </summary>
public partial class FluentCxBarcode
    : FluentComponentBase
{
    /// <summary>
    /// Provides the rendering target used to generate SVG representations of barcodes.
    /// </summary>
    private readonly BarcodeSvgRenderTarget _renderTarget = new();

    /// <summary>
    /// Gets or sets the symbology used for barcode generation or interpretation.
    /// </summary>
    /// <remarks>The symbology defines the barcode standard or format, such as QR Code, Data Matrix, or Code
    /// 128. Setting this property determines how the barcode is encoded and decoded. Ensure that the assigned symbology
    /// is compatible with the intended barcode operations.</remarks>
    private ISymbology? _symbology;

    /// <summary>
    /// Value indicating whether the Value parameter has changed during parameter updates,
    ///  which may require re-rendering.
    /// </summary>
    private bool _hasValueChanged;

    /// <summary>
    /// Value indicating whether the Height parameter has changed during parameter updates,
    ///  which may require re-rendering.
    /// </summary>
    private bool _hasHeightChanged;

    /// <summary>
    /// Value indicating whether the Width parameter has changed during parameter updates,
    ///  which may require re-rendering.
    /// </summary>
    private bool _hasWidthChanged;

    /// <summary>
    /// 
    /// </summary>
    private readonly BarcodeRendererFactory _barcodeRendererFactory = new();

    /// <summary>
    /// Provides the surface renderer used to render barcodes with the specified rendering options.
    /// </summary>
    /// <remarks>This field holds a reference to an implementation of the ISurfaceRenderer interface for
    /// barcode rendering. It may be null if no renderer has been assigned.</remarks>
    private ISurfaceRenderer<BarcodeRenderingOptions>? surfaceRenderer;

    /// <summary>
    /// Provides the default configuration options for the surface view component.
    /// </summary>
    private readonly SurfaceViewOptions _surfaceViewOptions = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxBarcode"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public FluentCxBarcode(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets a value indicating whether the operation should be performed asynchronously.
    /// </summary>
    [Parameter]
    public bool IsAsync { get; set; }

    /// <summary>
    /// Gets or sets the current value of the component.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the custom symbology content to render within the component.
    /// </summary>
    /// <remarks>Use this property to provide additional visual elements, such as icons or symbols, that
    /// represent or enhance the component's appearance. The content is rendered as a Blazor fragment and can include
    /// arbitrary markup or other components.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the options used to configure the background appearance of the surface.
    /// </summary>
    /// <remarks>Use this property to specify custom background settings, such as color or image, for the
    /// surface component. If not set, default background options are applied.</remarks>
    [Parameter]
    public SurfaceBackgroundOptions BackgroundOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the foreground appearance of the surface.
    /// </summary>
    [Parameter]
    public SurfaceForegroundOptions ForegroundOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the options used to configure the appearance and behavior of barcode labels.
    /// </summary>
    /// <remarks>Use this property to customize label formatting, positioning, and other display settings for
    /// barcodes. Changes to these options affect how labels are rendered within the barcode component.</remarks>
    [Parameter]
    public BarcodeLabelOptions LabelOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the options that define the quiet zone around the barcode.
    /// </summary>
    /// <remarks>The quiet zone is the blank margin surrounding the barcode, which is required for proper
    /// scanning. Adjust these options to control the size and appearance of the quiet zone based on the requirements of
    /// the barcode reader or the intended use case.</remarks>
    [Parameter]
    public BarcodeQuietZoneOptions QuietZone { get; set; } = new();

    /// <summary>
    /// Gets or sets the width of the <see cref="FluentCxBarcode" /> component.
    /// </summary>
    [Parameter]
    public double Width { get; set; } = 200;

    /// <summary>
    /// Gets or sets the height of the <see cref="FluentCxBarcode" /> component.
    /// </summary>
    [Parameter]
    public double Height { get; set; } = 200;

    /// <summary>
    /// Gets or sets the SVG markup representing the generated barcode.
    /// </summary>
    private MarkupString SvgBarcode { get; set; }

    /// <summary>
    /// Gets the computed inline CSS style string for positioning the element based on the current view coordinates.
    /// </summary>
    private string? InternalStyle => new StyleBuilder()
        .AddStyle("position", "absolute")
        .AddStyle("left", $"{_surfaceViewOptions.RenderLeft}px", _surfaceViewOptions.RenderLeft > 0)
        .AddStyle("top", $"{_surfaceViewOptions.RenderTop}px", _surfaceViewOptions.RenderTop > 0)
        .Build();

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        await RenderAsync();
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasValueChanged)
        {
            _hasValueChanged = false;
            await RenderAsync();
        }

        if (_hasHeightChanged || _hasWidthChanged)
        {
            _hasHeightChanged = false;
            _hasWidthChanged = false;
            _surfaceViewOptions.RenderWidth = Width;
            _surfaceViewOptions.RenderHeight = Height;

            await RenderAsync();
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasValueChanged = parameters.TryGetValue<string?>(nameof(Value), out var newValue) && !string.Equals(newValue, Value, StringComparison.Ordinal);
        _hasHeightChanged = parameters.TryGetValue<double>(nameof(Height), out var newHeight) && newHeight != Height;
        _hasWidthChanged = parameters.TryGetValue<double>(nameof(Width), out var newWidth) && newWidth != Width;

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Associates the specified symbology with the current instance.
    /// </summary>
    /// <param name="symbology">The symbology to associate. Cannot be null.</param>
    internal void AddSymbology(ISymbology symbology)
    {
        _symbology = symbology;
    }

    /// <summary>
    /// Renders the barcode asynchronously using the current symbology and value settings.
    /// </summary>
    /// <remarks>This method generates a barcode based on the configured symbology and value, updating the SVG
    /// output. Rendering is performed asynchronously if the IsAsync property is set; otherwise, rendering occurs
    /// synchronously within the asynchronous context. The method does not perform rendering if the symbology or value
    /// is not set.</remarks>
    /// <returns>A task that represents the asynchronous render operation.</returns>
    private async Task RenderAsync()
    {
        if (_symbology is null ||
            string.IsNullOrEmpty(Value))
        {
            return;
        }

        var options = new BarcodeRenderingOptions()
        {
            Background = BackgroundOptions,
            Foreground = ForegroundOptions,
            Label = LabelOptions,
            QuietZone = QuietZone,
            View = _surfaceViewOptions
        };

        _renderTarget.SetView(BuildViewPayload());
        surfaceRenderer = _barcodeRendererFactory.Get(_symbology, () => Value);

        if (surfaceRenderer is null)
        {
            return;
        }

        if (IsAsync)
        {
            await surfaceRenderer.RenderAsync(_renderTarget, options);

        }
        else
        {
            surfaceRenderer.Render(_renderTarget, options);
        }

        await _renderTarget.FlushAsync();

        SvgBarcode = _renderTarget.Svg;

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Creates a new instance of the ViewPayload class using the current view's render dimensions.
    /// </summary>
    /// <returns>A ViewPayload object initialized with the current render width and height.</returns>
    private ViewPayload BuildViewPayload()
    {
        return new ViewPayload
        {
            RenderWidth = _surfaceViewOptions.RenderWidth,
            RenderHeight = _surfaceViewOptions.RenderHeight
        };
    }

    /// <summary>
    /// Asynchronously refreshes the component by triggering a re-render operation.
    /// </summary>
    /// <returns>A task that represents the asynchronous refresh operation.</returns>
    internal async Task RefreshAsync()
    {
        await RenderAsync();
    }
}

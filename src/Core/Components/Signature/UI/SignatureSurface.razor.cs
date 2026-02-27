using System.Drawing;
using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interactive signature surface component that enables users to draw, edit, and manage digital
/// signatures within a Blazor application.
/// </summary>
/// <remarks>The SignatureSurface component provides configurable engine and rendering options, supports
/// pointer-based input for drawing, and exposes events for stroke changes. It is designed for integration with Fluent
/// UI Blazor applications and supports extensibility through rendering targets and options. The component manages its
/// own rendering lifecycle and resource cleanup, and is suitable for scenarios requiring user-drawn input, such as
/// signature capture or freehand annotation.</remarks>
public partial class SignatureSurface : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Represents a reference to a JavaScript module used for interop operations.
    /// </summary>
    /// <remarks>This field holds the loaded JavaScript module instance, or null if the module has not been
    /// loaded. It is typically used to invoke JavaScript functions from .NET code using JS interop.</remarks>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the relative path to the JavaScript module used by the SignatureSurface component.
    /// </summary>
    /// <remarks>This constant is used to reference the JavaScript file required for interactivity in the
    /// SignatureSurface component. The path is constructed using the root JavaScript directory defined in
    /// FluentCxConstants.</remarks>
    private const string JavascriptModulePath = FluentCxConstants.JAVASCRIPT_ROOT + "Signature/UI/SignatureSurface.razor.js";

    /// <summary>
    /// Represents the core engine responsible for managing signature strokes, processing input...
    /// </summary>
    private SignatureEngine? _engine;

    /// <summary>
    /// Represents the composite renderer responsible for rendering the signature surface.
    /// </summary>
    private CompositeSignatureRenderer? _renderer;

    /// <summary>
    /// Represents the device pixel ratio (DPI) used for rendering calculations, initialized to a default value of 96.
    /// </summary>
    private double _dpi = 96;

    /// <summary>
    /// Represents the width of the signature surface.
    /// </summary>
    private int _surfaceWidth = 800;

    /// <summary>
    /// Represents the height of the signature surface.
    /// </summary>
    private int _surfaceHeight = 400;

    /// <summary>
    /// Represents a value indicating whether the theme has changed.
    /// </summary>
    private bool _hasThemeChanged;

    /// <summary>
    /// Represents a value indicating whether the rendering options have changed.
    /// </summary>
    private bool _hasRenderingOptionsChanged;

    /// <summary>
    /// Represents a value indicating whether the engine options have changed.
    /// </summary>
    private bool _hasEngineOptionsChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignatureSurface"/> component with the specified library configuration.
    /// </summary>
    /// <param name="configuration">Configuration settings for the Fluent UI Blazor library, used to initialize the component's base class.</param>
    public SignatureSurface(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Options moteur (pression, lissage, gomme, sélection, surface, viewport...).
    /// </summary>
    [Parameter]
    public SignatureEngineOptions EngineOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the theme to use for the surface.
    /// </summary>
    [Parameter]
    public SignatureTheme Theme { get; set; } = SignatureTheme.Default;

    /// <summary>
    /// Options de rendu (stylo, grille, axes, watermark, sélection...).
    /// </summary>
    [Parameter]
    public SignatureRenderingOptions RenderingOptions { get; set; } = new();

    /// <summary>
    /// Callback quand les strokes changent (pour binding externe éventuel).
    /// </summary>
    [Parameter]
    public EventCallback<IReadOnlyList<SignatureStroke>> StrokesChanged { get; set; }

    /// <summary>
    /// Gets or sets the surface render target used for rendering operations.
    /// </summary>
    [Parameter]
    public ISurfaceRenderTarget? Target { get; set; }

    /// <summary>
    /// Gets or sets the surface margin.
    /// </summary>
    [Parameter]
    public Thickness SurfaceMargin { get; set; } = new Thickness(20);

    /// <summary>
    /// Gets or sets a value indicating whether the component should render its content asynchronously.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the component may perform rendering operations
    /// asynchronously, which can improve responsiveness for complex or long-running content. The default behavior is
    /// synchronous rendering.</remarks>
    [Parameter]
    public bool IsAsyncRender { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        _engine = new SignatureEngine(EngineOptions, RenderingOptions);
        _engine.OnChanged += HandleEngineChanged;
        _engine.OnStrokeSegmentAdded += HandleStrokeSegment;
    }

    /// <summary>
    /// Handles changes to rendering options by scheduling a re-render of the component.
    /// </summary>
    /// <param name="sender">The source of the event that triggered the rendering options change.</param>
    /// <param name="e">An object that contains the event data.</param>
    private void OnOptionsChanged(object? sender, EventArgs e)
    {
        InvokeAsync(RenderAsync);
    }

    /// <summary>
    /// Builds a <see cref="ViewPayload"/> object containing the current state of the signature surface.
    /// </summary>
    /// <returns>Returns a <see cref="ViewPayload"/> instance populated with the current surface
    ///  dimensions, rendering parameters, and device pixel ratio.</returns>
    private ViewPayload BuildViewPayload()
    {
        return new ViewPayload
        {
            Width = _surfaceWidth,
            Height = _surfaceHeight,

            OffsetX = SurfaceMargin.Left,
            OffsetY = SurfaceMargin.Top,
            RenderWidth = _surfaceWidth - SurfaceMargin.Horizontal,
            RenderHeight = _surfaceHeight - SurfaceMargin.Vertical,
            Dpi = _dpi,
            Scale = EngineOptions.Viewport.Zoom
        };
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        _module = await JSModule.ImportJavaScriptModuleAsync(JavascriptModulePath);
        await _module.InvokeVoidAsync("FluentUI.Blazor.Community.Signature.RegisterCanvas", Id!, false);

        var size = await _module.InvokeAsync<Size>("FluentUI.Blazor.Community.Signature.GetCanvasSize", Id!);
        _surfaceHeight = size.Height;
        _surfaceWidth = size.Width;
        await InvokeAsync(StateHasChanged);

        Target ??= new HtmlCanvasRenderTarget(Id!, _module);
        _dpi = await _module.InvokeAsync<double>("FluentUI.Blazor.Community.Signature.GetDPI");

        EngineOptions.Surface.Width = _surfaceWidth;
        EngineOptions.Surface.Height = _surfaceHeight;

        var view = BuildViewPayload();
        Target.SetView(view);

        await _module!.InvokeVoidAsync("FluentUI.Blazor.Community.Signature.ResizeOffscreens", Id!, view.RenderWidth, view.RenderHeight);

        _renderer = new CompositeSignatureRenderer(
            surfaceRenderers: [
                new BackgroundRenderer(),
                new GridRenderer(),
                new AxesRenderer(),
                new WatermarkRenderer(),
                 new SelectionStrokeRenderer(
                    () => _engine!.CurrentTool,
                    () => _engine!.SelectionManager.SelectedStrokes),
                 new HoverStrokeRenderer( () => _engine!.HoverStroke)
             ],

            strokeRenderers: [
                new StrokeLayerRenderer(),
                new DynamicStrokeRenderer(_engine!.StrokeManager),
                new DebugRenderer()
            ]);

        await RenderAsync();
    }

    /// <summary>
    /// Handles changes to the engine by invoking the strokes changed event and triggering a re-render asynchronously.
    /// </summary>
    /// <remarks>This method should be attached to engine change events to ensure that any updates to the
    /// engine's state are propagated to consumers and the UI is refreshed accordingly.</remarks>
    /// <param name="sender">The source of the event. This parameter is typically the engine instance that raised the event.</param>
    /// <param name="e">An object that contains the event data.</param>
    private void HandleEngineChanged(object? sender, EventArgs e)
    {
        if (_engine is null)
        {
            return;
        }

        InvokeAsync(async () =>
        {
            if (StrokesChanged.HasDelegate)
            {
                await StrokesChanged.InvokeAsync(_engine.Strokes);
            }

            await RenderAsync();
        });
    }

    /// <summary>
    /// Performs an asynchronous rendering operation using the configured rendering engine, target, and renderer.
    /// </summary>
    /// <remarks>This method sets up the target view and invokes either synchronous or asynchronous rendering
    /// based on the current configuration. It also ensures that all rendering layers are flushed after rendering
    /// completes. The method does nothing if the rendering engine, target, or renderer is not initialized.</remarks>
    /// <returns>A task that represents the asynchronous rendering operation.</returns>
    private async Task RenderAsync()
    {
        if (_engine is null ||
            Target is null ||
            _renderer is null)
        {
            return;
        }

        Target.SetView(BuildViewPayload());

        if (!IsAsyncRender)
        {
            _renderer.Render(Target, _engine.Strokes, RenderingOptions);
        }
        else
        {
            await _renderer.RenderAsync(Target, _engine.Strokes, RenderingOptions);
        }

        await Target.FlushAsync();
    }

    /// <summary>
    /// Handles the pointer down event by initiating a new stroke if the engine is available.
    /// </summary>
    /// <param name="e">The pointer event arguments containing information about the pointer down event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandlePointerDown(PointerEventArgs e)
    {
        if (_engine is null)
        {
            return;
        }

        var sample = await ToPointerSampleAsync(e);
        _engine.BeginStroke(sample);
    }

    /// <summary>
    /// Handles pointer move events to update the current stroke based on the pointer's position.
    /// </summary>
    /// <remarks>This method processes pointer movement only when a pointer is actively pressed and the stroke
    /// engine is available.</remarks>
    /// <param name="e">The pointer event data containing information about the pointer's current position and state.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandlePointerMove(PointerEventArgs e)
    {
        if (_engine is null)
        {
            return;
        }

        var sample = await ToPointerSampleAsync(e);
        _engine.UpdateStroke(sample);
    }

    /// <summary>
    /// Handles the pointer up event to complete the current stroke if a pointer interaction is in progress.
    /// </summary>
    /// <remarks>This method should be called when the pointer is released to ensure that the stroke is
    /// properly finalized. If no pointer interaction is active or the engine is not initialized, the method returns
    /// immediately.</remarks>
    /// <param name="e">The event data associated with the pointer up action.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task HandlePointerUp(PointerEventArgs e)
    {
        if (_engine is null)
        {
            return;
        }

        var sample = await ToPointerSampleAsync(e);
        _engine.EndStroke(sample);
    }

    /// <summary>
    /// Asynchronously converts the specified pointer event arguments to a new instance of the PointerSample class,
    /// capturing the pointer's position and state at the time of the event.
    /// </summary>
    /// <param name="e">The pointer event arguments containing information about the pointer's position, pressure, and modifier keys.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a PointerSample object with the
    /// pointer's position, pressure, modifier key states, and a timestamp.</returns>
    private async Task<PointerSample> ToPointerSampleAsync(PointerEventArgs e)
    {
        var pos = await _module!.InvokeAsync<PointF>("FluentUI.Blazor.Community.Signature.GetPointerPosition", Id!, e.ClientX, e.ClientY, Target!.View);

        return new PointerSample(
            X: pos.X,
            Y: pos.Y,
            Pressure: e.Pressure <= 0 ? 0.5 : e.Pressure,
            CtrlKey: e.CtrlKey,
            ShiftKey: e.ShiftKey,
            AltKey: e.AltKey,
            Timestamp: DateTimeOffset.UtcNow.Ticks
        );
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        await base.DisposeAsync(jsModule);

        if (_module is not null)
        {
            await _module.DisposeAsync();
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_engine is not null)
        {
            _engine.OnChanged -= HandleEngineChanged;
            _engine.OnStrokeSegmentAdded -= HandleStrokeSegment;
            _engine.Dispose();
        }

        RenderingOptions.OptionsChanged -= OnOptionsChanged;
        EngineOptions.OptionsChanged -= OnOptionsChanged;

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HandleStrokeSegment(object? sender, StrokeSegmentEventArgs e)
    {
        if (_module is null || Target is null)
        {
            return;
        }

        var view = Target.View;
        var payload = new
        {
            p1 = new { x = e.P1.X, y = e.P1.Y, width = e.P1.Width },
            p2 = new { x = e.P2.X, y = e.P2.Y, width = e.P2.Width },
            pen = new
            {
                color = e.Style.Rendering.Color,
                opacity = e.Style.Rendering.Opacity,
                lineCap = e.Style.Rendering.LineCap,
                lineJoin = e.Style.Rendering.LineJoin,
                dashArray = SignatureMathUtils.ToDashArray(e.Style.Rendering.DashArray),
                shadow = new
                {
                    enabled = e.Style.Rendering.Shadow.Enabled,
                    color = e.Style.Rendering.Shadow.Color,
                    opacity = e.Style.Rendering.Shadow.Opacity,
                    blur = e.Style.Rendering.Shadow.Blur,
                    offsetX = e.Style.Rendering.Shadow.OffsetX,
                    offsetY = e.Style.Rendering.Shadow.OffsetY
                },
            },
            blendMode = PayloadFactory.MapBlendMode(e.Style.Rendering.BlendMode)
        };

        InvokeAsync(async () =>
        {
            await Target.DrawStrokeSegmentAsync(
             payload,
             view);
        });
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasThemeChanged = parameters.TryGetValue(nameof(Theme), out SignatureTheme newTheme) && newTheme != Theme;
        _hasRenderingOptionsChanged = parameters.TryGetValue<SignatureRenderingOptions>(nameof(RenderingOptions), out var newOptions) && newOptions != RenderingOptions;
        _hasEngineOptionsChanged = parameters.TryGetValue<SignatureEngineOptions>(nameof(EngineOptions), out var newEngineOptions) && newEngineOptions != EngineOptions;

        if (_hasRenderingOptionsChanged)
        {
            RenderingOptions?.OptionsChanged -= OnOptionsChanged;
            newOptions?.OptionsChanged += OnOptionsChanged;
        }

        if (_hasEngineOptionsChanged)
        {
            EngineOptions?.OptionsChanged -= OnOptionsChanged;
            newEngineOptions?.OptionsChanged += OnOptionsChanged;
        }

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (_hasThemeChanged)
        {
            _hasThemeChanged = false;
            SignatureThemeApplier.Apply(SignatureThemeFactory.Create(Theme), RenderingOptions);
            await RenderAsync();
        }
    }
}


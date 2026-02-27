using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rendering target for an HTML canvas element, providing methods to set various visual elements such as the view, background, grid, axes, stroke layers, dynamic strokes, watermark, selection, and debug information. This class implements the ISurfaceRenderTarget interface and serves as a concrete implementation for rendering signature or drawing surfaces onto an HTML canvas using JavaScript interop. The constructor takes a canvas ID and a reference to a JavaScript module, which are used to interact with the canvas element in the browser for rendering operations.
/// </summary>
public sealed class HtmlCanvasRenderTarget : ISurfaceRenderTarget
{
    private readonly IJSObjectReference _module;
    private readonly string _canvasId;
    private readonly StaticCanvasLayer _staticBackLayer = new();
    private readonly DynamicCanvasLayer _dynamicLayer = new();
    private readonly StaticCanvasLayer _staticFrontLayer = new();
    private readonly DynamicCanvasLayer _debugLayer = new();
    private readonly StaticCanvasLayer _watermarkLayer = new();
    private readonly StaticCanvasLayer _hoverLayer = new();

    private ViewPayload _view = new();

    /// <summary>
    /// Initializes a new instance of the HtmlCanvasRenderTarget class for rendering to a specified HTML canvas element
    /// using the provided JavaScript module.
    /// </summary>
    /// <param name="canvasId">The ID of the HTML canvas element to be used as the rendering target. Cannot be null or empty.</param>
    /// <param name="module">A reference to the JavaScript module that provides rendering functionality. Cannot be null.</param>
    public HtmlCanvasRenderTarget(string canvasId, IJSObjectReference module)
    {
        _module = module;
        _canvasId = canvasId;
    }

    /// <inheritdoc />
    public IStaticSurfaceRender StaticBackLayer => _staticBackLayer;

    /// <inheritdoc />
    public IDynamicSurfaceRender DynamicLayer => _dynamicLayer;

    /// <inheritdoc />
    public IStaticSurfaceRender StaticFrontLayer => _staticFrontLayer;

    /// <inheritdoc />
    public IDynamicSurfaceRender DebugLayer => _debugLayer;

    /// <inheritdoc />
    public IStaticSurfaceRender WaterMark => _watermarkLayer;

    /// <inheritdoc />
    public IStaticSurfaceRender Hover => _hoverLayer;

    /// <inheritdoc />
    public ViewPayload View => _view;

    /// <inheritdoc />
    public async ValueTask DrawStrokeSegmentAsync(object payload, ViewPayload view)
    {
        await _module.InvokeVoidAsync(
            "FluentUI.Blazor.Community.Signature.DrawStrokeSegment",
            _canvasId,
            payload,
            view);
    }

    /// <inheritdoc />
    public async ValueTask FlushAsync()
    {
        var staticBackDirty = _staticBackLayer.Frame.Dirty.Any;
        var staticFrontDirty = _staticFrontLayer.Frame.Dirty.Any;
        var dynamicDirty = _dynamicLayer.Frame.Dirty.Any;
        var debugDirty = _debugLayer.Frame.Dirty.Any;
        var watermarkDirty = _watermarkLayer.Frame.Dirty.Any;
        var hoverDirty = _hoverLayer.Frame.Dirty.Any;

        if (!staticBackDirty &&
            !staticFrontDirty &&
            !dynamicDirty &&
            !debugDirty &&
            !hoverDirty &&
            !watermarkDirty)
        {
            return;
        }

        var payload = new FramePayload
        {
            StaticBack = _staticBackLayer.Frame,
            StaticFront = _staticFrontLayer.Frame,
            Dynamic = _dynamicLayer.Frame,
            Debug = _debugLayer.Frame,
            Watermark = _watermarkLayer.Frame,
            Hover = _hoverLayer.Frame
        };

        await _module.InvokeVoidAsync(
            "FluentUI.Blazor.Community.Signature.RenderFrame",
            _canvasId,
            payload);

        _staticBackLayer.Frame.Dirty.Reset();
        _staticFrontLayer.Frame.Dirty.Reset();
        _dynamicLayer.Frame.Dirty.Reset();
        _debugLayer.Frame.Dirty.Reset();
        _watermarkLayer.Frame.Dirty.Reset();
        _hoverLayer.Frame.Dirty.Reset();
    }

    /// <inheritdoc />
    public object? GetNativeHandle()
    {
        return _canvasId;
    }

    /// <inheritdoc />
    public void SetView(ViewPayload view)
    {
        _view = view;
        _staticBackLayer.SetView(view);
        _dynamicLayer.SetView(view);
        _staticFrontLayer.SetView(view);
        _debugLayer.SetView(view);
        _hoverLayer.SetView(view);
        _watermarkLayer.SetView(view);
    }
}

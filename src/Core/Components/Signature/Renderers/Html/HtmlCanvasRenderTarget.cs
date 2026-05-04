using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a rendering target for an HTML canvas element, providing methods to set various visual elements such as the view, background, grid, axes, stroke layers, dynamic strokes, watermark, selection, and debug information. This class implements the ISurfaceRenderTarget interface and serves as a concrete implementation for rendering signature or drawing surfaces onto an HTML canvas using JavaScript interop. The constructor takes a canvas ID and a reference to a JavaScript module, which are used to interact with the canvas element in the browser for rendering operations.
/// </summary>
public sealed class HtmlCanvasRenderTarget : ISignatureSurfaceRenderTarget
{
    private readonly IJSObjectReference _module;
    private readonly string _canvasId;
    private ViewPayload _view = new();
    private readonly FrameBuilder _frameBuilder = new();
    private readonly List<ILayer> _layers = new();

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
    public ViewPayload View => _view;

    /// <inheritdoc />
    public void SetView(ViewPayload view) => _view = view;

    /// <inheritdoc />
    public ISurfaceRenderTarget AddLayer(ILayer layer)
    {
        _layers.Add(layer);

        return this;
    }

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
        if (_layers.Count == 0)
        {
            return;
        }

        _layers.Sort((a, b) =>
        {
            var order = a.Order.CompareTo(b.Order);

            if (order != 0)
            {
                return order;
            }

            return a.Priority.CompareTo(b.Priority);
        });

        foreach (var layer in _layers)
        {
            _frameBuilder.Set(layer.Key, layer.LayerPayload);
        }

        var frame = new CanvasFramePayload
        {
            View = _view,
            Layers = _frameBuilder.Payloads
        };

        await _module.InvokeVoidAsync(
            "FluentUI.Blazor.Community.Signature.RenderCanvasFrame",
            _canvasId,
            frame);

        _layers.Clear();
        _frameBuilder.Clear();
    }

    /// <inheritdoc />
    public object? GetNativeHandle()
    {
        return _canvasId;
    }
}

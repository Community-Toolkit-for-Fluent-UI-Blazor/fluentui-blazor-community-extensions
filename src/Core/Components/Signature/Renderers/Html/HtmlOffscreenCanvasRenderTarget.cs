using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a render target for drawing to an offscreen HTML canvas element using a JavaScript module for rendering
/// operations.
/// </summary>
/// <remarks>This class is intended for use in scenarios where rendering to an offscreen canvas is required, such
/// as advanced graphics or layered drawing in Blazor applications. It manages the lifecycle of the canvas, including
/// initialization, resizing, and rendering of multiple layers. The render target interacts with JavaScript via the
/// provided module to perform all canvas operations. Instances of this class are not thread-safe.</remarks>
internal sealed class HtmlOffscreenCanvasRenderTarget : ISurfaceRenderTarget
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
    /// <param name="module">A reference to the JavaScript module that provides rendering functionality. Cannot be null.</param>
    public HtmlOffscreenCanvasRenderTarget(
        IJSObjectReference module)
    {
        _module = module;
        _canvasId = "offscreen_" + Guid.NewGuid().ToString("N");
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

    /// <summary>
    /// Initializes the canvas and resizes its offscreen buffers to the specified dimensions asynchronously.
    /// </summary>
    /// <remarks>Call this method before performing drawing operations to ensure the canvas is properly
    /// registered and sized.</remarks>
    /// <param name="width">The width, in pixels, to set for the canvas and its offscreen buffers. Must be a positive integer.</param>
    /// <param name="height">The height, in pixels, to set for the canvas and its offscreen buffers. Must be a positive integer.</param>
    /// <returns>A task that represents the asynchronous initialization operation.</returns>
    public async ValueTask InitializeAsync(int width, int height)
    {
        await _module.InvokeVoidAsync(
            "FluentUI.Blazor.Community.Signature.RegisterCanvas",
            _canvasId,
            true,
            width,
            height);

        await _module.InvokeVoidAsync(
            "FluentUI.Blazor.Community.Signature.ResizeOffscreens",
            _canvasId,
            width,
            height);
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

using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a render target for composing and generating SVG markup from multiple layers using the current view
/// settings.
/// </summary>
/// <remarks>This class manages a collection of render layers and produces a single SVG document representing
/// their combined output. After adding layers and setting the view, call FlushAsync to generate the final SVG markup,
/// which is then available via the Svg property. The class is not thread-safe; concurrent access should be synchronized
/// externally if needed.</remarks>
public abstract class SvgRenderTarget : ISurfaceRenderTarget
{
    /// <summary>
    /// Represents the collection of layers managed by the render target.
    /// </summary>
    private readonly List<ILayer> _layers = [];

    /// <summary>
    /// Represents the current view payload used internally by the class.
    /// </summary>
    private ViewPayload _view = new();

    /// <summary>
    /// Gets the SVG markup content as a Blazor MarkupString.
    /// </summary>
    /// <remarks>Use this property to retrieve the raw SVG markup for rendering within a Blazor component. The
    /// value is suitable for direct use in markup rendering scenarios where SVG content is required.</remarks>
    public MarkupString Svg { get; private set; }

    /// <inheritdoc />
    public ViewPayload View => _view;

    /// <inheritdoc />
    public void SetView(ViewPayload view) => _view = view;

    /// <summary>
    /// Gets a value indicating whether any layers are present.
    /// </summary>
    protected bool HasLayers => _layers.Count > 0;

    /// <inheritdoc />
    public ISurfaceRenderTarget AddLayer(ILayer layer)
    {
        ValidateLayer(layer);
        _layers.Add(layer);

        return this;
    }

    /// <summary>
    /// Performs validation on the specified layer to ensure it meets required criteria.
    /// </summary>
    /// <remarks>Implementations should throw an appropriate exception if the layer does not satisfy
    /// validation requirements.</remarks>
    /// <param name="layer">The layer to validate. Cannot be null.</param>
    protected abstract void ValidateLayer(ILayer layer);

    /// <summary>
    /// Renders the specified layer using the provided SVG builder.
    /// </summary>
    /// <remarks>Implementations should use the builder to generate SVG elements that visually represent the
    /// given layer. This method is intended to be overridden by derived classes to define custom rendering logic for
    /// different layer types.</remarks>
    /// <param name="builder">The SVG builder used to construct the graphical representation of the layer.</param>
    /// <param name="layer">The layer to render. Provides the data and properties required for rendering.</param>
    protected abstract void RenderLayer(SvgBuilder builder, ILayer layer);

    /// <summary>
    /// Calculates the width and height of the view box required to display the specified layer.
    /// </summary>
    /// <param name="layer">The layer for which to compute the view box dimensions. Cannot be null.</param>
    /// <returns>A tuple containing the width and height of the view box that encompasses the specified layer.</returns>
    protected abstract (double width, double height) ComputeViewBox(ILayer layer);

    /// <inheritdoc />
    public ValueTask FlushAsync()
    {
        if (_layers.Count == 0)
        {
            Svg = new MarkupString("");
            return ValueTask.CompletedTask;
        }

        _layers.Sort((a, b) =>
        {
            var order = a.Order.CompareTo(b.Order);
            return order != 0 ? order : a.Priority.CompareTo(b.Priority);
        });

        var root = new SvgBuilder();

        var (logicalWidth, logicalHeight) = ComputeViewBox(_layers[0]);
        var renderedWidth = _view.RenderWidth > 0 ? _view.RenderWidth : logicalWidth;
        var renderedHeight = _view.RenderHeight > 0 ? _view.RenderHeight : logicalHeight;

        root
            .RenderedWidth(renderedWidth)
            .RenderedHeight(renderedHeight)
            .ViewBox(0, 0, logicalWidth, logicalHeight)
            .ShapeRendering("crispEdges");

        foreach (var layer in _layers)
        {
            RenderLayer(root, layer);
        }

        Svg = new MarkupString(root.Build());
        _layers.Clear();

        return ValueTask.CompletedTask;
    }

    /// <inheritdoc />
    public object? GetNativeHandle() => null;
}

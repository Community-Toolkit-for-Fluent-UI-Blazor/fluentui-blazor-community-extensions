using FluentUI.Blazor.Community.Components.Enums;
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
/// <param name="id">Optional identifier for the SVG element, which can be used for targeting with CSS or JavaScript. If not provided, no id attribute will be set on the generated SVG element.</param>
public abstract class SvgRenderTarget(string? id = null) : ISurfaceRenderTarget
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
    private MarkupString _svg;

    /// <inheritdoc />
    public ViewPayload View => _view;

    /// <summary>
    /// Represents the SVG shape-rendering property that controls how shapes are rendered in the generated SVG output.
    /// </summary>
    public SvgShapeRendering ShapeRendering { get; protected set; } = SvgShapeRendering.CrispEdges;

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
    /// <param name="layers">The layer for which to compute the view box dimensions. Cannot be null.</param>
    /// <returns>A tuple containing the width and height of the view box that encompasses the specified layer.</returns>
    protected abstract (double width, double height) ComputeViewBox(IEnumerable<ILayer> layers);

    /// <inheritdoc />
    public ValueTask FlushAsync()
    {
        if (_layers.Count == 0)
        {
            _svg = new MarkupString("");
            return ValueTask.CompletedTask;
        }

        _layers.Sort((a, b) =>
        {
            var order = a.Order.CompareTo(b.Order);
            return order != 0 ? order : a.Priority.CompareTo(b.Priority);
        });

        BeforeRender();

        var root = new SvgBuilder();

        var (logicalWidth, logicalHeight) = ComputeViewBox(_layers);
        var renderedWidth = _view.RenderWidth > 0 ? _view.RenderWidth : logicalWidth;
        var renderedHeight = _view.RenderHeight > 0 ? _view.RenderHeight : logicalHeight;

        root
            .RenderedWidth(renderedWidth)
            .RenderedHeight(renderedHeight)
            .ViewBox(0, 0, logicalWidth, logicalHeight)
            .ShapeRendering(ShapeRendering)
            .WithId(id)
            .WithDataId(id);

        foreach (var layer in _layers)
        {
            RenderLayer(root, layer);
        }

        _svg = new MarkupString(root.Build());
        _layers.Clear();

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Operations to perform before rendering the layers.
    /// </summary>
    protected virtual void BeforeRender()
    {
    }

    /// <inheritdoc />
    public object? GetNativeHandle() => _svg;
}

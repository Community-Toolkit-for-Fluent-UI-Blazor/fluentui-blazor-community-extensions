namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an eraser that combines both pixel-based and stroke-based erasing strategies for digital ink, selecting the
/// appropriate method based on the pointer input and configured options.
/// </summary>
/// <remarks>The HybridEraser delegates erasing operations to either a pixel eraser or a stroke eraser depending
/// on the proximity of the pointer sample to existing strokes and the configured tolerance. This allows for flexible
/// erasing behavior that adapts to user intent, supporting both fine-grained and whole-stroke removal. The eraser is
/// typically used in digital ink or signature applications where both granular and broad erasing actions are
/// needed.</remarks>
public sealed class HybridEraser : IEraser
{
    /// <summary>
    /// Provides access to the pixel eraser used for manipulating pixel data.
    /// </summary>
    private readonly PixelEraser _pixel;

    /// <summary>
    /// Represents the stroke eraser used for erasing strokes.
    /// </summary>
    private readonly StrokeEraser _stroke;

    /// <summary>
    /// Provides the configuration options used to control signature erasure behavior.
    /// </summary>
    private readonly SignatureEraserOptions _options;

    /// <summary>
    /// Manages stroke-related operations for the associated component.
    /// </summary>
    private readonly StrokeManager _strokeManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrokeEraser"/> class with the specified stroke manager, history manager, and eraser options.
    /// </summary>
    /// <param name="pixel">Pixel eraser that provides functionality for manipulating pixel data. Cannot be null.</param>
    /// <param name="stroke">Stroke eraser that provides functionality for erasing strokes. Cannot be null.</param>
    /// <param name="strokeManager">Stroke manager that provides access to the collection of signature strokes. Cannot be null.</param>
    /// <param name="options">Options used to configure the behavior of the pixel eraser, such as radius and other settings. Cannot be null.</param>
    public HybridEraser(
        PixelEraser pixel,
        StrokeEraser stroke,
        StrokeManager strokeManager,
        SignatureEraserOptions options)
    {
        _pixel = pixel;
        _stroke = stroke;
        _options = options;
        _strokeManager = strokeManager;
    }

    /// <inheritdoc />
    public void Begin(PointerSample sample) => Erase(sample);

    /// <inheritdoc />
    public void Update(PointerSample sample) => Erase(sample);

    /// <inheritdoc />
    public void End(PointerSample sample) => Erase(sample);

    /// <summary>
    /// Attempts to erase a portion of the drawing at the specified pointer sample location, updating either the current
    /// stroke or the pixel data as appropriate.
    /// </summary>
    /// <remarks>The erase operation affects either the current stroke or the overall pixel data, depending on
    /// the proximity of the pointer sample to existing stroke points. The radius and tolerance options determine the
    /// sensitivity of the erase action.</remarks>
    /// <param name="sample">The pointer sample that specifies the location and context for the erase operation.</param>
    private void Erase(PointerSample sample)
    {
        var radiusSq = _options.Radius * _options.Radius;

        foreach (var stroke in _strokeManager.Strokes)
        {
            foreach (var p in stroke.Points)
            {
                if (SignatureMathUtils.DistanceSquared(p.X, p.Y, sample.X, sample.Y) <= radiusSq * _options.Tolerance * stroke.Style.Engine.Pen.MaxWidth)
                {
                    _stroke.Update(sample);
                    return;
                }
            }
        }

        _pixel.Update(sample);
    }
}

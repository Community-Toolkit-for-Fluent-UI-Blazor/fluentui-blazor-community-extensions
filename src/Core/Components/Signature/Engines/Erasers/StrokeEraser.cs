namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to erase signature strokes by detecting intersections with a specified eraser radius.
/// </summary>
/// <remarks>This class is typically used in digital signature or drawing applications to remove strokes that
/// intersect with the eraser area as defined by user input. It maintains stroke history to support undo operations and
/// relies on configurable eraser options for behavior customization. Thread safety is not guaranteed; use from a single
/// UI thread.</remarks>
public sealed class StrokeEraser : IEraser
{
    /// <summary>
    /// Manages the collection of strokes for the associated component.
    /// </summary>
    private readonly StrokeManager _strokes;

    /// <summary>
    /// Provides access to the history management functionality for the containing class.
    /// </summary>
    private readonly HistoryManager _history;

    /// <summary>
    /// Provides the configuration options used to control signature erasure behavior.
    /// </summary>
    private readonly SignatureEraserOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="StrokeEraser"/> class with the specified stroke manager, history manager, and eraser options.
    /// </summary>
    /// <param name="strokes">Stroke manager that provides access to the collection of signature strokes. Cannot be null.</param>
    /// <param name="history">History manager that provides access to the undo/redo functionality for the signature component. Cannot be null.</param>
    /// <param name="options">Options used to configure the behavior of the pixel eraser, such as radius and other settings. Cannot be null.</param>
    public StrokeEraser(
        StrokeManager strokes,
        HistoryManager history,
        SignatureEraserOptions options)
    {
        _strokes = strokes;
        _history = history;
        _options = options;
    }

    /// <inheritdoc/>
    public void Begin(PointerSample sample) => Erase(sample);

    /// <inheritdoc/>
    public void Update(PointerSample sample) => Erase(sample);

    /// <inheritdoc />
    public void End(PointerSample sample) => Erase(sample);

    /// <summary>
    /// Removes any signature strokes that intersect with the specified pointer sample, simulating an eraser action at
    /// the given location.
    /// </summary>
    /// <remarks>This method identifies and removes all strokes that intersect with the eraser's area, as
    /// defined by the current eraser radius. The operation is recorded in the history for undo/redo support.</remarks>
    /// <param name="sample">The pointer sample representing the location and context for the erasing operation.</param>
    private void Erase(PointerSample sample)
    {
        var radiusSq = _options.Radius * _options.Radius;

        var toRemove = new List<SignatureStroke>();

        foreach (var stroke in _strokes.Strokes)
        {
            var pts = stroke.Points;

            for (var i = 0; i < pts.Count - 1; i++)
            {
                if (SurfaceMathUtils.SegmentIntersectsCircle(pts[i], pts[i + 1], sample.X, sample.Y, radiusSq))
                {
                    toRemove.Add(stroke);
                    break;
                }
            }
        }

        if (toRemove.Count > 0)
        {
            _history.Execute(new EraseStrokeAction(_strokes, toRemove, []));
        }
    }
}

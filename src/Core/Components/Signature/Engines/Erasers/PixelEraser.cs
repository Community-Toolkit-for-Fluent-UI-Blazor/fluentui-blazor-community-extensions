namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides pixel-level erasing functionality for signature strokes, allowing portions of strokes to be removed or
/// split based on pointer input and configurable options.
/// </summary>
/// <remarks>The pixel eraser operates by detecting intersections between the erase area, defined by the pointer
/// sample and configured radius, and existing signature strokes. Strokes that intersect the erase area are either
/// removed or split into new segments, and all changes are recorded in the history manager to support undo and redo
/// operations. This class is intended for use with signature or drawing components that require fine-grained erasing
/// capabilities.</remarks>
public sealed class PixelEraser : IEraser
{
    /// <summary>
    /// Manages the collection of strokes for the associated component.
    /// </summary>
    private readonly StrokeManager _strokes;

    /// <summary>
    /// Provides access to the history management functionality for the current instance.
    /// </summary>
    private readonly HistoryManager _history;

    /// <summary>
    /// Provides the configuration options used to control signature erasure behavior.
    /// </summary>
    private readonly SignatureEraserOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="PixelEraser"/> class with the specified stroke manager, history manager, and eraser options.
    /// </summary>
    /// <param name="strokes">Stroke manager that provides access to the collection of signature strokes. Cannot be null.</param>
    /// <param name="history">History manager that provides access to the undo/redo functionality for the signature component. Cannot be null.</param>
    /// <param name="options">Options used to configure the behavior of the pixel eraser, such as radius and other settings. Cannot be null.</param>
    public PixelEraser(StrokeManager strokes, HistoryManager history, SignatureEraserOptions options)
    {
        _strokes = strokes;
        _history = history;
        _options = options;
    }

    /// <inheritdoc/>
    public void Begin(PointerSample sample)
    {
        EraseAt(sample);
    }

    /// <inheritdoc/>
    public void Update(PointerSample sample)
    {
        EraseAt(sample);
    }

    /// <inheritdoc/>
    public void End(PointerSample sample)
    {
        EraseAt(sample);
    }

    /// <summary>
    /// Removes or splits signature strokes that intersect with the specified pointer sample, effectively erasing
    /// portions of strokes within the defined radius.
    /// </summary>
    /// <remarks>This method modifies the collection of signature strokes by removing or splitting any strokes
    /// that intersect the erase area defined by the sample's position and the configured radius. Strokes fully within
    /// the erase area are removed, while intersecting strokes are split into new segments as needed. The operation is
    /// recorded in the history for undo support.</remarks>
    /// <param name="sample">The pointer sample indicating the location and context for the erase operation. The erase action is applied
    /// around the coordinates of this sample.</param>
    private void EraseAt(PointerSample sample)
    {
        var radius = _options.Radius;
        var radiusSq = radius * radius;

        var toRemove = new List<SignatureStroke>();
        var toAdd = new List<SignatureStroke>();

        foreach (var stroke in _strokes.Strokes)
        {
            var newSegments = new List<List<SignaturePoint>>();
            var current = new List<SignaturePoint>();

            var pts = stroke.Points;

            for (var i = 0; i < pts.Count - 1; i++)
            {
                var p1 = pts[i];
                var p2 = pts[i + 1];

                var intersects = SurfaceMathUtils.SegmentIntersectsCircle(p1, p2, sample.X, sample.Y, radiusSq);

                if (!intersects)
                {
                    if (current.Count == 0)
                    {
                        current.Add(p1);
                    }

                    current.Add(p2);
                }
                else
                {
                    if (current.Count > 1)
                    {
                        newSegments.Add(current);
                    }

                    current = [];
                }
            }

            if (current.Count > 1)
            {
                newSegments.Add(current);
            }

            if (newSegments.Count == 0)
            {
                toRemove.Add(stroke);
            }
            else if (!(newSegments.Count == 1 && newSegments[0].Count == stroke.Points.Count))
            {
                toRemove.Add(stroke);

                foreach (var seg in newSegments)
                {
                    toAdd.Add(new SignatureStroke(stroke.Style.Clone(), seg));
                }
            }
        }

        if (toRemove.Count > 0 || toAdd.Count > 0)
        {
            _history.Execute(new EraseStrokeAction(_strokes, toRemove, toAdd));
        }
    }
}


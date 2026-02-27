namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes a collection of signature strokes by applying eraser logic based on the specified eraser options and
/// eraser stroke. Supports both pixel-level and stroke-level erasing modes.
/// </summary>
/// <remarks>The eraser behavior is determined by the provided options, allowing for either partial removal of
/// stroke segments (pixel mode) or complete removal of strokes that meet the erasure criteria (stroke mode). This
/// processor is typically used to implement erasing functionality in digital ink or signature applications.</remarks>
public sealed class EraserStrokeProcessor : IStrokeProcessor
{
    /// <summary>
    /// Represents the options for the eraser.
    /// </summary>
    private readonly SignatureEraserOptions _eraser;

    /// <summary>
    /// Represents the stroke that defines the eraser's path and shape.
    /// </summary>
    private readonly SignatureStroke _eraserStroke;

    /// <summary>
    /// Initializes a new instance of the EraserStrokeProcessor class with the specified eraser options and eraser
    /// stroke.
    /// </summary>
    /// <param name="eraser">The eraser options that define how the erasing operation should be performed.</param>
    /// <param name="eraserStroke">The stroke data representing the path or shape of the eraser.</param>
    public EraserStrokeProcessor(SignatureEraserOptions eraser, SignatureStroke eraserStroke)
    {
        _eraser = eraser;
        _eraserStroke = eraserStroke;
    }

    /// <inheritdoc />
    public IEnumerable<SignatureStroke> Process(
        IEnumerable<SignatureStroke> strokes,
        SignatureEngineOptions options)
    {
        foreach (var stroke in strokes)
        {
            if (_eraser.Mode == EraserMode.Pixel)
            {
                foreach (var s in ErasePixel(stroke))
                {
                    yield return s;
                }
            }
            else
            {
                if (!IsStrokeErased(stroke))
                {
                    yield return stroke;
                }
            }
        }
    }

    /// <summary>
    /// Splits a signature stroke into one or more new strokes by removing points that fall within the eraser area.
    /// </summary>
    /// <remarks>Each returned stroke preserves the style of the original stroke. Only segments with more than
    /// one point are included in the result.</remarks>
    /// <param name="stroke">The signature stroke to process for erasing. Cannot be null.</param>
    /// <returns>An enumerable collection of new signature strokes, each representing a continuous segment of the original stroke
    /// that does not intersect the eraser area. The collection is empty if all points are erased.</returns>
    private IEnumerable<SignatureStroke> ErasePixel(SignatureStroke stroke)
    {
        var segments = new List<List<SignaturePoint>>();
        var current = new List<SignaturePoint>();

        foreach (var p in stroke.Points)
        {
            var erased = IsPointInsideEraser(p);

            if (!erased)
            {
                current.Add(p);
            }
            else
            {
                if (current.Count > 1)
                {
                    segments.Add(current);
                }

                current = [];
            }
        }

        if (current.Count > 1)
        {
            segments.Add(current);
        }

        foreach (var seg in segments)
        {
            yield return new SignatureStroke(stroke.Style.Clone(), seg);
        }
    }

    /// <summary>
    /// Determines whether the specified stroke is considered erased based on the eraser's tolerance and the number of
    /// stroke points within the eraser area.
    /// </summary>
    /// <remarks>A stroke is considered erased if the ratio of its points inside the eraser area is greater
    /// than or equal to the eraser's tolerance value. This method does not modify the stroke or the eraser
    /// state.</remarks>
    /// <param name="stroke">The stroke to evaluate for erasure. Must not be null and should contain at least one point.</param>
    /// <returns>true if the proportion of points within the stroke that are inside the eraser area meets or exceeds the eraser's
    /// tolerance; otherwise, false.</returns>
    private bool IsStrokeErased(SignatureStroke stroke)
    {
        if (stroke.Points.Count == 0)
        {
            return false;
        }

        var erasedCount = 0;

        foreach (var p in stroke.Points)
        {
            if (IsPointInsideEraser(p))
            {
                erasedCount++;
            }
        }

        var ratio = (double)erasedCount / stroke.Points.Count;

        return ratio >= _eraser.Tolerance;
    }

    /// <summary>
    /// Determines whether the specified point is within the area covered by the eraser, taking into account the
    /// eraser's shape, radius, and soft edges.
    /// </summary>
    /// <remarks>The method considers both circular and square eraser shapes, as well as any additional area
    /// provided by soft edges, when determining if the point is inside the eraser.</remarks>
    /// <param name="p">The point to test for inclusion within the eraser's area.</param>
    /// <returns>true if the point is inside the eraser's area; otherwise, false.</returns>
    private bool IsPointInsideEraser(SignaturePoint p)
    {
        var baseRadius = _eraser.Radius;

        foreach (var e in _eraserStroke.Points)
        {
            var dx = p.X - e.X;
            var dy = p.Y - e.Y;

            if (_eraser.Shape == EraserShape.Circle)
            {
                var dist = Math.Sqrt(dx * dx + dy * dy);
                var radius = baseRadius;

                if (_eraser.SoftEdges)
                {
                    radius += _eraser.SoftEdgeRadius;
                }

                if (dist <= radius)
                {
                    return true;
                }
            }
            else
            {
                var half = baseRadius;
                var ax = Math.Abs(dx);
                var ay = Math.Abs(dy);

                var extra = _eraser.SoftEdges ? _eraser.SoftEdgeRadius : 0;

                if (ax <= half + extra && ay <= half + extra)
                {
                    return true;
                }
            }
        }

        return false;
    }
}


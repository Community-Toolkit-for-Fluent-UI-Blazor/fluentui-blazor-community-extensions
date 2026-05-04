namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides stroke simplification processing for digital ink signatures using the Douglas-Peucker algorithm.
/// </summary>
/// <remarks>This processor reduces the number of points in each stroke to simplify the signature while preserving
/// its overall shape. Simplification is applied only if enabled in the provided engine options. The processor is
/// typically used to optimize signature data for storage or rendering without significantly altering the visual
/// appearance.</remarks>
public sealed class StrokeSimplificationProcessor : IStrokeProcessor
{
    /// <inheritdoc />
    public IEnumerable<SignatureStroke> Process(
        IEnumerable<SignatureStroke> strokes,
        SignatureEngineOptions options)
    {
        var opt = options.Stroke;

        if (opt.SimplificationEnabled)
        {
            foreach (var stroke in strokes)
            {
                var simplified = DouglasPeucker(stroke.Points, opt.SimplificationTolerance);
                yield return new SignatureStroke(stroke.Style.Clone(), simplified);
            }
        }
    }

    /// <summary>
    /// Reduces the number of points in a polyline using the Douglas-Peucker algorithm while preserving its overall
    /// shape within a specified tolerance.
    /// </summary>
    /// <remarks>This method is commonly used to simplify polylines in digital ink, signature, or path data.
    /// The endpoints of the polyline are always retained. The algorithm ensures that the simplified polyline does not
    /// deviate from the original by more than the specified tolerance.</remarks>
    /// <param name="pts">The collection of points representing the original polyline to be simplified. Must contain at least two points.</param>
    /// <param name="tolerance">The maximum allowed deviation from the original polyline. Points that deviate less than this value may be
    /// removed.</param>
    /// <returns>A list of points representing the simplified polyline. The returned list contains a subset of the original
    /// points, preserving the endpoints.</returns>
    private static List<SignaturePoint> DouglasPeucker(
        IReadOnlyList<SignaturePoint> pts,
        double tolerance)
    {
        if (pts.Count < 3)
        {
            return [.. pts];
        }

        var keep = new bool[pts.Count];
        keep[0] = keep[^1] = true;

        Simplify(pts, 0, pts.Count - 1, tolerance, keep);

        var result = new List<SignaturePoint>();

        for (var i = 0; i < pts.Count; i++)
        {
            if (keep[i])
            {
                result.Add(pts[i]);
            }
        }

        return result;
    }

    /// <summary>
    /// Recursively marks points in a polyline to retain based on their perpendicular distance from a baseline, enabling
    /// simplification of the polyline within a specified tolerance.
    /// </summary>
    /// <remarks>This method is typically used as part of the Ramer–Douglas–Peucker algorithm for polyline
    /// simplification. It does not modify the input list of points but updates the provided boolean array to indicate
    /// which points to keep.</remarks>
    /// <param name="pts">The list of points representing the polyline to be simplified.</param>
    /// <param name="start">The index of the starting point of the current segment under consideration.</param>
    /// <param name="end">The index of the ending point of the current segment under consideration.</param>
    /// <param name="tolerance">The maximum allowed perpendicular distance from the baseline for a point to be considered insignificant and thus
    /// removable.</param>
    /// <param name="keep">A boolean array indicating which points should be retained in the simplified polyline. Points marked as <see
    /// langword="true"/> are kept.</param>
    private static void Simplify(
        IReadOnlyList<SignaturePoint> pts,
        int start, int end,
        double tolerance,
        bool[] keep)
    {
        if (end <= start + 1)
        {
            return;
        }

        var maxDist = 0.0;
        var index = -1;

        var a = pts[start];
        var b = pts[end];

        for (var i = start + 1; i < end; i++)
        {
            var dist = PerpendicularDistance(pts[i], a, b);

            if (dist > maxDist)
            {
                maxDist = dist;
                index = i;
            }
        }

        if (maxDist > tolerance)
        {
            keep[index] = true;
            Simplify(pts, start, index, tolerance, keep);
            Simplify(pts, index, end, tolerance, keep);
        }
    }

    /// <summary>
    /// Calculates the perpendicular distance from a point to a line segment defined by two endpoints, which is used to determine
    /// </summary>
    /// <param name="p">The point from which the perpendicular distance to the line segment is being calculated.</param>
    /// <param name="a">The starting point of the line segment to which the distance is being calculated.</param>
    /// <param name="b">The ending point of the line segment to which the distance is being calculated.</param>
    /// <returns>Returns the perpendicular distance from point <paramref name="p"/> to the line segment
    ///  defined by points <paramref name="a"/> and <paramref name="b"/></returns>
    private static double PerpendicularDistance(SignaturePoint p, SignaturePoint a, SignaturePoint b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;

        if (dx == 0 && dy == 0)
        {
            return Math.Sqrt(Math.Pow(p.X - a.X, 2) + Math.Pow(p.Y - a.Y, 2));
        }

        var t = ((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy);
        var projX = a.X + t * dx;
        var projY = a.Y + t * dy;

        return Math.Sqrt(Math.Pow(p.X - projX, 2) + Math.Pow(p.Y - projY, 2));
    }
}

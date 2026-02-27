namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides hit testing functionality for determining whether a point is within a specified tolerance of any stroke
/// managed by a given stroke manager.
/// </summary>
/// <remarks>This class is intended for internal use to support hit testing operations on collections of signature
/// strokes. It enables efficient detection of user interactions with drawn strokes, such as selecting or editing
/// strokes based on pointer input.</remarks>
public sealed class StrokeHitTester
{
    /// <summary>
    /// Represents the stroke manager that provides access to the collection of signature strokes for hit testing operations.
    /// </summary>
    private readonly StrokeManager _strokes;

    /// <summary>
    /// Initializes a new instance of the StrokeHitTester class using the specified stroke manager.
    /// </summary>
    /// <param name="strokes">The StrokeManager instance that manages the collection of strokes to be used for hit testing. Cannot be null.</param>
    public StrokeHitTester(StrokeManager strokes)
    {
        _strokes = strokes;
    }

    /// <summary>
    /// Returns the first signature stroke that is within the specified tolerance of the given point, if any.
    /// </summary>
    /// <remarks>This method checks each stroke and returns the first one that is close enough to the
    /// specified point. If multiple strokes are within the tolerance, only the first encountered is returned.</remarks>
    /// <param name="x">The X-coordinate of the point to test, in device-independent units.</param>
    /// <param name="y">The Y-coordinate of the point to test, in device-independent units.</param>
    /// <param name="tolerance">The maximum distance, in device-independent units, from the point to a stroke for it to be considered a hit.
    /// Must be non-negative.</param>
    /// <returns>A <see cref="SignatureStroke"/> that is within the specified tolerance of the point; otherwise, <see
    /// langword="null"/> if no stroke is found.</returns>
    public SignatureStroke? HitTestPoint(double x, double y, double tolerance)
    {
        var tolSq = tolerance * tolerance;

        foreach (var stroke in _strokes.Strokes)
        {
            var pts = stroke.Points;

            if (pts.Count == 0)
            {
                continue;
            }

            if (pts.Count == 1)
            {
                var p = pts[0];

                if (SignatureMathUtils.DistanceSquared(p.X, p.Y, x, y) <= tolSq)
                {
                    return stroke;
                }

                continue;
            }

            for (var i = 0; i < pts.Count - 1; i++)
            {
                var p1 = pts[i];
                var p2 = pts[i + 1];

                if (PointToSegmentDistanceSquared(x, y, p1.X, p1.Y, p2.X, p2.Y) <= tolSq)
                {
                    return stroke;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Performs a hit test to find the topmost signature stroke that is within the specified tolerance of the given
    /// point.
    /// </summary>
    /// <remarks>The method checks all strokes in reverse order, returning the first stroke that is within the
    /// specified tolerance. This ensures that strokes drawn later (and thus visually on top) are prioritized.</remarks>
    /// <param name="x">The x-coordinate of the point to test, in the same coordinate space as the signature strokes.</param>
    /// <param name="y">The y-coordinate of the point to test, in the same coordinate space as the signature strokes.</param>
    /// <param name="tolerance">The maximum distance, in coordinate units, from the point to a stroke for the stroke to be considered a hit.
    /// Must be non-negative.</param>
    /// <returns>The topmost signature stroke that is within the specified tolerance of the given point; otherwise, null if no
    /// stroke is hit.</returns>
    public SignatureStroke? HitTestTopMost(double x, double y, double tolerance)
    {
        var tolSq = tolerance * tolerance;
        var strokes = _strokes.Strokes;

        for (var s = strokes.Count - 1; s >= 0; s--)
        {
            var stroke = strokes[s];
            var pts = stroke.Points;

            if (pts.Count == 0)
            {
                continue;
            }

            if (pts.Count == 1)
            {
                var p = pts[0];

                if (SignatureMathUtils.DistanceSquared(p.X, p.Y, x, y) <= tolSq)
                {
                    return stroke;
                }

                continue;
            }

            for (var i = 0; i < pts.Count - 1; i++)
            {
                var p1 = pts[i];
                var p2 = pts[i + 1];

                if (PointToSegmentDistanceSquared(x, y, p1.X, p1.Y, p2.X, p2.Y) <= tolSq)
                {
                    return stroke;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Calculates the squared distance between a point and the closest point on a line segment defined by two
    /// endpoints.
    /// </summary>
    /// <remarks>This method avoids computing the square root for performance reasons. Use when only the
    /// relative distance is needed or when comparing distances.</remarks>
    /// <param name="px">The X-coordinate of the point.</param>
    /// <param name="py">The Y-coordinate of the point.</param>
    /// <param name="x1">The X-coordinate of the first endpoint of the segment.</param>
    /// <param name="y1">The Y-coordinate of the first endpoint of the segment.</param>
    /// <param name="x2">The X-coordinate of the second endpoint of the segment.</param>
    /// <param name="y2">The Y-coordinate of the second endpoint of the segment.</param>
    /// <returns>The squared distance between the specified point and the nearest point on the segment.</returns>
    private static double PointToSegmentDistanceSquared(
        double px, double py,
        double x1, double y1,
        double x2, double y2)
    {
        var dx = x2 - x1;
        var dy = y2 - y1;

        if (dx == 0 && dy == 0)
        {
            return SignatureMathUtils.DistanceSquared(px, py, x1, y1);
        }

        var t = SignatureMathUtils.ProjectPointOnSegment(px, py, x1, y1, x2, y2);
        t = SignatureMathUtils.Clamp(t, 0.0, 1.0);

        var projX = x1 + t * dx;
        var projY = y1 + t * dy;

        return SignatureMathUtils.DistanceSquared(px, py, projX, projY);
    }
}

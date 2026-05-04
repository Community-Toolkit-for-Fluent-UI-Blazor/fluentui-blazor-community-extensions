namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides hit testing functionality to determine which signature strokes intersect with a specified rectangular area.
/// </summary>
/// <remarks>This class is intended for internal use in scenarios where it is necessary to identify strokes that
/// are partially or fully within a given rectangle, such as for selection or manipulation in drawing applications.
/// Instances are associated with a specific collection of strokes and are not thread-safe.</remarks>
public sealed class RectangleHitTester
{
    /// <summary>
    /// Represents the stroke manager that provides access to the collection of signature strokes for hit testing operations.
    /// </summary>
    private readonly StrokeManager _strokes;

    /// <summary>
    /// Initializes a new instance of the <see cref="RectangleHitTester"/> class with the specified stroke manager.
    /// </summary>
    /// <param name="strokes">Manager that provides access to the collection of signature strokes to be tested against the rectangle. Cannot be null.</param>
    public RectangleHitTester(StrokeManager strokes)
    {
        _strokes = strokes;
    }

    /// <summary>
    /// Returns a read-only list of signature strokes that intersect with the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangular region to test for intersection with signature strokes.</param>
    /// <returns>A read-only list of signature strokes that intersect with the specified rectangle. The list is empty if no
    /// strokes intersect.</returns>
    public IReadOnlyList<SignatureStroke> HitTestRectangle(RectD rect)
    {
        var result = new List<SignatureStroke>();

        foreach (var stroke in _strokes.Strokes)
        {
            if (IntersectsStroke(rect, stroke))
            {
                result.Add(stroke);
            }
        }

        return result;
    }

    /// <summary>
    /// Determines whether any part of the specified stroke intersects with the given rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to test for intersection with the stroke.</param>
    /// <param name="stroke">The stroke whose points and segments are tested for intersection with the rectangle.</param>
    /// <returns>true if any point or segment of the stroke intersects the rectangle; otherwise, false.</returns>
    private static bool IntersectsStroke(RectD rect, SignatureStroke stroke)
    {
        var pts = stroke.Points;

        if (pts.Count == 0)
        {
            return false;
        }

        foreach (var p in pts)
        {
            if (rect.Contains(p.X, p.Y))
            {
                return true;
            }
        }

        for (var i = 0; i < pts.Count - 1; i++)
        {
            var p1 = pts[i];
            var p2 = pts[i + 1];

            if (SegmentIntersectsRect(p1.X, p1.Y, p2.X, p2.Y, rect))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether a line segment defined by two points (x1, y1) and (x2, y2) intersects with a given rectangle.
    /// </summary>
    /// <param name="x1">The x-coordinate of the first point of the line segment.</param>
    /// <param name="y1">The y-coordinate of the first point of the line segment.</param>
    /// <param name="x2">The x-coordinate of the second point of the line segment.</param>
    /// <param name="y2">The y-coordinate of the second point of the line segment.</param>
    /// <param name="r">Bounding rectangle to test for intersection with the line segment.</param>
    /// <returns></returns>
    private static bool SegmentIntersectsRect(double x1, double y1, double x2, double y2, RectD r)
    {
        if (x1 < r.Left && x2 < r.Left)
        {
            return false;
        }

        if (x1 > r.Right && x2 > r.Right)
        {
            return false;
        }

        if (y1 < r.Top && y2 < r.Top)
        {
            return false;
        }

        if (y1 > r.Bottom && y2 > r.Bottom)
        {
            return false;
        }

        if (r.Contains(x1, y1) || r.Contains(x2, y2))
        {
            return true;
        }

        return LineIntersectsLine(x1, y1, x2, y2, r.Left, r.Top, r.Right, r.Top) ||
               LineIntersectsLine(x1, y1, x2, y2, r.Right, r.Top, r.Right, r.Bottom) ||
               LineIntersectsLine(x1, y1, x2, y2, r.Right, r.Bottom, r.Left, r.Bottom) ||
               LineIntersectsLine(x1, y1, x2, y2, r.Left, r.Bottom, r.Left, r.Top);
    }

    /// <summary>
    /// Determines whether two line segments defined by their endpoints intersect with each other.
    /// </summary>
    /// <param name="x1">Start x-coordinate of the first line segment.</param>
    /// <param name="y1">Start y-coordinate of the first line segment.</param>
    /// <param name="x2">End x-coordinate of the first line segment.</param>
    /// <param name="y2">End y-coordinate of the first line segment.</param>
    /// <param name="x3">Start x-coordinate of the second line segment.</param>
    /// <param name="y3">Start y-coordinate of the second line segment.</param>
    /// <param name="x4">End x-coordinate of the second line segment.</param>
    /// <param name="y4">End y-coordinate of the second line segment.</param>
    /// <returns></returns>
    private static bool LineIntersectsLine(
        double x1,
        double y1,
        double x2,
        double y2,
        double x3,
        double y3,
        double x4,
        double y4)
    {
        var d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

        if (Math.Abs(d) < double.Epsilon)
        {
            return false;
        }

        var t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / d;
        var u = -((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3)) / d;

        return t >= 0 && t <= 1 && u >= 0 && u <= 1;
    }
}

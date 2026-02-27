namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Processes signature strokes by clipping them to a specified rectangular region.
/// </summary>
/// <remarks>This processor implements stroke clipping using a rectangular boundary defined by the provided width
/// and height. It is typically used to ensure that signature strokes do not extend beyond the visible or allowed
/// drawing area. The processor can be used as part of a signature processing pipeline to enforce boundary constraints
/// on user input.</remarks>
public sealed class StrokeClippingProcessor : IStrokeProcessor
{
    private readonly double _width;
    private readonly double _height;
    private const int INSIDE = 0;
    private const int LEFT = 1;
    private const int RIGHT = 2;
    private const int BOTTOM = 4;
    private const int TOP = 8;

    /// <summary>
    /// Initializes a new instance of the StrokeClippingProcessor class with the specified width and height.
    /// </summary>
    /// <param name="width">The width of the clipping area, in device-independent units. Must be a non-negative value.</param>
    /// <param name="height">The height of the clipping area, in device-independent units. Must be a non-negative value.</param>
    public StrokeClippingProcessor(double width, double height)
    {
        _width = width;
        _height = height;
    }

    /// <inheritdoc />
    public IEnumerable<SignatureStroke> Process(
        IEnumerable<SignatureStroke> strokes,
        SignatureEngineOptions options)
    {
        if (!options.Stroke.ClipToBounds)
        {
            foreach (var s in strokes)
            {
                yield return s;
            }

            yield break;
        }

        foreach (var stroke in strokes)
        {
            var clippedPoints = ClipStroke(stroke.Points).ToList();

            if (clippedPoints.Count > 1)
            {
                yield return new SignatureStroke(stroke.Style.Clone(), clippedPoints);
            }
        }
    }

    /// <summary>
    /// Clips the input stroke to a defined region and returns the resulting sequence of signature points.
    /// </summary>
    /// <remarks>The method processes each consecutive pair of points in the input collection and yields only
    /// those segments that intersect the clipping region. The order of points in the output corresponds to the order of
    /// valid segments in the input.</remarks>
    /// <param name="points">The collection of signature points representing the original stroke to be clipped. Must contain at least two
    /// points.</param>
    /// <returns>An enumerable collection of signature points representing the clipped segments of the stroke. Returns an empty
    /// sequence if the input contains fewer than two points.</returns>
    private IEnumerable<SignaturePoint> ClipStroke(List<SignaturePoint> points)
    {
        if (points.Count < 2)
        {
            yield break;
        }

        for (var i = 0; i < points.Count - 1; i++)
        {
            var p1 = points[i];
            var p2 = points[i + 1];

            if (ClipSegment(ref p1, ref p2))
            {
                yield return p1;
                yield return p2;
            }
        }
    }

    /// <summary>
    /// Calculates the outcode for a point relative to the rectangular clipping region.
    /// </summary>
    /// <remarks>The outcode is typically used in line clipping algorithms, such as Cohen–Sutherland, to
    /// determine the position of a point relative to a rectangular boundary.</remarks>
    /// <param name="x">The x-coordinate of the point to evaluate.</param>
    /// <param name="y">The y-coordinate of the point to evaluate.</param>
    /// <returns>An integer bitmask representing the outcode for the specified point. The outcode indicates which region relative
    /// to the clipping rectangle the point lies in (inside, left, right, top, or bottom).</returns>
    private int ComputeOutCode(double x, double y)
    {
        var code = INSIDE;

        if (x < 0)
        {
            code |= LEFT;
        }
        else if (x > _width)
        {
            code |= RIGHT;
        }

        if (y < 0)
        {
            code |= BOTTOM;
        }
        else if (y > _height)
        {
            code |= TOP;
        }

        return code;
    }

    /// <summary>
    /// Attempts to clip a line segment defined by two signature points to the current rectangular bounds.
    /// </summary>
    /// <remarks>This method modifies the input points to represent the clipped segment if it intersects the
    /// rectangular bounds. If the segment lies entirely outside the bounds, the points are not modified and the method
    /// returns false.</remarks>
    /// <param name="p1">The first endpoint of the segment. If the segment is clipped, this value is updated to the new endpoint within
    /// the bounds.</param>
    /// <param name="p2">The second endpoint of the segment. If the segment is clipped, this value is updated to the new endpoint within
    /// the bounds.</param>
    /// <returns>true if the segment intersects the bounds and has been clipped or remains unchanged; otherwise, false.</returns>
    private bool ClipSegment(ref SignaturePoint p1, ref SignaturePoint p2)
    {
        double x1 = p1.X, y1 = p1.Y;
        double x2 = p2.X, y2 = p2.Y;

        var out1 = ComputeOutCode(x1, y1);
        var out2 = ComputeOutCode(x2, y2);

        var accept = false;

        while (true)
        {
            // Two endpoints are inside the rectangle.
            if ((out1 | out2) == 0)
            {
                accept = true;
                break;
            }
            // Two endpoints are outside the rectangle in the same region.
            else if ((out1 & out2) != 0)
            {
                break;
            }
            else
            {
                // Calculate intersection point;
                var x = 0.0;
                var y = 0.0;
                var outCode = out1 != 0 ? out1 : out2;

                if ((outCode & TOP) != 0)
                {
                    x = x1 + (x2 - x1) * (_height - y1) / (y2 - y1);
                    y = _height;
                }
                else if ((outCode & BOTTOM) != 0)
                {
                    x = x1 + (x2 - x1) * (0 - y1) / (y2 - y1);
                    y = 0;
                }
                else if ((outCode & RIGHT) != 0)
                {
                    y = y1 + (y2 - y1) * (_width - x1) / (x2 - x1);
                    x = _width;
                }
                else if ((outCode & LEFT) != 0)
                {
                    y = y1 + (y2 - y1) * (0 - x1) / (x2 - x1);
                    x = 0;
                }

                if (outCode == out1)
                {
                    x1 = x;
                    y1 = y;
                    out1 = ComputeOutCode(x1, y1);
                }
                else
                {
                    x2 = x;
                    y2 = y;
                    out2 = ComputeOutCode(x2, y2);
                }
            }
        }

        if (!accept)
        {
            return false;
        }

        p1 = new SignaturePoint(x1, y1, p1.Pressure, p1.Velocity, p1.Width, p1.Timestamp);
        p2 = new SignaturePoint(x2, y2, p2.Pressure, p2.Velocity, p2.Width, p2.Timestamp);

        return true;
    }
}

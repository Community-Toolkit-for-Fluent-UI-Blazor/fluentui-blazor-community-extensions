namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides stroke interpolation processing for digital ink signatures, generating additional points between existing
/// stroke points based on specified interpolation options.
/// </summary>
/// <remarks>This processor is typically used to smooth or resample signature strokes by interpolating points
/// according to the configured interpolation mode and spacing. It is designed to be used as part of a signature
/// processing pipeline and adheres to the options specified in the associated signature engine configuration.</remarks>
public sealed class StrokeInterpolationProcessor : IStrokeProcessor
{
    /// <inheritdoc />
    public IEnumerable<SignatureStroke> Process(
        IEnumerable<SignatureStroke> strokes,
        SignatureEngineOptions options)
    {
        var opt = options.Interpolation;

        if (opt.Enabled && opt.Mode != StrokeInterpolationMode.None)
        {
            foreach (var stroke in strokes)
            {
                yield return InterpolateStroke(stroke, opt);
            }
        }
    }

    /// <summary>
    /// Interpolates the points of a signature stroke using the specified interpolation options.
    /// </summary>
    /// <remarks>If the stroke contains fewer than two points, the original stroke is returned without
    /// modification. The interpolation mode and spacing in the options control how the points are generated. If final
    /// resampling is enabled in the options, an additional resampling step is applied after interpolation.</remarks>
    /// <param name="stroke">The original signature stroke to interpolate.</param>
    /// <param name="opt">The interpolation options that determine the interpolation mode and spacing.</param>
    /// <returns>A new SignatureStroke instance containing the interpolated points based on the specified options.</returns>
    private static SignatureStroke InterpolateStroke(
        SignatureStroke stroke,
        SignatureInterpolationOptions opt)
    {
        var pts = stroke.Points;

        if (pts.Count < 2)
        {
            return stroke;
        }

        var result = opt.Mode switch
        {
            StrokeInterpolationMode.Resample => ResampleStroke(pts, opt.Spacing),
            StrokeInterpolationMode.CatmullRom => CatmullRomStroke(pts, opt.Spacing),
            StrokeInterpolationMode.Quadratic => QuadraticStroke(pts, opt.Spacing),
            StrokeInterpolationMode.Bezier => BezierStroke(pts, opt.Spacing),
            _ => pts
        };

        if (opt.FinalResample)
        {
            result = ResampleStroke(result, opt.Spacing);
        }

        return new SignatureStroke(stroke.Style.Clone(), result);
    }

    /// <summary>
    /// Resamples a sequence of signature points so that the resulting points are spaced at approximately equal
    /// intervals.
    /// </summary>
    /// <remarks>This method preserves the first point of the input sequence and generates additional points
    /// as needed to maintain the specified spacing. The last point of the input may not be included if it does not
    /// align with the spacing interval.</remarks>
    /// <param name="pts">The collection of signature points representing the original stroke to be resampled. Must not be null.</param>
    /// <param name="spacing">The desired distance between consecutive points in the resampled stroke. Must be greater than zero.</param>
    /// <returns>A list of signature points representing the resampled stroke, with points spaced at approximately the specified
    /// interval. Returns an empty list if the input collection is empty.</returns>
    private static List<SignaturePoint> ResampleStroke(
        IReadOnlyList<SignaturePoint> pts,
        double spacing)
    {
        var result = new List<SignaturePoint>();

        if (pts.Count == 0)
        {
            return result;
        }

        result.Add(pts[0]);

        var acc = 0.0;

        for (var i = 1; i < pts.Count; i++)
        {
            var a = result[^1];
            var b = pts[i];

            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            var dist = Math.Sqrt(dx * dx + dy * dy);

            if (acc + dist >= spacing)
            {
                var t = (spacing - acc) / dist;

                result.Add(InterpolatePoint(a, b, t));
                acc = 0;
            }
            else
            {
                acc += dist;
            }
        }

        return result;
    }

    /// <summary>
    /// Generates a smoothed stroke by interpolating a sequence of signature points using the Catmull-Rom spline
    /// algorithm.
    /// </summary>
    /// <remarks>This method creates a smooth curve that passes through the input points, suitable for
    /// rendering freehand signatures or paths. The Catmull-Rom spline ensures that the curve is continuous and visually
    /// smooth.</remarks>
    /// <param name="pts">The collection of signature points to interpolate. Must contain at least two points.</param>
    /// <param name="spacing">The desired distance between interpolated points along the resulting stroke. Must be a positive value.</param>
    /// <returns>A list of signature points representing the smoothed stroke, with points spaced approximately according to the
    /// specified spacing.</returns>
    private static List<SignaturePoint> CatmullRomStroke(
        IReadOnlyList<SignaturePoint> pts,
        double spacing)
    {
        var result = new List<SignaturePoint>();

        for (var i = 0; i < pts.Count - 1; i++)
        {
            var p0 = i > 0 ? pts[i - 1] : pts[i];
            var p1 = pts[i];
            var p2 = pts[i + 1];
            var p3 = i + 2 < pts.Count ? pts[i + 2] : pts[i + 1];

            foreach (var p in CatmullRomSegment(p0, p1, p2, p3, spacing))
            {
                result.Add(p);
            }
        }

        return result;
    }

    /// <summary>
    /// Generates a sequence of points along a Catmull-Rom spline segment defined by four control points, using the
    /// specified spacing between points.
    /// </summary>
    /// <remarks>The number of points generated depends on the distance between the start and end points and
    /// the specified spacing. The sequence always includes the start and end points of the segment.</remarks>
    /// <param name="p0">The first control point, which influences the tangent at the start of the segment.</param>
    /// <param name="p1">The starting point of the spline segment.</param>
    /// <param name="p2">The ending point of the spline segment.</param>
    /// <param name="p3">The fourth control point, which influences the tangent at the end of the segment.</param>
    /// <param name="spacing">The approximate distance between consecutive points generated along the spline. Must be greater than zero.</param>
    /// <returns>An enumerable collection of points representing the Catmull-Rom spline segment between the specified start and
    /// end points.</returns>
    private static IEnumerable<SignaturePoint> CatmullRomSegment(
        SignaturePoint p0,
        SignaturePoint p1,
        SignaturePoint p2,
        SignaturePoint p3,
        double spacing)
    {
        var length = Distance(p1, p2);
        var steps = (int)(length / spacing);

        if (steps < 1)
        {
            steps = 1;
        }

        for (var i = 0; i <= steps; i++)
        {
            var t = i / (double)steps;

            yield return CatmullRomPoint(p0, p1, p2, p3, t);
        }
    }

    /// <summary>
    /// Calculates a point on a Catmull-Rom spline defined by four control points and a parameter t, which represents the
    /// </summary>
    /// <param name="p0">The first control point, which influences the tangent at the start of the segment.</param>
    /// <param name="p1">The starting point of the spline segment.</param>
    /// <param name="p2">The ending point of the spline segment.</param>
    /// <param name="p3">The fourth control point, which influences the tangent at the end of the segment.</param>
    /// <param name="t">The parameter between 0 and 1 that indicates the position along the spline segment, where 0 corresponds to p1 and 1 corresponds to p2.</param>
    /// <returns>Returns a SignaturePoint representing the position on the Catmull-Rom spline corresponding to the parameter t, calculated based on the four control points.</returns>
    private static SignaturePoint CatmullRomPoint(
        SignaturePoint p0,
        SignaturePoint p1,
        SignaturePoint p2,
        SignaturePoint p3,
        double t)
    {
        var t2 = t * t;
        var t3 = t2 * t;

        var x =
            0.5 * ((2 * p1.X) +
            (-p0.X + p2.X) * t +
            (2 * p0.X - 5 * p1.X + 4 * p2.X - p3.X) * t2 +
            (-p0.X + 3 * p1.X - 3 * p2.X + p3.X) * t3);

        var y =
            0.5 * ((2 * p1.Y) +
            (-p0.Y + p2.Y) * t +
            (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * t2 +
            (-p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y) * t3);

        return new SignaturePoint(
            x, y,
            Lerp(p1.Pressure, p2.Pressure, t),
            Lerp(p1.Velocity, p2.Velocity, t),
            Lerp(p1.Width, p2.Width, t),
            Lerp(p1.Timestamp, p2.Timestamp, t)
        );
    }

    /// <summary>
    /// Generates a list of interpolated points forming a smooth quadratic stroke through the specified sequence of
    /// signature points, using the given spacing between points.
    /// </summary>
    /// <remarks>This method processes each consecutive triplet of points in the input collection to generate
    /// smooth curves. The resulting stroke can be used for rendering smooth signatures or freehand lines.</remarks>
    /// <param name="pts">The collection of signature points to interpolate. Must contain at least three points to form a quadratic
    /// segment.</param>
    /// <param name="spacing">The distance between consecutive interpolated points along the stroke. Must be a positive value.</param>
    /// <returns>A list of signature points representing the interpolated quadratic stroke. The list will be empty if fewer than
    /// three points are provided.</returns>
    private static List<SignaturePoint> QuadraticStroke(
        IReadOnlyList<SignaturePoint> pts,
        double spacing)
    {
        var result = new List<SignaturePoint>();

        for (var i = 0; i < pts.Count - 2; i++)
        {
            var p0 = pts[i];
            var p1 = pts[i + 1];
            var p2 = pts[i + 2];

            foreach (var p in QuadraticSegment(p0, p1, p2, spacing))
            {
                result.Add(p);
            }
        }

        return result;
    }

    /// <summary>
    /// Generates a sequence of points that approximate a quadratic Bézier curve defined by three control points, using
    /// the specified spacing between points.
    /// </summary>
    /// <remarks>The generated points interpolate position and additional properties such as pressure,
    /// velocity, width, and time between the start and end points. The number of points depends on the total curve
    /// length and the specified spacing.</remarks>
    /// <param name="p0">The starting control point of the quadratic Bézier curve.</param>
    /// <param name="p1">The control point that defines the curve's shape between the start and end points.</param>
    /// <param name="p2">The ending control point of the quadratic Bézier curve.</param>
    /// <param name="spacing">The approximate distance between consecutive points in the generated sequence. Must be greater than zero.</param>
    /// <returns>An enumerable collection of points representing the quadratic Bézier curve, spaced approximately by the
    /// specified distance.</returns>
    private static IEnumerable<SignaturePoint> QuadraticSegment(
        SignaturePoint p0,
        SignaturePoint p1,
        SignaturePoint p2,
        double spacing)
    {
        var length = Distance(p0, p2);
        var steps = (int)(length / spacing);

        if (steps < 1)
        {
            steps = 1;
        }

        for (var i = 0; i <= steps; i++)
        {
            var t = i / (double)steps;

            var x = (1 - t) * (1 - t) * p0.X + 2 * (1 - t) * t * p1.X + t * t * p2.X;
            var y = (1 - t) * (1 - t) * p0.Y + 2 * (1 - t) * t * p1.Y + t * t * p2.Y;

            yield return new SignaturePoint(
                x, y,
                Lerp(p0.Pressure, p2.Pressure, t),
                Lerp(p0.Velocity, p2.Velocity, t),
                Lerp(p0.Width, p2.Width, t),
                Lerp(p0.Timestamp, p2.Timestamp, t)
            );
        }
    }

    /// <summary>
    /// Generates a list of interpolated signature points along a series of cubic Bézier curve segments, using the
    /// specified spacing between points.
    /// </summary>
    /// <remarks>Each group of four consecutive points in the input defines a cubic Bézier segment. The method
    /// processes the input in steps of three to create multiple connected segments, if possible.</remarks>
    /// <param name="pts">A read-only list of signature points that defines the control points for one or more cubic Bézier curve
    /// segments. The list must contain at least four points, and each consecutive group of four points defines a
    /// segment.</param>
    /// <param name="spacing">The distance between consecutive interpolated points along each Bézier segment. Must be a positive value.</param>
    /// <returns>A list of signature points representing the interpolated points along the Bézier curve segments. The list is
    /// empty if no segments can be formed from the input.</returns>
    private static List<SignaturePoint> BezierStroke(
        IReadOnlyList<SignaturePoint> pts,
        double spacing)
    {
        var result = new List<SignaturePoint>();

        for (var i = 0; i < pts.Count - 3; i += 3)
        {
            var p0 = pts[i];
            var p1 = pts[i + 1];
            var p2 = pts[i + 2];
            var p3 = pts[i + 3];

            foreach (var p in BezierSegment(p0, p1, p2, p3, spacing))
            {
                result.Add(p);
            }
        }

        return result;
    }

    /// <summary>
    /// Generates a sequence of interpolated points along a cubic Bézier curve defined by four control points, spaced at
    /// approximately equal intervals.
    /// </summary>
    /// <remarks>The number of points generated depends on the total length of the curve and the specified
    /// spacing. The returned sequence always includes the start and end points.</remarks>
    /// <param name="p0">The starting control point of the Bézier curve.</param>
    /// <param name="p1">The first control point influencing the curve's shape between the start and end points.</param>
    /// <param name="p2">The second control point influencing the curve's shape between the start and end points.</param>
    /// <param name="p3">The ending control point of the Bézier curve.</param>
    /// <param name="spacing">The approximate distance between consecutive points along the curve. Must be a positive value.</param>
    /// <returns>An enumerable collection of SignaturePoint instances representing points along the Bézier curve, spaced at
    /// approximately the specified interval.</returns>
    private static IEnumerable<SignaturePoint> BezierSegment(
        SignaturePoint p0,
        SignaturePoint p1,
        SignaturePoint p2,
        SignaturePoint p3,
        double spacing)
    {
        var length = Distance(p0, p3);
        var steps = (int)(length / spacing);

        if (steps < 1)
        {
            steps = 1;
        }

        for (var i = 0; i <= steps; i++)
        {
            var t = i / (double)steps;

            var u = 1 - t;
            var x = u * u * u * p0.X +
                       3 * u * u * t * p1.X +
                       3 * u * t * t * p2.X +
                       t * t * t * p3.X;

            var y = u * u * u * p0.Y +
                       3 * u * u * t * p1.Y +
                       3 * u * t * t * p2.Y +
                       t * t * t * p3.Y;

            yield return new SignaturePoint(
                x, y,
                Lerp(p0.Pressure, p3.Pressure, t),
                Lerp(p0.Velocity, p3.Velocity, t),
                Lerp(p0.Width, p3.Width, t),
                Lerp(p0.Timestamp, p3.Timestamp, t)
            );
        }
    }

    /// <summary>
    /// Calculates the linear interpolation between two values based on the specified weighting factor.
    /// </summary>
    /// <remarks>If the interpolation factor is outside the range [0.0, 1.0], the result will be extrapolated
    /// beyond the start or end values.</remarks>
    /// <param name="a">The start value of the interpolation.</param>
    /// <param name="b">The end value of the interpolation.</param>
    /// <param name="t">The interpolation factor, typically between 0.0 and 1.0, where 0.0 returns the start value and 1.0 returns the
    /// end value.</param>
    /// <returns>The interpolated value between the start and end values, based on the specified factor.</returns>
    private static double Lerp(double a, double b, double t) => a + (b - a) * t;

    /// <summary>
    /// Calculates the Euclidean distance between two signature points.
    /// </summary>
    /// <param name="a">The first signature point used as the starting position for the distance calculation.</param>
    /// <param name="b">The second signature point used as the ending position for the distance calculation.</param>
    /// <returns>The straight-line distance between the two specified signature points.</returns>
    private static double Distance(SignaturePoint a, SignaturePoint b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Calculates an interpolated signature point between two specified points using linear interpolation for each
    /// property.
    /// </summary>
    /// <remarks>This method performs linear interpolation independently on each property of the signature
    /// points, including position, pressure, velocity, width, and time. Values of 't' outside the range [0.0, 1.0] will
    /// extrapolate beyond the provided points.</remarks>
    /// <param name="a">The starting signature point for interpolation.</param>
    /// <param name="b">The ending signature point for interpolation.</param>
    /// <param name="t">The interpolation factor, typically between 0.0 and 1.0, where 0.0 returns point 'a' and 1.0 returns point 'b'.</param>
    /// <returns>A new SignaturePoint whose properties are linearly interpolated between 'a' and 'b' according to the specified
    /// factor.</returns>
    private static SignaturePoint InterpolatePoint(
        SignaturePoint a,
        SignaturePoint b,
        double t)
    {
        return new SignaturePoint(
            x: Lerp(a.X, b.X, t),
            y: Lerp(a.Y, b.Y, t),
            pressure: Lerp(a.Pressure, b.Pressure, t),
            velocity: Lerp(a.Velocity, b.Velocity, t),
            width: Lerp(a.Width, b.Width, t),
            timestamp: Lerp(a.Timestamp, b.Timestamp, t)
        );
    }
}


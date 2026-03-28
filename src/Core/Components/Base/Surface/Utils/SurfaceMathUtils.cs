using System.Globalization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides utility methods for performing common mathematical operations related to signature processing, such as
/// distance calculations, interpolation, clamping, geometric projections, intersection checks, velocity computation,
/// and curve evaluation.
/// </summary>
/// <remarks>This class is intended for internal use within signature-related components and is not designed for
/// general-purpose mathematical operations. All methods are static and operate on primitive numeric types or
/// signature-specific data structures. Thread safety is ensured as no instance state is maintained.</remarks>
internal class SurfaceMathUtils
{
    /// <summary>
    /// Calculates the squared Euclidean distance between two points in a two-dimensional plane.
    /// </summary>
    /// <remarks>This method avoids the computational cost of taking the square root, making it suitable for
    /// comparisons where the actual distance is not required.</remarks>
    /// <param name="x1">The X-coordinate of the first point.</param>
    /// <param name="y1">The Y-coordinate of the first point.</param>
    /// <param name="x2">The X-coordinate of the second point.</param>
    /// <param name="y2">The Y-coordinate of the second point.</param>
    /// <returns>A double value representing the squared distance between the two points.</returns>
    public static double DistanceSquared(double x1, double y1, double x2, double y2)
    => (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);

    /// <summary>
    /// Calculates the Euclidean distance between two points in a two-dimensional plane.
    /// </summary>
    /// <param name="x1">The X-coordinate of the first point.</param>
    /// <param name="y1">The Y-coordinate of the first point.</param>
    /// <param name="x2">The X-coordinate of the second point.</param>
    /// <param name="y2">The Y-coordinate of the second point.</param>
    /// <returns>The distance between the two specified points as a double-precision floating-point value.</returns>
    public static double Distance(double x1, double y1, double x2, double y2)
        => Math.Sqrt(DistanceSquared(x1, y1, x2, y2));

    /// <summary>
    /// Calculates the linear interpolation between two values based on a given interpolation factor.
    /// </summary>
    /// <remarks>If the interpolation factor is outside the range [0, 1], the result will be extrapolated
    /// beyond the start or end values.</remarks>
    /// <param name="a">The start value for the interpolation.</param>
    /// <param name="b">The end value for the interpolation.</param>
    /// <param name="t">The interpolation factor, typically in the range [0, 1], where 0 returns the start value and 1 returns the end
    /// value.</param>
    /// <returns>A double representing the interpolated value between the start and end values, based on the specified
    /// interpolation factor.</returns>
    public static double Lerp(double a, double b, double t) => a + (b - a) * t;

    /// <summary>
    /// Constrains a double-precision floating-point value to be within the specified minimum and maximum bounds.
    /// </summary>
    /// <remarks>If <paramref name="min"/> is greater than <paramref name="max"/>, the result may not be
    /// meaningful. It is recommended to ensure that <paramref name="min"/> is less than or equal to <paramref
    /// name="max"/>.</remarks>
    /// <param name="value">The value to be clamped within the specified range.</param>
    /// <param name="min">The minimum allowable value. If <paramref name="value"/> is less than this, <paramref name="min"/> is returned.</param>
    /// <param name="max">The maximum allowable value. If <paramref name="value"/> is greater than this, <paramref name="max"/> is
    /// returned.</param>
    /// <returns>A double value that is within the range defined by <paramref name="min"/> and <paramref name="max"/>. Returns
    /// <paramref name="min"/> if <paramref name="value"/> is less than <paramref name="min"/>; returns <paramref
    /// name="max"/> if <paramref name="value"/> is greater than <paramref name="max"/>; otherwise, returns <paramref
    /// name="value"/>.</returns>
    public static double Clamp(double value, double min, double max) => Math.Max(min, Math.Min(max, value));

    /// <summary>
    /// Calculates the normalized projection factor of a point onto a line segment defined by two endpoints.
    /// </summary>
    /// <remarks>If the segment endpoints are identical, the method returns 0. The returned value can be used
    /// to interpolate between the segment endpoints or to determine if the projection falls within the
    /// segment.</remarks>
    /// <param name="px">The X coordinate of the point to project.</param>
    /// <param name="py">The Y coordinate of the point to project.</param>
    /// <param name="x1">The X coordinate of the first endpoint of the segment.</param>
    /// <param name="y1">The Y coordinate of the first endpoint of the segment.</param>
    /// <param name="x2">The X coordinate of the second endpoint of the segment.</param>
    /// <param name="y2">The Y coordinate of the second endpoint of the segment.</param>
    /// <returns>A double value representing the relative position of the projected point along the segment: 0 corresponds to the
    /// first endpoint, 1 to the second endpoint, and values between 0 and 1 indicate a position between the endpoints.</returns>
    public static double ProjectPointOnSegment(
        double px,
        double py,
        double x1,
        double y1,
        double x2,
        double y2)
    {
        var dx = x2 - x1;
        var dy = y2 - y1;

        if (dx == 0 && dy == 0)
        {
            return 0;
        }

        return ((px - x1) * dx + (py - y1) * dy) / (dx * dx + dy * dy);
    }

    /// <summary>
    /// Determines whether a line segment defined by two points intersects a circle specified by its center and squared
    /// radius.
    /// </summary>
    /// <remarks>The method checks for intersection by projecting the circle's center onto the segment and
    /// comparing the squared distance to the squared radius. This approach avoids unnecessary square root calculations
    /// and improves performance.</remarks>
    /// <param name="p1">The starting point of the line segment.</param>
    /// <param name="p2">The ending point of the line segment.</param>
    /// <param name="cx">The X-coordinate of the circle's center.</param>
    /// <param name="cy">The Y-coordinate of the circle's center.</param>
    /// <param name="radiusSq">The squared radius of the circle. Must be non-negative.</param>
    /// <returns>true if the segment intersects the circle; otherwise, false.</returns>
    public static bool SegmentIntersectsCircle(
        SignaturePoint p1,
        SignaturePoint p2,
        double cx,
        double cy,
        double radiusSq)
    {
        var dx = p2.X - p1.X;
        var dy = p2.Y - p1.Y;

        if (dx == 0 && dy == 0)
        {
            return DistanceSquared(p1.X, p1.Y, cx, cy) <= radiusSq;
        }

        var t = ProjectPointOnSegment(cx, cy, p1.X, p1.Y, p2.X, p2.Y);
        t = Clamp(t, 0, 1);

        var projX = p1.X + t * dx;
        var projY = p1.Y + t * dy;

        return DistanceSquared(projX, projY, cx, cy) <= radiusSq;
    }

    /// <summary>
    /// Calculates the velocity between two signature points based on their positions and timestamps.
    /// </summary>
    /// <remarks>The velocity is computed as the Euclidean distance between the points divided by the elapsed
    /// time in seconds. If the timestamps are identical or the current timestamp is earlier than the previous, the
    /// method returns 0.</remarks>
    /// <param name="prev">The previous signature point, representing the starting position and timestamp.</param>
    /// <param name="curr">The current signature point, representing the ending position and timestamp.</param>
    /// <returns>The velocity, in units per second, between the two points. Returns 0 if the time difference is zero or negative.</returns>
    public static double Velocity(SignaturePoint prev, SignaturePoint curr)
    {
        var dt = (curr.Timestamp - prev.Timestamp) / 1000.0;

        if (dt <= 0)
        {
            return 0;
        }

        var dx = curr.X - prev.X;
        var dy = curr.Y - prev.Y;

        return Math.Sqrt(dx * dx + dy * dy) / dt;
    }

    /// <summary>
    /// Calculates the value of a generalized sigmoid function for the specified input and steepness parameter.
    /// </summary>
    /// <remarks>This method uses a shifted and scaled sigmoid function, which is commonly used in machine
    /// learning and data normalization scenarios. The midpoint of the curve is at x = 0.5.</remarks>
    /// <param name="x">The input value for which to compute the sigmoid function.</param>
    /// <param name="k">The steepness parameter that controls the slope of the sigmoid curve. Higher values produce a steeper
    /// transition.</param>
    /// <returns>A double representing the sigmoid function value for the specified input and steepness. The result is always
    /// between 0 and 1.</returns>
    public static double Sigmoid(double x, double k) => 1.0 / (1.0 + Math.Exp(-k * (x - 0.5)));

    /// <summary>
    /// Calculates the value of a quadratic Bézier curve at the specified parameter position.
    /// </summary>
    /// <remarks>The method implements the quadratic Bézier formula for a single control point. The result is
    /// useful for smooth interpolation between values, such as in animation or graphics scenarios.</remarks>
    /// <param name="t">The parameter position along the curve, typically in the range [0, 1]. Represents the interpolation factor
    /// between the start and end points.</param>
    /// <param name="c">The control point coefficient that influences the shape of the curve.</param>
    /// <returns>The computed value of the quadratic Bézier curve at the given parameter position.</returns>
    public static double Bezier(double t, double c)
    {
        var u = 1 - t;

        return 2 * u * t * c + t * t;
    }

    /// <summary>
    /// Converts a string representation of a dash array to an array of doubles.
    /// </summary>
    /// <param name="dashArray">A string containing dash lengths separated by commas, or null to indicate no dash pattern.</param>
    /// <returns>An array of doubles representing the dash pattern, or null if the input is null or empty.</returns>
    internal static double[] ToDashArray(string? dashArray)
    {
        if (string.IsNullOrEmpty(dashArray))
        {
            return [];
        }

        return [.. dashArray.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => double.TryParse(s.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : 0)
            .Where(value => value > 0)];
    }
}

using System.Diagnostics;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Vision;

/// <summary>
/// Represents a confusion line used in color vision deficiency simulations.
/// </summary>
[DebuggerDisplay("{ToString()}")]
public readonly struct ConfusionLine
    : IEquatable<ConfusionLine>
{
    /// <summary>
    /// Represents the precomputed hash code for the confusion line, based on its properties. This allows for efficient hashing and equality checks when instances of <see cref="ConfusionLine"/> are used in collections or compared for equality.
    /// </summary>
    private readonly int hashCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfusionLine"/> class.
    /// </summary>
    /// <param name="x">The horizontal offset of the confusion line.</param>
    /// <param name="y">The vertical offset of the confusion line.</param>
    /// <param name="slope">The slope of the confusion line.</param>
    /// <param name="intercept">The intercept point of the confusion line.</param>
    internal ConfusionLine(
        double x,
        double y,
        double slope,
        double intercept)
    {
        X = x;
        Y = y;
        Slope = slope;
        Intercept = intercept;
        hashCode = HashCode.Combine(x, y, slope, intercept);
    }

    /// <summary>
    /// Gets the horizontal offset of the confusion line.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the vertical offset of the confusion line.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Gets the slope of the confusion line.
    /// </summary>
    public double Slope { get; }

    /// <summary>
    /// Gets the intercept point of the confusion line.
    /// </summary>
    public double Intercept { get; }

    /// <summary>
    /// Determines whether the specified <see cref="ConfusionLine"/> is equal to the current instance.
    /// </summary>
    /// <param name="other">The instance to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public bool Equals(ConfusionLine other)
    {
        return this == other;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is ConfusionLine other)
        {
            return this == other;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified <see cref="ConfusionLine"/> is equal to the specified <see cref="ConfusionLine"/>.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the instances are equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(ConfusionLine left, ConfusionLine right)
    {
        return left.X == right.X &&
               left.Y == right.Y &&
               left.Slope == right.Slope &&
               left.Intercept == right.Intercept;
    }

    /// <summary>
    /// Determines whether the specified <see cref="ConfusionLine"/> is not equal to the specified <see cref="ConfusionLine"/>.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns><see langword="true"/> if the instances are not equal; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(ConfusionLine left, ConfusionLine right)
    {
        return left.X != right.X ||
               left.Y != right.Y ||
               left.Slope != right.Slope ||
               left.Intercept != right.Intercept;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return hashCode;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"X={X}, Y={Y}, Slope={Slope}, Intercept={Intercept}";
    }
}

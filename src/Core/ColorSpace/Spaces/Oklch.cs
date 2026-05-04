using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the OKLCH color space, defined by lightness, chroma, and hue components.
/// </summary>
/// <remarks>The OKLCH color space is a perceptually uniform color model based on the Oklch color space, where
/// colors are specified by their lightness (L), chroma (C), and hue (H) values. This struct is immutable and provides
/// direct access to each component. The hue value is expressed in radians.</remarks>
public readonly struct Oklch(double l, double c, double h) : IEquatable<Oklch>
{
    /// <summary>
    /// Gets the L component value of the color in the CIELAB color space.
    /// </summary>
    public double L { get; } = l;

    /// <summary>
    /// Gets the value of the constant C.
    /// </summary>
    public double C { get; } = c;

    /// <summary>
    /// Gets the value of the H parameter.
    /// </summary>
    public double H { get; } = h;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Oklch other && Equals(other);

    /// <inheritdoc />
    public bool Equals(Oklch other)
    {
        return L == other.L && C == other.C && H == other.H;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(L, C, H);
    }

    /// <inheritdoc />
    public static bool operator ==(Oklch left, Oklch right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    public static bool operator !=(Oklch left, Oklch right)
    {
        return !left.Equals(right);
    }
}

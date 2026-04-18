using System.Diagnostics.CodeAnalysis;

namespace FluentUI.Blazor.Community.Components.ColorSpace.Spaces;

/// <summary>
/// Represents a color in the Oklab color space, which is designed to be perceptually uniform and is based on the CIE XYZ color space. The Oklab color space consists of three components: L (lightness), a (green-red axis), and b (blue-yellow axis).
/// </summary>
/// <param name="l">The lightness component of the Oklab color.</param>
/// <param name="a">The green-red axis component of the Oklab color.</param>
/// <param name="b">The blue-yellow axis component of the Oklab color.</param>
public readonly struct Oklab(double l, double a, double b) : IEquatable<Oklab>
{
    /// <summary>
    /// Gets the lightness component of the color in the HSL color model.
    /// </summary>
    public double L { get; } = l;

    /// <summary>
    /// Gets the green-red axis component of the color in the Oklab color model.
    /// </summary>
    public double A { get; } = a;

    /// <summary>
    /// Gets the blue-yellow axis component of the color in the Oklab color model.
    /// </summary>
    public double B { get; } = b;

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Oklab other && Equals(other);

    /// <inheritdoc />
    public bool Equals(Oklab other)
    {
        return L == other.L && A == other.A && B == other.B;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(L, A, B);
    }

    /// <inheritdoc />
    public static bool operator ==(Oklab left, Oklab right)
    {
        return left.Equals(right);
    }

    /// <inheritdoc />
    public static bool operator !=(Oklab left, Oklab right)
    {
        return !left.Equals(right);
    }
}

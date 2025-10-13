namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a generic interface for interpolating between two values of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Type of the value to interpolate.</typeparam>
public interface IInterpolator<T>
{
    /// <summary>
    /// Linearly interpolates between two values based on a weighting factor.
    /// </summary>
    /// <remarks>The behavior of the interpolation depends on the implementation of the type <typeparamref
    /// name="T"/>.  Ensure that the type supports the required operations for linear interpolation.</remarks>
    /// <param name="start">The starting value of the interpolation.</param>
    /// <param name="end">The ending value of the interpolation.</param>
    /// <param name="amount">A value between 0.0 and 1.0 that specifies the weight of the <paramref name="end"/> value.  A value of 0.0
    /// returns <paramref name="start"/>, and a value of 1.0 returns <paramref name="end"/>.</param>
    /// <returns>The interpolated value between <paramref name="start"/> and <paramref name="end"/> based on the <paramref
    /// name="amount"/>.</returns>
    T Lerp(T start, T end, double amount);
}

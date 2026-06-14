namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for interpolating between two values of type T using a specified interpolation amount.
/// </summary>
/// <remarks>Implementations of this interface provide a method to calculate an intermediate value between two
/// inputs based on a given interpolation factor. This is commonly used in animation, transitions, or blending scenarios
/// where smooth progression between values is required.</remarks>
/// <typeparam name="T">The type of values to interpolate.</typeparam>
public interface IMotionInterpolator<T>
{
    /// <summary>
    /// Interpolates between two values using a linear interpolation factor.
    /// </summary>
    /// <remarks>If amount is outside the range [0.0, 1.0], the result will extrapolate beyond the start or
    /// end values.</remarks>
    /// <param name="start">The value to interpolate from.</param>
    /// <param name="end">The value to interpolate to.</param>
    /// <param name="amount">The interpolation factor, typically between 0.0 and 1.0, where 0.0 returns the start value and 1.0 returns the
    /// end value.</param>
    /// <returns>The interpolated value between start and end, based on the specified amount.</returns>
    T Lerp(T start, T end, double amount);
}

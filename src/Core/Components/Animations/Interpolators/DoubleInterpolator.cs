namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides linear interpolation functionality for double values.
/// </summary>
public sealed class DoubleInterpolator
    : IMotionInterpolator<double>
{
    /// <inheritdoc />
    public double Lerp(double start, double end, double amount) => start + (end - start) * amount;
}

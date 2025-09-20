namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interpolator for double values, providing linear interpolation functionality.
/// </summary>
public sealed class DoubleInterpolator : IInterpolator<double>
{
    /// <inheritdoc/> 
    public double Lerp(double start, double end, double amount)
    {
        return start + (end - start) * amount;
    }
}

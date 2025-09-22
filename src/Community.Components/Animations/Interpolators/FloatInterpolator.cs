namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an interpolator for floating-point values.
/// </summary>
public sealed class FloatInterpolator : IInterpolator<float>
{
    /// <inheritdoc/> 
    public float Lerp(float start, float end, double amount)
    {
        return (float)(start + (end - start) * amount);
    }
}

using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a wave-like pattern, where the vertical position of each element is
/// determined by a sine function based on its horizontal position.
/// </summary>
/// <remarks>The layout calculates the position of each element using the specified amplitude, frequency, and
/// spacing. The horizontal position is determined by the index of the element multiplied by the spacing, while the
/// vertical position is calculated as the sine of the horizontal position scaled by the amplitude and frequency. This
/// layout is animated, and the positions of the elements are updated over time using the specified animation
/// parameters.</remarks>
public sealed class WaveLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the amplitude of the wave, which determines the height of the wave peaks and troughs.
    /// </summary>
    [Parameter]
    public double Amplitude { get; set; } = 50;

    /// <summary>
    /// Gets or sets the frequency of the wave, which determines how many wave cycles occur over a given horizontal distance.
    /// </summary>
    [Parameter]
    public double Frequency { get; set; } = 0.2;

    /// <summary>
    /// Gets or sets the spacing between consecutive elements along the horizontal axis.
    /// </summary>
    [Parameter]
    public double Spacing { get; set; } = 60;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var x = index * Spacing;
        var y = Amplitude * Math.Sin(Frequency * x);

        animatedElement.OffsetXState = CreateState(x);
        animatedElement.OffsetYState = CreateState(y);
    }
}

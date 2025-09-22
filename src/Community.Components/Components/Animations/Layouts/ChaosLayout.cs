using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that applies randomized transformations to animated elements,  creating a chaotic and dynamic
/// visual effect.
/// </summary>
/// <remarks>The <see cref="ChaosLayout"/> class randomizes the position, rotation, scale, and opacity  of
/// animated elements within a specified spread range. This layout is useful for creating  visually dynamic and
/// unpredictable animations. The randomness is applied independently  to each element, ensuring unique transformations
/// for each.  The <see cref="Spread"/> property determines the range within which the random offsets  for position are
/// calculated. The layout also applies random rotation (0 to 360 degrees),  scaling (between 0.5 and 1.5), and opacity
/// (between 0.5 and 1.0) to each element.</remarks>
public sealed class ChaosLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the spread value, which represents the difference between two related quantities.
    /// </summary>
    [Parameter]
    public double Spread { get; set; } = 200;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        animatedElement.OffsetXState = CreateState(Random.Shared.NextDouble() * Spread - Spread / 2);
        animatedElement.OffsetYState = CreateState(Random.Shared.NextDouble() * Spread - Spread / 2);
        animatedElement.RotationState = CreateState(Random.Shared.NextDouble() * 360);
        animatedElement.ScaleXState = CreateState(0.5 + Random.Shared.NextDouble());
        animatedElement.ScaleYState = CreateState(0.5 + Random.Shared.NextDouble());
        animatedElement.OpacityState = CreateState(0.5 + Random.Shared.NextDouble() * 0.5);
    }
}

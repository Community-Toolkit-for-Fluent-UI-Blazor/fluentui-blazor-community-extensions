using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout where elements float freely without a specific arrangement.
/// </summary>
public sealed class FloatLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the allowable range of drift.
    /// </summary>
    [Parameter]
    public double DriftRange { get; set; } = 20;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        animatedElement.OffsetXState = CreateState(Random.Shared.NextDouble() * DriftRange - DriftRange / 2);
        animatedElement.OffsetYState = CreateState(Random.Shared.NextDouble() * DriftRange - DriftRange / 2);
    }
}

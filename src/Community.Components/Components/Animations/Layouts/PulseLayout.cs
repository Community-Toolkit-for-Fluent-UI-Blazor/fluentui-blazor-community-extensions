using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that applies a pulsing animation to its child elements,  scaling them dynamically based on a
/// randomized pulse factor.
/// </summary>
/// <remarks>The <see cref="PulseLayout"/> class is designed to animate the scaling of child elements  by applying
/// a randomized pulse effect. The pulse scale is determined by the combination  of the <see cref="BaseScale"/> and a
/// random value scaled by <see cref="PulseScale"/>.  This layout is useful for creating dynamic, attention-grabbing
/// animations in user interfaces.</remarks>
public sealed class PulseLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the base scale factor for the pulsing animation.
    /// </summary>
    [Parameter]
    public double BaseScale { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the maximum additional scale factor for the pulsing animation.
    /// </summary>
    [Parameter]
    public double PulseScale { get; set; } = 0.2;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var pulse = BaseScale + Random.Shared.NextDouble() * PulseScale;

        animatedElement.ScaleXState = CreateState(pulse);
        animatedElement.ScaleYState = CreateState(pulse);
    }
}

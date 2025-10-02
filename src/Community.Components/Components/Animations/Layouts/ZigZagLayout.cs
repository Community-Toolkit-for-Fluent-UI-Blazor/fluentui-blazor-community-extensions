using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a zig-zag layout strategy where elements are arranged in a zig-zag pattern.
/// </summary>
public sealed class ZigZagLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the horizontal step size between elements.
    /// </summary>
    [Parameter]
    public double StepX { get; set; } = 60;

    /// <summary>
    /// Gets or sets the vertical step size between elements.
    /// </summary>
    [Parameter]
    public double StepY { get; set; } = 30;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var direction = index % 2 == 0 ? 1 : -1;

        animatedElement.OffsetXState = CreateState(index * StepX);
        animatedElement.OffsetYState = CreateState(direction * StepY);
    }
}

using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges animated elements in a spiral galaxy pattern.
/// </summary>
/// <remarks>The layout positions elements in a spiral formation, with their positions determined by the specified
/// <see cref="SpiralFactor"/> and their index in the sequence. The animation includes transitions for the X and Y
/// offsets as well as rotation, creating a dynamic spiral effect.</remarks>
public sealed class SpiralGalaxyLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpiralGalaxyLayout"/> class with default settings.
    /// </summary>
    /// <remarks>The default duration for the layout animation is set to 2.5 seconds.</remarks>
    public SpiralGalaxyLayout()
    {
        Duration = TimeSpan.FromSeconds(2.5);
    }

    /// <summary>
    /// Gets or sets the spiral factor that influences the tightness of the spiral.
    /// </summary>
    [Parameter] 
    public double SpiralFactor { get; set; } = 0.3;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var theta = index * 0.3;
        var radius = SpiralFactor * Math.Exp(0.1 * theta);
        var centerX = Width / 2;
        var centerY = Height / 2;

        var x = centerX + radius * Math.Cos(theta);
        var y = centerY + radius * Math.Sin(theta);

        animatedElement.OffsetXState = CreateState(x, centerX);
        animatedElement.OffsetYState = CreateState(y, centerY);

        animatedElement.RotationState = CreateState(MathHelper.ToDegrees(theta));
    }
}

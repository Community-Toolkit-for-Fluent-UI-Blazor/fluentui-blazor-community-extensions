using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a spiral pattern, where each element's position is determined by its
/// index, a step radius, and a step angle.
/// </summary>
/// <remarks>The layout calculates the position of each element based on polar coordinates, converting them to
/// Cartesian coordinates. The distance between elements is controlled by <see cref="RadiusStep"/>, and the angular
/// spacing is controlled by <see cref="AngleStep"/>. The layout also supports animating the position and rotation of
/// elements.</remarks>
public sealed class SpiralLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the incremental distance between each element in the spiral.
    /// </summary>
    [Parameter]
    public double RadiusStep { get; set; } = 10;

    /// <summary>
    /// Gets or sets the angular increment (in degrees) between each element in the spiral.
    /// </summary>
    [Parameter]
    public double AngleStep { get; set; } = 30;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var angle = index * AngleStep * Math.PI / 180;
        var radius = index * RadiusStep;
        var x = radius * Math.Cos(angle);
        var y = radius * Math.Sin(angle);
        var rotation = MathHelper.ToDegrees(angle);

        animatedElement.OffsetXState = CreateState(x);
        animatedElement.OffsetYState = CreateState(y);
        animatedElement.RotationState = CreateState(rotation);
    }
}

using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout strategy that arranges elements in a circular orbit around a specified center point.
/// </summary>
public sealed class OrbitLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the radius of the orbit circle.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 100;

    /// <summary>
    /// Gets or sets the X coordinate of the center point of the orbit.
    /// </summary>
    [Parameter]
    public double CenterX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the center point of the orbit.
    /// </summary>
    [Parameter]
    public double CenterY { get; set; }

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var angle = MathHelper.TwoPi * index / count;
        var x = Radius * Math.Cos(angle);
        var y = Radius * Math.Sin(angle);

        animatedElement.OffsetXState = CreateState(CenterX + x);
        animatedElement.OffsetYState = CreateState(CenterY + y);
    }
}

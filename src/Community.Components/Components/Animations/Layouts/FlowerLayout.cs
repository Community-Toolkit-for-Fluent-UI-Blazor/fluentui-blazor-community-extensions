using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges elements in a circular layout resembling the petals of a flower.
/// </summary>
/// <remarks>The layout positions elements in a circular pattern, with each element placed at an equal angular    
/// distance from its neighbors. The radius of the circle is determined as one-third of the smaller dimension of the
/// layout's width or height. The layout also applies a rotation to each element corresponding to its angular
/// position.</remarks>
public sealed class FlowerLayout
    : AnimatedLayoutBase
{
    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var radius = Math.Min(Width, Height) / 3;
        var angleStep = 360.0 / count;
        var angle = angleStep * index;
        var radians = angle * MathHelper.Radians;
        var centerX = Width / 2;
        var centerY = Height / 2;
        var x = centerX + radius * Math.Cos(radians);
        var y = centerY + radius * Math.Sin(radians);

        animatedElement.OffsetXState = CreateState(x);
        animatedElement.OffsetYState = CreateState(y);
        animatedElement.RotationState = CreateState(angle, animatedElement.Rotation);
    }
}

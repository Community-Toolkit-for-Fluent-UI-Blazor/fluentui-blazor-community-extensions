using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in the shape of a heart.
/// </summary>
/// <remarks>The <see cref="HeartLayout"/> class positions elements in a heart-shaped pattern,  with their
/// positions animated over time. The layout dynamically calculates the  position of each element based on its index and
/// the total number of elements,  ensuring that all elements are evenly distributed along the heart curve.  This layout
/// is particularly useful for visually appealing animations or  decorative UI elements. The animation parameters, such
/// as duration and easing  functions, are inherited from the base class <see cref="AnimatedLayoutBase"/>.</remarks>
public sealed class HeartLayout
    : AnimatedLayoutBase
{
    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var centerX = Width / 2;
        var centerY = Height / 2;
        var scale = Math.Min(Width, Height) / 30;
        var t = MathHelper.TwoPi * index / count;
        var x = 16 * Math.Pow(Math.Sin(t), 3);
        var y = 13 * Math.Cos(t) - 5 * Math.Cos(2 * t) - 2 * Math.Cos(3 * t) - Math.Cos(4 * t);

        animatedElement.OffsetXState = CreateState(centerX + scale * x);
        animatedElement.OffsetYState = CreateState(centerY - scale * y);
    }
}

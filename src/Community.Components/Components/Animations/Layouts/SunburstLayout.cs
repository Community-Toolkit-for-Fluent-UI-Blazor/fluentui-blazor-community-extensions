using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a sunburst pattern, where elements are positioned  radially around a
/// central point with animated transitions.
/// </summary>
/// <remarks>The <see cref="SunburstLayout"/> calculates the position of each element based on its index, 
/// distributing elements evenly around a circle. The layout supports animated transitions for  smooth movement of
/// elements to their calculated positions. The radius of each element's position  increases incrementally, creating a
/// layered, concentric effect.  This layout is particularly useful for visualizations or UI components that require a
/// radial  arrangement of elements, such as charts or menus.</remarks>
public sealed class SunburstLayout
    : AnimatedLayoutBase
{
    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var centerX = Width / 2;
        var centerY = Height / 2;
        var radiusStep = Math.Min(Width, Height) / (2 * count);
        var angleStep = 360.0 / count;
        var angle = index * angleStep * MathHelper.Radians;
        var radius = radiusStep * (index + 1);

        animatedElement.OffsetXState = CreateState(centerX + radius * Math.Cos(angle));
        animatedElement.OffsetYState = CreateState(centerY + radius * Math.Sin(angle));
    }
}

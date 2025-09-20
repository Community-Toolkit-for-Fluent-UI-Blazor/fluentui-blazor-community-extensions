using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a fan-like pattern, with configurable radius and angle spread.
/// </summary>
/// <remarks>The <see cref="FanLayout"/> positions elements in a circular arc, where the radius determines the
/// distance  from the center and the angle spread defines the total angular range covered by the elements. The layout 
/// animates the position and rotation of each element based on the specified animation parameters.</remarks>
public sealed class FanLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the radius of the fan layout, determining how far elements are positioned from the center.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 100;

    /// <summary>
    /// Gets or sets the angle spread of the fan layout in degrees, defining the total angular range covered by the elements.
    /// </summary>
    [Parameter]
    public double AngleSpread { get; set; } = 90;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var startAngle = -AngleSpread / 2;
        var step = AngleSpread / Math.Max(count - 1, 1);
        var angle = startAngle + index * step;
        var rad = MathHelper.ToRadians(angle);

        animatedElement.OffsetXState = CreateState(Radius * Math.Cos(rad));
        animatedElement.OffsetYState = CreateState(Radius * Math.Sin(rad));
        animatedElement.RotationState = CreateState(angle);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a dynamic spiral pattern, with optional rotation effects.
/// </summary>
/// <remarks>The <see cref="VortexLayout"/> arranges elements in a spiral configuration, where the spacing between
/// elements is determined by <see cref="RadiusStep"/> and <see cref="AngleStep"/>. The layout also supports dynamic
/// rotation effects based on the elapsed time, controlled by the <see cref="RotationSpeed"/> property.  This layout is
/// particularly useful for creating visually engaging animations or effects where elements follow a vortex-like
/// motion.</remarks>
public sealed class VortexLayout
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

    /// <summary>
    /// Gets or sets the rotation speed, in degrees per second.
    /// </summary>
    [Parameter]
    public double RotationSpeed { get; set; } = 90;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var elapsedSeconds = (DateTime.Now - StartTime).TotalSeconds;
        var baseAngle = index * AngleStep;
        var dynamicOffset = elapsedSeconds * RotationSpeed;
        var angle = MathHelper.ToRadians(baseAngle + dynamicOffset);
        var radius = index * RadiusStep;
        var x = radius * Math.Cos(angle);
        var y = radius * Math.Sin(angle);
        var rotation = MathHelper.ToDegrees(angle);

        animatedElement.OffsetXState = CreateState(x);
        animatedElement.OffsetYState = CreateState(y);
        animatedElement.RotationState = CreateState(rotation);
    }
}

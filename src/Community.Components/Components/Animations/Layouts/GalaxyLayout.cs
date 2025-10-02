using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a spiral pattern resembling a galaxy,  with configurable arms, spread,
/// rotation, and number of turns.
/// </summary>
/// <remarks>This layout animates the position and rotation of elements to create a dynamic galaxy-like effect.
/// The layout is defined by several parameters, including the number of arms, the spread of the elements,  the rotation
/// per turn, and the total number of turns. The animation is applied to each element's  position and rotation over
/// time.</remarks>
public sealed class GalaxyLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the number of arms in the galaxy layout.
    /// </summary>
    [Parameter]
    public int Arms { get; set; } = 3;

    /// <summary>
    /// Gets or sets the spread factor of the galaxy layout, determining how far elements are spread out from the center.
    /// </summary>
    [Parameter]
    public double Spread { get; set; } = 0.5;

    /// <summary>
    /// Gets or sets the rotation in degrees for each complete turn of the spiral.
    /// </summary>
    [Parameter]
    public double RotationPerTurn { get; set; } = 360;

    /// <summary>
    /// Gets or sets the number of turns in the galaxy layout.
    /// </summary>
    [Parameter]
    public int Turns { get; set; } = 2;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var centerX = Width / 2;
        var centerY = Height / 2;
        var t = index / (double)count;
        var armIndex = index % Arms;
        var baseAngle = armIndex * (360.0 / Arms);
        var angle = baseAngle + t * Turns * RotationPerTurn;
        var radius = Spread * t * Math.Min(Width, Height) / 2;
        var rad = angle * Math.PI / 180;

        animatedElement.OffsetXState = CreateState(centerX + radius * Math.Cos(rad));
        animatedElement.OffsetYState = CreateState(centerY + radius * Math.Sin(rad));
        animatedElement.RotationState = CreateState(angle);
    }
}

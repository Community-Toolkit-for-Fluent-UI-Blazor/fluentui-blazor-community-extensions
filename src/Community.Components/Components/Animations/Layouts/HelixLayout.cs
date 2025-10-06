using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a helical pattern, with each element positioned at an increasing angle
/// and distance from the center.
/// </summary>
/// <remarks>The <see cref="HelixLayout"/> class calculates the position and rotation of elements based on their
/// index in the sequence. Elements are arranged in a spiral-like structure, with their offsets and rotations animated
/// over time. The layout is centered within the available width and height of the container.  This layout is
/// particularly useful for creating visually dynamic animations or effects where elements appear to follow a helical
/// path.</remarks>
public sealed class HelixLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Represents the fixed angle increment, in degrees, used for calculations or operations that require discrete
    /// angular steps.
    /// </summary>
    /// <remarks>This constant defines the step size for angles, measured in degrees, and is intended for use
    /// in scenarios where angular values are processed in fixed increments, such as in geometric calculations or
    /// graphical rendering.</remarks>
    private const int AngleStep = 30;

    /// <summary>
    /// Represents the default spacing value used for layout calculations.
    /// </summary>
    private const double Spacing = 20.0;

    /// <summary>
    /// Represents the constant angle increment in radians, calculated as the product of the angle step and the radians
    /// conversion factor.
    /// </summary>
    /// <remarks>This value is used to determine the incremental change in angle for operations requiring
    /// angular calculations in radians.</remarks>
    private const double AngleIncrement = AngleStep * MathHelper.Radians;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var angle = index * AngleIncrement;
        var radius = Spacing * index;
        var centerX = Width / 2;
        var centerY = Height / 2;

        animatedElement.OffsetXState = CreateState(centerX + radius * Math.Cos(angle));
        animatedElement.OffsetYState = CreateState(centerY + radius * Math.Sin(angle));
        animatedElement.RotationState = CreateState(MathHelper.ToDegrees(angle));
    }
}

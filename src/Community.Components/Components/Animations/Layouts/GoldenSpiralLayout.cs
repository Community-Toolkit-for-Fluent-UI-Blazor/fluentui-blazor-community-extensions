using FluentUI.Blazor.Community.Helpers;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a layout strategy that arranges elements in a golden spiral pattern.
/// </summary>
/// <remarks>The golden spiral layout is based on the mathematical constant Phi (approximately 1.618), which
/// defines the proportions of the spiral. Elements are positioned in a spiral pattern radiating outward from the center
/// of the layout area. The layout animates the position and rotation of elements over a specified duration.</remarks>
public sealed class GoldenSpiralLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Represents the mathematical constant Phi (φ), also known as the golden ratio.
    /// </summary>
    /// <remarks>The value of Phi is approximately 1.61803398875. It is an irrational number that appears in
    /// various fields such as mathematics, art, architecture, and nature.</remarks>
    private const double Phi = 1.61803398875;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldenSpiralLayout"/> class.
    /// </summary>
    /// <remarks>The default duration for the layout animation is set to 2.5 seconds.</remarks>
    public GoldenSpiralLayout()
    {
        Duration = TimeSpan.FromSeconds(2.5);
    }

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var scale = Math.Min(Width, Height) / 10;

        var centerX = Width / 2;
        var centerY = Height / 2;
        var angle = index * 0.5;
        var radius = scale * Math.Pow(Phi, angle / (2 * Math.PI));

        var x = centerX + radius * Math.Cos(angle);
        var y = centerY + radius * Math.Sin(angle);

        animatedElement.OffsetXState = CreateState(x, centerX);
        animatedElement.OffsetYState = CreateState(y, centerY);
        animatedElement.RotationState = CreateState(MathHelper.ToDegrees(angle));
    }
}

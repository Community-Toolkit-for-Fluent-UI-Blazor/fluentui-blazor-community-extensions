using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child elements in a spiral pattern resembling a galaxy, distributing items along multiple arms with
/// configurable spread and rotation.
/// </summary>
/// <remarks>Use the GalaxyLayout to create visually dynamic, spiral-based arrangements for UI elements, such as
/// avatars or icons, in Blazor applications. The layout parameters allow customization of the number of arms, the
/// spread of items from the center, the rotation per turn, and the total number of spiral turns. This layout is
/// suitable for scenarios where a non-linear, decorative arrangement is desired.</remarks>
public sealed class GalaxyLayout : MotionDynamicLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GalaxyLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public GalaxyLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the number of arms to display.
    /// </summary>
    [Parameter]
    public int Arms { get; set; } = 3;

    /// <summary>
    /// Gets or sets the spread value used to adjust the spacing or distribution of elements.
    /// </summary>
    /// <remarks>The spread value typically controls the amount of space between elements in a layout or
    /// visual component. Adjust this property to increase or decrease the separation as needed for your
    /// scenario.</remarks>
    [Parameter]
    public double Spread { get; set; } = 0.5;

    /// <summary>
    /// Gets or sets the number of degrees to rotate per turn.
    /// </summary>
    /// <remarks>Use this property to control how much the component rotates for each complete turn. Adjusting
    /// this value allows customization of the rotation behavior to suit different scenarios.</remarks>
    [Parameter]
    public double RotationPerTurn { get; set; } = 360;

    /// <summary>
    /// Gets or sets the number of turns to apply for the associated operation or component.
    /// </summary>
    /// <remarks>The value determines how many times the operation will be performed or repeated. The default
    /// value is 2.</remarks>
    [Parameter]
    public int Turns { get; set; } = 2;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;
        var cx = Width / 2;
        var cy = Height / 2;
        var maxR = Math.Min(Width, Height) / 2;

        for (var i = 0; i < count; i++)
        {
            var t = i / (double)count;
            var arm = i % Arms;
            var baseAngle = arm * (360.0 / Arms);
            var angle = baseAngle + t * Turns * RotationPerTurn;
            var rad = angle * Math.PI / 180;
            var r = Spread * t * maxR;

            var target = items[i].LayoutTransition.Target;
            target.Set("x", cx + r * Math.Cos(rad));
            target.Set("y", cy + r * Math.Sin(rad));
            target.Set("r", angle);
        }
    }
}

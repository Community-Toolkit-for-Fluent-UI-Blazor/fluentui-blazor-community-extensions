using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a motion behavior that animates scaling in a smooth, cyclical pattern to simulate a breathing effect.
/// </summary>
/// <remarks>This behavior interpolates the scale of a motion item between the specified minimum and maximum
/// values over time, creating a rhythmic expansion and contraction. The speed of the animation can be adjusted to
/// control how quickly the breathing effect cycles. This class is typically used to add subtle, organic motion to UI
/// elements.</remarks>
public sealed class BreathingBehavior : MotionBehaviorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BreathingBehavior"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this component.</param>
    public BreathingBehavior(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the minimum scale value for the breathing effect.
    /// </summary>
    [Parameter]
    public double MinScale { get; set; } = 0.9;

    /// <summary>
    /// Gets or sets the maximum scale value for the breathing effect.
    /// </summary>
    [Parameter]
    public double MaxScale { get; set; } = 1.1;

    /// <summary>
    /// Gets or sets the speed multiplier for the breathing animation.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 1.0;

    /// <inheritdoc />
    protected internal override void Apply(MotionItem item, int index, TimeSpan elapsed)
    {
        var t = (Math.Sin(elapsed.TotalSeconds * Speed) + 1) / 2.0;
        var s = MinScale + (MaxScale - MinScale) * t;

        item.State.Set("sX", s);
        item.State.Set("sY", s);
    }
}


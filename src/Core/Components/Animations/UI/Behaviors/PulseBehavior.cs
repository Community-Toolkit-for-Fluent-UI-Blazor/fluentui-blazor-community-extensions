using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a motion behavior that applies a pulsing scale animation to a motion item.
/// </summary>
/// <remarks>The pulse effect is achieved by periodically adjusting the scale of the item over time, creating a
/// smooth in-and-out animation. The behavior can be customized using the BaseScale, PulseScale, and Speed parameters to
/// control the scale's baseline, amplitude, and animation speed, respectively.</remarks>
public sealed class PulseBehavior : MotionBehaviorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PulseBehavior"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this component.</param>
    public PulseBehavior(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the base scaling factor for the pulse effect.
    /// </summary>
    [Parameter]
    public double BaseScale { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the amplitude of the pulse effect.
    /// </summary>
    [Parameter]
    public double PulseScale { get; set; } = 0.2;

    /// <summary>
    /// Gets or sets the speed value of the pulse effect.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 2.0;

    /// <inheritdoc />
    protected internal override void Apply(MotionItem item, int index, TimeSpan elapsed)
    {
        var t = elapsed.TotalSeconds * Speed;
        var s = BaseScale + Math.Sin(t) * PulseScale;

        item.State.Set("sX", s);
        item.State.Set("sY", s);
    }
}


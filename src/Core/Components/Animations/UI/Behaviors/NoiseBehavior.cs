using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a motion behavior that applies a procedural noise effect to motion items, producing animated,
/// pseudo-random movement based on amplitude and speed parameters.
/// </summary>
/// <remarks>Use this behavior to create organic, non-uniform motion effects for UI elements. The noise is
/// generated using sine functions, resulting in smooth, continuous movement that varies over time and by item index.
/// This class is typically used in conjunction with motion components that support custom behaviors.</remarks>
public sealed class NoiseBehavior : MotionBehaviorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoiseBehavior"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this component.</param>
    public NoiseBehavior(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the amplitude value used by the component.
    /// </summary>
    [Parameter]
    public double Amplitude { get; set; } = 20;

    /// <summary>
    /// Gets or sets the playback speed multiplier.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 1.0;

    /// <inheritdoc />
    protected internal override void Apply(MotionItem item, int index, TimeSpan elapsed)
    {
        var t = elapsed.TotalSeconds * Speed;

        var x = Math.Sin(t + index * 1.37) * Amplitude;
        var y = Math.Sin(t * 0.7 + index * 2.11) * Amplitude;

        item.State.X = x;
        item.State.Y = y;
    }
}

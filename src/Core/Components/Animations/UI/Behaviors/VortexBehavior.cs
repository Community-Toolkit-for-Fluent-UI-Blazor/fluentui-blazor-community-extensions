using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a motion behavior that applies a vortex-like rotational effect to motion items, with configurable speed
/// and per-index offset.
/// </summary>
public sealed class VortexBehavior : MotionBehaviorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VortexBehavior"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this component.</param>
    public VortexBehavior(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the playback speed multiplier.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the offset value to apply per index when rendering the component.
    /// </summary>
    [Parameter]
    public double OffsetPerIndex { get; set; } = 0.0;

    /// <inheritdoc />
    protected internal override void Apply(MotionItem item, int index, TimeSpan elapsed)
    {
        var angle = elapsed.TotalSeconds * Speed + index * OffsetPerIndex;
        var deg = angle * 180.0 / Math.PI;

        item.State.Rotation = deg;
    }
}

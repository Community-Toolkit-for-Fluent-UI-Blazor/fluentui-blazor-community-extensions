using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a motion behavior that moves an item in a circular orbit based on the specified radius, speed, and phase
/// parameters.
/// </summary>
/// <remarks>Use this behavior to animate items along a circular path, such as for visual effects or dynamic
/// layouts. The orbit's size, speed, and starting angle can be customized through the corresponding parameters. This
/// class is typically used in conjunction with motion components that support custom behaviors.</remarks>
public sealed class OrbitBehavior : MotionBehaviorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrbitBehavior"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this component.</param>
    public OrbitBehavior(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the radius value of the orbit.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 100;

    /// <summary>
    /// Gets or sets the speed value of the orbit.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the phase offset applied to the component's calculation or rendering logic.
    /// </summary>
    [Parameter]
    public double Phase { get; set; } = 0.0;

    /// <inheritdoc />
    protected internal override void Apply(MotionItem item, int index, TimeSpan elapsed)
    {
        var angle = elapsed.TotalSeconds * Speed + index * Phase;

        var x = Math.Cos(angle) * Radius;
        var y = Math.Sin(angle) * Radius;

        item.State.X = x;
        item.State.Y = y;
    }
}


using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an animated layout that arranges items in a sunburst pattern with dynamic, pulsating motion. Suitable for
/// visualizing collections in a circular, animated arrangement within a motion layout system.
/// </summary>
/// <remarks>The layout animates item positions in a circular pattern, creating a pulsating effect by varying the
/// radius over time. The animation parameters, such as the base radius, pulse amplitude, and animation speed, can be
/// customized through the corresponding properties. This layout is typically used in scenarios where a visually
/// engaging, animated arrangement of items is desired, such as dashboards or data visualizations.</remarks>
public sealed class SunburstAnimatedLayout
    : MotionAnimatedLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SunburstAnimatedLayout"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the layout. Cannot be null.</param>
    public SunburstAnimatedLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the radius value of the sunburst layout.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 120;

    /// <summary>
    /// Gets or sets the pulse value.
    /// </summary>
    [Parameter]
    public double Pulse { get; set; } = 20;

    /// <summary>
    /// Gets or sets the speed value.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 2.0;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;
        var cx = Width / 2;
        var cy = Height / 2;

        for (var i = 0; i < count; i++)
        {
            var angle = (2 * Math.PI * i) / count;
            var r = Radius + Math.Sin(Elapsed.TotalSeconds * Speed + i * 0.3) * Pulse;

            var t = items[i].LayoutTransition.Target;
            t.Set("x", cx + Math.Cos(angle) * r);
            t.Set("y", cy + Math.Sin(angle) * r);
        }
    }
}


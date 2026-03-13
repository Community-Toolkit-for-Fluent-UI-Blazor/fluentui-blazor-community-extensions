using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child items in an animated orbit layout, positioning each item along a circular path with configurable
/// radius and speed.
/// </summary>
/// <remarks>This layout animates items in a circular orbit, with each item's position determined by its index,
/// the base radius, the step between radii, and the elapsed animation time. The layout is suitable for scenarios where
/// items should revolve smoothly around a central point, such as visualizing planets or menu items in a radial
/// arrangement. The animation speed and spacing can be customized using the provided parameters.</remarks>
public sealed class OrbitAnimatedLayout
    : MotionAnimatedLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrbitAnimatedLayout"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the layout. Cannot be null.</param>

    public OrbitAnimatedLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the base radius value used for rendering or layout calculations.
    /// </summary>
    [Parameter]
    public double BaseRadius { get; set; } = 80;

    /// <summary>
    /// Gets or sets the incremental value by which the radius increases for each step.
    /// </summary>
    [Parameter]
    public double RadiusStep { get; set; } = 20;

    /// <summary>
    /// Gets or sets the playback speed multiplier.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 1.0;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var cx = Width / 2;
        var cy = Height / 2;

        for (var i = 0; i < items.Count; i++)
        {
            var radius = BaseRadius + i * RadiusStep;
            var angle = Elapsed.TotalSeconds * Speed + i * 0.5;

            var t = items[i].LayoutTransition.Target;
            t.X = cx + Math.Cos(angle) * radius;
            t.Y = cy + Math.Sin(angle) * radius;
        }
    }
}

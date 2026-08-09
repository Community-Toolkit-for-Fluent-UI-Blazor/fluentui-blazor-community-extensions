using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges items in an animated spiral pattern, updating their positions over time based on configurable growth and
/// speed parameters.
/// </summary>
/// <remarks>This layout animates items along a spiral trajectory, with each item's position determined by its
/// index and the elapsed animation time. The spiral's expansion rate is controlled by the Growth property, while the
/// Speed property determines how quickly the spiral rotates. This layout is suitable for dynamic visualizations where
/// items should move smoothly in a spiral formation.</remarks>
public sealed class SpiralAnimatedLayout
    : MotionAnimatedLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpiralAnimatedLayout"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the layout. Cannot be null.</param>
    public SpiralAnimatedLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the growth value used to determine the incremental increase for the associated component or
    /// operation
    /// </summary>
    [Parameter]
    public double Growth { get; set; } = 5;

    /// <summary>
    /// Gets or sets the playback speed multiplier.
    /// </summary>
    /// <remarks>A value of 1.0 represents normal speed. Values greater than 1.0 increase the speed, while
    /// values less than 1.0 decrease it. Negative values may not be supported and can result in undefined behavior
    /// depending on the component's implementation.</remarks>
    [Parameter]
    public double Speed { get; set; } = 1.0;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var cx = Width / 2;
        var cy = Height / 2;

        for (var i = 0; i < items.Count; i++)
        {
            var angle = i * 0.4 + Elapsed.TotalSeconds * Speed;
            var radius = Growth * i;

            var t = items[i].LayoutTransition.Target;
            t.X = cx + Math.Cos(angle) * radius;
            t.Y = cy + Math.Sin(angle) * radius;
        }
    }
}

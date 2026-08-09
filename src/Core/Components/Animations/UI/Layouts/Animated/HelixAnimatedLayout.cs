using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides an animated layout that arranges items along a helical path, supporting configurable spacing and animation
/// speed.
/// </summary>
/// <remarks>The HelixAnimatedLayout is designed for use with motion-based UI components where items are
/// positioned in a spiral or helix pattern. The layout animates item positions over time, creating a dynamic, rotating
/// effect. Spacing and speed can be adjusted to control the distance between items and the rate of animation,
/// respectively. This layout is suitable for scenarios where a visually engaging, non-linear arrangement of items is
/// desired.</remarks>
public sealed class HelixAnimatedLayout
    : MotionAnimatedLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HelixAnimatedLayout" /> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the layout. Cannot be null.</param>
    public HelixAnimatedLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the spacing, in pixels, between elements within the component.
    /// </summary>
    /// <remarks>The default value is 20. Adjust this property to control the visual separation between child
    /// elements. Negative values may result in overlapping content.</remarks>
    [Parameter]
    public double Spacing { get; set; } = 20;

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
            var angle = i * 0.3 + Elapsed.TotalSeconds * Speed;
            var radius = Spacing * i;

            var t = items[i].LayoutTransition.Target;
            t.X = cx + radius * Math.Cos(angle);
            t.Y = cy + radius * Math.Sin(angle);
            t.Rotation = angle * 180 / Math.PI;
        }
    }
}

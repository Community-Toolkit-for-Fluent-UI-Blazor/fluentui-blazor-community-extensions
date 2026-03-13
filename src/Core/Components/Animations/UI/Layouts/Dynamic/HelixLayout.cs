using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges items in a helix pattern by positioning each item along a spiral with a fixed angular step and spacing.
/// </summary>
/// <remarks>Each item is placed at an increasing radius from the center, with its position and rotation
/// determined by its index. This layout is useful for visualizing sequences or collections in a dynamic, spiral
/// arrangement. The layout is centered within the available width and height.</remarks>
public sealed class HelixLayout : MotionDynamicLayoutBase
{
    /// <summary>
    /// Represents the fixed increment, in degrees, used for angle calculations.
    /// </summary>
    private const int AngleStep = 30;

    /// <summary>
    /// Represents the default spacing value used for layout or positioning calculations.
    /// </summary>
    private const double Spacing = 20.0;

    /// <summary>
    /// Initializes a new instance of the <see cref="HelixLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public HelixLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var cx = Width / 2;
        var cy = Height / 2;

        for (var i = 0; i < items.Count; i++)
        {
            var angle = i * AngleStep * Math.PI / 180;
            var radius = Spacing * i;

            var t = items[i].LayoutTransition.Target;
            t.Set("x", cx + radius * Math.Cos(angle));
            t.Set("y", cy + radius * Math.Sin(angle));
            t.Set("r", angle * 180 / Math.PI);
        }
    }
}


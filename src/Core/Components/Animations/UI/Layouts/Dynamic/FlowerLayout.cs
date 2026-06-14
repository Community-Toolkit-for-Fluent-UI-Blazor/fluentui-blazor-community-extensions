using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child elements in a circular, flower-like pattern with evenly distributed angles around a central point.
/// </summary>
/// <remarks>The layout positions each item at equal angular intervals on a circle centered within the available
/// space. The radius is determined by the smaller of the width or height of the layout area. This layout is useful for
/// visualizing items in a radial or decorative arrangement.</remarks>
public sealed class FlowerLayout : MotionDynamicLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FlowerLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public FlowerLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;
        var radius = Math.Min(Width, Height) / 3;
        var cx = Width / 2;
        var cy = Height / 2;
        var step = 360.0 / count;

        for (var i = 0; i < count; i++)
        {
            var angle = step * i;
            var rad = angle * Math.PI / 180;

            var t = items[i].LayoutTransition.Target;
            t.Set("x", cx + radius * Math.Cos(rad));
            t.Set("y", cy + radius * Math.Sin(rad));
            t.Set("r", angle);
        }
    }
}

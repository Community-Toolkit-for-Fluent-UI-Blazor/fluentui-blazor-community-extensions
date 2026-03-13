using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges motion items in a heart-shaped layout within the available bounds.
/// </summary>
/// <remarks>This layout positions each item along a parametric heart curve, distributing them evenly based on
/// their index. The layout automatically scales and centers the heart shape to fit the current width and height. Use
/// this layout to create visually appealing, animated heart-shaped arrangements of items.</remarks>
public sealed class HeartLayout : MotionDynamicLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HeartLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public HeartLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var cx = Width / 2;
        var cy = Height / 2;
        var scale = Math.Min(Width, Height) / 30;
        var count = items.Count;

        for (var i = 0; i < count; i++)
        {
            var t = Math.PI * 2 * i / count;
            var x = 16 * Math.Pow(Math.Sin(t), 3);
            var y = 13 * Math.Cos(t) - 5 * Math.Cos(2 * t) - 2 * Math.Cos(3 * t) - Math.Cos(4 * t);

            var target = items[i].LayoutTransition.Target;
            target.Set("x", cx + scale * x);
            target.Set("y", cy - scale * y);
        }
    }
}

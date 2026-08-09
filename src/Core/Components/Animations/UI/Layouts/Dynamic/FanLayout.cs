using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child elements in a fan-shaped layout, distributing them along an arc with a specified radius and angle
/// spread.
/// </summary>
/// <remarks>The FanLayout positions each item by calculating its angle along the arc defined by the AngleSpread
/// property, centered at the origin, and places it at the specified Radius from the center. This layout is useful for
/// visualizing items in a radial or semi-circular arrangement, such as menus or selection wheels.</remarks>
public sealed class FanLayout : MotionDynamicLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FanLayout"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this layout.</param>
    public FanLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the radius of the fan layout.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 100;

    /// <summary>
    /// Gets or sets the angle, in degrees, over which the elements are spread.
    /// </summary>
    [Parameter]
    public double AngleSpread { get; set; } = 90;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;
        var startAngle = -AngleSpread / 2;
        var step = AngleSpread / Math.Max(count - 1, 1);

        for (var i = 0; i < count; i++)
        {
            var angle = startAngle + i * step;
            var rad = angle * Math.PI / 180.0;

            var item = items[i];
            var target = item.LayoutTransition.Target;

            target.Set("x", Radius * Math.Cos(rad));
            target.Set("y", Radius * Math.Sin(rad));
            target.Set("r", angle);
        }
    }
}


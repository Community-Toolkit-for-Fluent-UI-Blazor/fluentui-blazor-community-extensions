using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child elements in a circular layout and animates their rotation over time.
/// </summary>
/// <remarks>The RotatingLayout positions each item at equal intervals around a circle, with the center determined
/// by the layout's dimensions and the distance from the center controlled by the Radius property. The layout animates
/// the rotation of items based on the Speed property and elapsed time, creating a continuous rotational motion. This
/// layout is useful for visualizations or interfaces where items should orbit a central point.</remarks>
public sealed class RotatingLayout : MotionAnimatedLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RotatingLayout"/> class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to be used for initializing the layout. Cannot be null.</param>
    public RotatingLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the radius of the circular layout.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 100;

    /// <summary>
    /// Gets or sets the playback speed multiplier.
    /// </summary>
    [Parameter]
    public double Speed { get; set; } = 1.0;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;
        var cx = Width / 2;
        var cy = Height / 2;

        for (var i = 0; i < count; i++)
        {
            var baseAngle = (2 * Math.PI * i) / count;
            var angle = baseAngle + Elapsed.TotalSeconds * Speed;

            var x = cx + Math.Cos(angle) * Radius;
            var y = cy + Math.Sin(angle) * Radius;

            var t = items[i].LayoutTransition.Target;
            t.Set("x", x);
            t.Set("y", y);
            t.Set("r", angle * 180 / Math.PI);
        }
    }
}

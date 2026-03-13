using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child motion items in a circular orbit around a specified center point.
/// </summary>
/// <remarks>Use this layout to position items evenly spaced along the circumference of a circle, such as for
/// radial menus or visualizations. The radius and center coordinates determine the size and position of the orbit.
/// Items are distributed at equal angular intervals.</remarks>
public sealed class OrbitLayout : MotionDynamicLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrbitLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public OrbitLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the radius value used for the component.
    /// </summary>
    [Parameter]
    public double Radius { get; set; } = 100;

    /// <summary>
    /// Gets or sets the horizontal center coordinate for the component.
    /// </summary>
    [Parameter]
    public double CenterX { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate of the center point for the component.
    /// </summary>
    [Parameter]
    public double CenterY { get; set; }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;

        for (var i = 0; i < count; i++)
        {
            var angle = Math.PI * 2 * i / count;

            var t = items[i].LayoutTransition.Target;
            t.Set("x", CenterX + Radius * Math.Cos(angle));
            t.Set("y", CenterY + Radius * Math.Sin(angle));
        }
    }
}


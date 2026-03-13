using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges items in a randomized, chaotic fashion, applying random positions, rotations,
/// scales, and opacities to each item.
/// </summary>
/// <remarks>Use this layout to create visually dynamic or unpredictable arrangements of items, such as for
/// creative or playful UI effects. The layout ensures that each item's transformation is consistent across layout
/// computations unless the layout is reset or the item is removed. The degree of spread for the randomization can be
/// controlled via the Spread property.</remarks>
public sealed class ChaosLayout : MotionLayoutBase
{
    /// <summary>
    /// Stores cached transformation values for each motion item, including position, rotation, scale, and opacity.
    /// </summary>
    /// <remarks>The cache enables efficient retrieval of previously computed transformation data for motion
    /// items, which can improve performance by avoiding redundant calculations.</remarks>
    private readonly Dictionary<MotionItem, (double x, double y, double r, double sx, double sy, double o)> _cache = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="ChaosLayout"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this layout.</param>
    public ChaosLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the spread value used to determine the spacing or distribution of elements.
    /// </summary>
    /// <remarks>The spread value typically controls the distance or separation between visual components.
    /// Adjust this property to increase or decrease the spacing as needed for layout customization.</remarks>
    [Parameter]
    public double Spread { get; set; } = 200;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        foreach (var item in items)
        {
            if (!_cache.TryGetValue(item, out var v))
            {
                v = (
                    x: Random.Shared.NextDouble() * Spread - Spread / 2,
                    y: Random.Shared.NextDouble() * Spread - Spread / 2,
                    r: Random.Shared.NextDouble() * 360,
                    sx: 0.5 + Random.Shared.NextDouble(),
                    sy: 0.5 + Random.Shared.NextDouble(),
                    o: 0.5 + Random.Shared.NextDouble() * 0.5
                );

                _cache[item] = v;
            }

            var target = item.LayoutTransition.Target;

            target.Set("x", v.x);
            target.Set("y", v.y);
            target.Set("r", v.r);
            target.Set("sX", v.sx);
            target.Set("sY", v.sy);
            target.Set("o", v.o);
        }
    }
}


using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges motion items around a specified magnet point, assigning each item a position near the defined coordinates.
/// </summary>
/// <remarks>This layout positions each item randomly within a fixed range around the magnet coordinates, ensuring
/// a scattered but centered distribution. The assigned positions are cached for consistency across layout computations.
/// Use this layout when you want items to cluster loosely around a central point.</remarks>
public sealed class MagnetLayout : MotionLayoutBase
{
    /// <summary>
    /// Represents a cache that stores the computed positions for each motion item.
    /// </summary>
    private readonly Dictionary<string, (double x, double y)> _cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="MagnetLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public MagnetLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the horizontal position of the magnet, typically in device-independent units.
    /// </summary>
    [Parameter]
    public double MagnetX { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate value used for magnetic alignment or snapping behavior.
    /// </summary>
    [Parameter]
    public double MagnetY { get; set; }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        foreach (var item in items)
        {
            if (!_cache.TryGetValue(item.Id!, out var v))
            {
                v = (
                    MagnetX + Random.Shared.NextDouble() * 50 - 25,
                    MagnetY + Random.Shared.NextDouble() * 50 - 25
                );

                _cache[item.Id!] = v;
            }

            var t = item.LayoutTransition.Target;
            t.Set("x", v.x);
            t.Set("y", v.y);
        }
    }
}

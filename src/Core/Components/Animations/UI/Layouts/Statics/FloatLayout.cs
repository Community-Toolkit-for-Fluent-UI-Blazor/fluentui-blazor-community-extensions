using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a layout component that positions items with a random drift within a specified range, allowing for floating
/// or scattered visual arrangements.
/// </summary>
/// <remarks>Use this component to create layouts where items appear to float or are distributed with a degree of
/// randomness. The drift range can be adjusted to control how far items may deviate from their original positions. This
/// component is suitable for scenarios requiring non-uniform, dynamic, or visually organic layouts.</remarks>
public sealed class FloatLayout : MotionLayoutBase
{
    private readonly Dictionary<string, (double x, double y)> _cache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="FloatLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public FloatLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the maximum allowed drift range for the component.
    /// </summary>
    /// <remarks>The drift range determines the extent to which the component can deviate from its original
    /// position or value. Adjust this property to control sensitivity or tolerance as needed for your
    /// scenario.</remarks>
    [Parameter]
    public double DriftRange { get; set; } = 20;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        foreach (var item in items)
        {
            if (!_cache.TryGetValue(item.Id!, out var v))
            {
                v = (
                    Random.Shared.NextDouble() * DriftRange - DriftRange / 2,
                    Random.Shared.NextDouble() * DriftRange - DriftRange / 2
                );

                _cache[item.Id!] = v;
            }

            var t = item.LayoutTransition.Target;
            t.Set("x", v.x);
            t.Set("y", v.y);
        }
    }
}

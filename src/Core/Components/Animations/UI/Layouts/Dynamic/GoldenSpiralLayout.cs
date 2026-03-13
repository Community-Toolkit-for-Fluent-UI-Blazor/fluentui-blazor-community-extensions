using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges items in a layout that follows the golden spiral pattern, positioning each item along a logarithmic spiral
/// based on the golden ratio.
/// </summary>
/// <remarks>This layout is useful for visually distributing items in a way that mimics natural spirals, such as
/// those found in shells or sunflowers. The layout centers the spiral within the available width and height, scaling it
/// to fit. Items are positioned and rotated according to their index, creating a visually appealing, expanding spiral
/// effect.</remarks>
public sealed class GoldenSpiralLayout : MotionDynamicLayoutBase
{
    /// <summary>
    /// Represents the mathematical constant φ (phi), also known as the golden ratio.
    /// </summary>
    /// <remarks>The golden ratio is an irrational number, approximately equal to 1.618, that appears in
    /// mathematics, art, and nature. It is often used in algorithms and calculations involving proportions and
    /// aesthetics.</remarks>
    private const double Phi = 1.61803398875;

    /// <summary>
    /// Initializes a new instance of the <see cref="GoldenSpiralLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public GoldenSpiralLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var cx = Width / 2;
        var cy = Height / 2;
        var scale = Math.Min(Width, Height) / 10;

        for (var i = 0; i < items.Count; i++)
        {
            var angle = i * 0.5;
            var radius = scale * Math.Pow(Phi, angle / (2 * Math.PI));

            var t = items[i].LayoutTransition.Target;
            t.Set("x", cx + radius * Math.Cos(angle));
            t.Set("y", cy + radius * Math.Sin(angle));
            t.Set("r", angle * 180 / Math.PI);
        }
    }
}

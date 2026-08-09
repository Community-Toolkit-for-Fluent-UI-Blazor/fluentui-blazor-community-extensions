using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a layout that arranges items in a wave pattern, optionally distributed along a circular or linear path with
/// animated motion.
/// </summary>
/// <remarks>Use this layout to create visually dynamic arrangements where items move in a wave-like fashion. The
/// amplitude and frequency parameters control the height and speed of the wave, while the Circular property determines
/// whether items are positioned around a circle or along a straight line. This layout is suitable for scenarios where
/// animated, non-uniform item placement enhances the user experience.</remarks>
public sealed class WaveAnimatedLayout
    : MotionAnimatedLayoutBase
{
    /// <summary>
    /// Initialise une nouvelle instance de la classe WaveAnimatedLayout avec la configuration spécifiée de la
    /// bibliothèque.
    /// </summary>
    /// <param name="configuration">La configuration de la bibliothèque à utiliser pour initialiser la disposition animée.</param>
    public WaveAnimatedLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the amplitude value.
    /// </summary>
    [Parameter]
    public double Amplitude { get; set; } = 30;

    /// <summary>
    /// Gets or sets the frequency value.
    /// </summary>
    [Parameter]
    public double Frequency { get; set; } = 2;

    /// <summary>
    /// Gets or sets a value indicating whether the items should be arranged in a circular pattern.
    /// </summary>
    [Parameter]
    public bool Circular { get; set; }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;
        var cx = Width / 2;
        var cy = Height / 2;

        for (var i = 0; i < count; i++)
        {
            var t = Elapsed.TotalSeconds * Frequency + i * 0.3;
            var wave = Math.Sin(t) * Amplitude;

            var target = items[i].LayoutTransition.Target;

            if (Circular)
            {
                var angle = (2 * Math.PI * i) / count;
                target.X = cx + Math.Cos(angle) * (100 + wave);
                target.Y = cy + Math.Sin(angle) * (100 + wave);
            }
            else
            {
                target.X = i * 40;
                target.Y = cy + wave;
            }
        }
    }
}


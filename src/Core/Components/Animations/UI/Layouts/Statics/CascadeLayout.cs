using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges motion items in a cascading layout, offsetting each item by a fixed amount along the X and Y axes.
/// </summary>
/// <remarks>The CascadeLayout positions each item so that it appears diagonally offset from the previous item,
/// creating a cascading visual effect. The amount of offset for each axis is determined by the OffsetXStep and
/// OffsetYStep properties. This layout is useful for scenarios where a staggered or layered appearance is
/// desired.</remarks>
public sealed class CascadeLayout : MotionLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CascadeLayout"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this layout.</param>
    public CascadeLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the horizontal offset increment, in pixels, applied for each step.
    /// </summary>
    [Parameter]
    public double OffsetXStep { get; set; } = 30;

    /// <summary>
    /// Gets or sets the vertical offset increment, in pixels, applied for each step.
    /// </summary>
    [Parameter]
    public double OffsetYStep { get; set; } = 30;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var target = item.LayoutTransition.Target;

            target.Set("x", i * OffsetXStep);
            target.Set("y", i * OffsetYStep);
        }
    }
}


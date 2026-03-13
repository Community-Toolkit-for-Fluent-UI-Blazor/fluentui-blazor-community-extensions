using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child motion items in a stack layout with configurable columns, spacing, offsets, and optional opacity
/// variation.
/// </summary>
/// <remarks>Use this layout to position items in a grid-like stack, specifying the number of columns and the
/// horizontal and vertical spacing between items. The layout also supports offsetting the entire stack and applying a
/// decreasing opacity to each item based on its index. This component is intended for use with motion-based UI
/// scenarios where animated transitions and layout changes are required.</remarks>
public sealed class BindStackLayout : MotionLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the BindStackLayout class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to use for initializing the component. Cannot be null.</param>
    public BindStackLayout(LibraryConfiguration configuration)
        :  base(configuration)
    { }

    /// <summary>
    /// Gets or sets the horizontal spacing between elements, in device-independent units.
    /// </summary>
    [Parameter]
    public double SpacingX { get; set; } = 20;

    /// <summary>
    /// Gets or sets the vertical spacing between elements, in device-independent units.
    /// </summary>
    [Parameter]
    public double SpacingY { get; set; } = 20;

    /// <summary>
    /// Gets or sets the horizontal offset, in pixels, applied to the component's position.
    /// </summary>
    [Parameter]
    public double OffsetX { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in device-independent units, applied to the component's position.
    /// </summary>
    [Parameter]
    public double OffsetY { get; set; }

    /// <summary>
    /// Gets or sets the opacity level applied to the variant element.
    /// </summary>
    /// <remarks>The value should be between 0.0 (fully transparent) and 1.0 (fully opaque). Values outside
    /// this range may be clamped or produce undefined results depending on the rendering logic.</remarks>
    [Parameter]
    public double VariantOpacity { get; set; } = 0.0;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;

        for (var i = 0; i < count; i++)
        {
            var item = items[i];
            var target = item.LayoutTransition.Target;

            var x = OffsetX + i * SpacingX;
            var y = OffsetY + i * SpacingY;

            target.Set("x", x);
            target.Set("y", y);

            if (VariantOpacity > 0)
            {
                var opacity = 1.0 - (i * VariantOpacity);

                if (opacity < 0)
                {
                    opacity = 0;
                }

                target.Set("o", opacity);
            }
        }
    }
}

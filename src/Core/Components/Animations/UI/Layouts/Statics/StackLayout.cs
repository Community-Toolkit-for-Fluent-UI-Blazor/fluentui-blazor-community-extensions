using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Arranges child elements in a single horizontal or vertical stack with configurable spacing, orientation, and
/// optional reversed order.
/// </summary>
/// <remarks>Use StackLayout to create linear layouts where elements are positioned sequentially along a single
/// axis. The layout supports both horizontal and vertical orientations, adjustable spacing between items, and optional
/// offset and opacity variation for each item. This component is suitable for scenarios where a simple, ordered
/// arrangement of elements is required, such as toolbars, lists, or step indicators.</remarks>
public sealed class StackLayout : MotionLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StackLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public StackLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the orientation in which the content is arranged.
    /// </summary>
    /// <remarks>Use this property to specify whether the content should be laid out vertically or
    /// horizontally. The default orientation is vertical.</remarks>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Vertical;

    /// <summary>
    /// Gets or sets the spacing value used to separate elements within the component.
    /// </summary>
    /// <remarks>The spacing is typically measured in pixels. Adjust this value to control the distance
    /// between child elements. The default value is 20.</remarks>
    [Parameter]
    public double Spacing { get; set; } = 20;

    /// <summary>
    /// Gets or sets the horizontal offset, in device-independent units, applied to the component's content.
    /// </summary>
    [Parameter]
    public double OffsetX { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset, in device-independent units, applied to the component's content.
    /// </summary>
    [Parameter]
    public double OffsetY { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component's content is displayed in reverse order.
    /// </summary>
    [Parameter]
    public bool Reversed { get; set; }

    /// <summary>
    /// Gets or sets the opacity value applied to the variant element.
    /// </summary>
    /// <remarks>The value should be between 0.0 (fully transparent) and 1.0 (fully opaque). Values outside
    /// this range may be clamped or produce undefined results depending on the rendering implementation.</remarks>
    [Parameter]
    public double VariantOpacity { get; set; } = 0.0;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;

        for (var i = 0; i < count; i++)
        {
            var index = Reversed ? (count - 1 - i) : i;
            var item = items[i];
            var target = item.LayoutTransition.Target;

            var x = OffsetX;
            var y = OffsetY;

            if (Orientation == Orientation.Horizontal)
            {
                x += index * Spacing;
            }
            else
            {
                y += index * Spacing;
            }

            target.Set("x", x);
            target.Set("y", y);

            if (VariantOpacity > 0)
            {
                var opacity = 1.0 - (index * VariantOpacity);

                if (opacity < 0)
                {
                    opacity = 0;
                }

                target.Set("o", opacity);
            }
        }
    }
}


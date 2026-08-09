using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout component that positions all motion items at a specified X and Y coordinate, effectively pinning
/// them to a single point.
/// </summary>
/// <remarks>Use this layout to align multiple items to the same location within a motion-based UI. The position
/// is determined by the values of the PinX and PinY properties. This component is typically used when a fixed placement
/// of items is required, regardless of their original positions.</remarks>
public sealed class PinLayout : MotionLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PinLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public PinLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the X-coordinate position for the pin element.
    /// </summary>
    [Parameter]
    public double PinX { get; set; }

    /// <summary>
    /// Gets or sets the Y-coordinate position for the pin element.
    /// </summary>
    [Parameter]
    public double PinY { get; set; }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        foreach (var item in items)
        {
            var t = item.LayoutTransition.Target;
            t.Set("x", PinX);
            t.Set("y", PinY);
        }
    }
}

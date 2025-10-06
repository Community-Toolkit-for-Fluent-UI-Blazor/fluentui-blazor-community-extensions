using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that positions animated elements at a fixed point defined by the <see cref="PinX"/> and <see
/// cref="PinY"/> coordinates.
/// </summary>
/// <remarks>The <see cref="PinLayout"/> class is used to anchor animated elements to a specific point within a
/// layout. The position is determined by the <see cref="PinX"/> and <see cref="PinY"/> properties, which specify the
/// horizontal and vertical coordinates, respectively.</remarks>
public sealed class PinLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the X coordinate of the pin point where elements will be positioned.
    /// </summary>
    [Parameter]
    public double PinX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the pin point where elements will be positioned.
    /// </summary>
    [Parameter]
    public double PinY { get; set; }

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        animatedElement.OffsetXState = CreateState(PinX);
        animatedElement.OffsetYState = CreateState(PinY);
    }
}

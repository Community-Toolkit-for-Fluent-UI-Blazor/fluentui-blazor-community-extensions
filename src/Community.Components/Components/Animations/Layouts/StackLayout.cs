using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges its child elements in a stack, either horizontally or vertically,  with optional
/// spacing between elements.
/// </summary>
/// <remarks>The <see cref="StackLayout"/> is a specialized layout that positions its child elements sequentially 
/// in a single line, determined by the <see cref="Orientation"/> property. The spacing between elements  can be
/// customized using the <see cref="Spacing"/> property. This layout supports animations for  transitioning child
/// elements to their new positions.</remarks>
public sealed class StackLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the spacing value used to define the distance between elements.
    /// </summary>
    [Parameter]
    public double Spacing { get; set; } = 50;

    /// <summary>
    /// Gets or sets the orientation of the layer.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        if (Orientation == Orientation.Horizontal)
        {
            animatedElement.OffsetXState = CreateState(index * Spacing);
        }
        else
        {
            animatedElement.OffsetYState = CreateState(index * Spacing);
        }
    }
}

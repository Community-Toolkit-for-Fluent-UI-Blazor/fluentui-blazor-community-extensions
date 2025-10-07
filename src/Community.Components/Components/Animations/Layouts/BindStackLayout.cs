using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that stacks elements with specified offsets and spacing, optionally reversing the order and applying variant opacity.
/// </summary>
public sealed class BindStackLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the initial horizontal offset for the stacked elements.
    /// </summary>
    [Parameter]
    public double OffsetX { get; set; }

    /// <summary>
    /// Gets or sets the initial vertical offset for the stacked elements.
    /// </summary>
    [Parameter]
    public double OffsetY { get; set; }

    /// <summary>
    /// Gets or sets the spacing between each stacked element.
    /// </summary>
    [Parameter]
    public double Spacing { get; set; } = 20;

    /// <summary>
    /// Gets or sets a value indicating whether the stacking order should be reversed.
    /// </summary>
    [Parameter]
    public bool Reversed { get; set; }

    /// <summary>
    /// Gets or sets the opacity reduction applied to each subsequent element in the stack.
    /// </summary>
    [Parameter]
    public double VariantOpacity { get; set; } = 0.05;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        animatedElement.OffsetXState = CreateState(
            Reversed ? OffsetX + (count - index) * Spacing : OffsetX + index * Spacing,
            OffsetX);

        animatedElement.OffsetYState = CreateState(
            Reversed ? OffsetY + (count - index) * Spacing : OffsetY + index * Spacing,
            OffsetY);

        animatedElement.OpacityState = CreateState(
            Reversed ? 1.0 - ((count - index - 1) * VariantOpacity) : 1.0 - (index * VariantOpacity),
            1.0);
    }
}

using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a cascading pattern, applying incremental offsets to each element
/// based on its position in the sequence.
/// </summary>
/// <remarks>The <see cref="CascadeLayout"/> class is designed to animate elements by applying horizontal and
/// vertical offsets that increase proportionally with the element's index. This layout is useful for creating visually
/// appealing staggered animations or cascading effects.</remarks>
public sealed class CascadeLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the horizontal offset step applied to each subsequent element.
    /// </summary>
    [Parameter]
    public double OffsetXStep { get; set; } = 30;

    /// <summary>
    /// Gets or sets the vertical offset step applied to each subsequent element.
    /// </summary>
    [Parameter]
    public double OffsetYStep { get; set; } = 30;

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        animatedElement.OffsetXState = CreateState(index * OffsetXStep);
        animatedElement.OffsetYState = CreateState(index * OffsetYStep);
    }
}

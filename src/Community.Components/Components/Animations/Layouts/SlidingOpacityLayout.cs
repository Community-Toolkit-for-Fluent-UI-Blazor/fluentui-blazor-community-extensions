using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a layout that animates the opacity of its child elements as they slide into or out of view.
/// </summary>
/// <remarks>Use this layout to create smooth fade-in and fade-out transitions for child elements during layout
/// changes. The animation is typically triggered when elements are added, removed, or repositioned within the
/// layout.</remarks>
public sealed class SlidingOpacityLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the initial horizontal offset for the stacked elements.
    /// </summary>
    [Parameter]
    public double? StartOffset { get; set; }

    /// <summary>
    /// Gets or sets the initial vertical offset for the stacked elements.
    /// </summary>
    [Parameter]
    public double? EndOffset { get; set; }

    /// <summary>
    /// Gets or sets the initial horizontal offset for the stacked elements.
    /// </summary>
    [Parameter]
    public double StartOpacity { get; set; } = 0.0;

    /// <summary>
    /// Gets or sets the initial vertical offset for the stacked elements.
    /// </summary>
    [Parameter]
    public double EndOpacity { get; set; } = 1.0;

    /// <summary>
    /// Gets or sets the direction in which the slide animation is performed.
    /// </summary>
    [Parameter]
    public SlideDirection Direction { get; set; } = SlideDirection.Left;

    /// <summary>
    /// Gets or sets a value indicating whether the child elements should be centered within the layout during the animation.
    /// </summary>
    [Parameter]
    public bool IsCentered { get; set; }

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        if (count > 1)
        {
            throw new InvalidOperationException($"{nameof(SlidingOpacityLayout)} supports only one child element.");
        }

        var startOffset = StartOffset ?? (Direction == SlideDirection.Left ? Width : Direction == SlideDirection.Right ? -Width : Direction == SlideDirection.Up ? Height : -Height);      
        var endOffset = EndOffset ?? (Direction == SlideDirection.Left ? 0 : Direction == SlideDirection.Right ? 0 : Direction == SlideDirection.Up ? 0 : 0);

        animatedElement.OpacityState = CreateState(EndOpacity, StartOpacity);

        if (IsCentered)
        {
            startOffset = Direction == SlideDirection.Left ? Width :
                          Direction == SlideDirection.Right ? 0 :
                          Direction == SlideDirection.Up ? Height : 0;

            endOffset = Direction == SlideDirection.Left ? Width / 2 :
                        Direction == SlideDirection.Right ? Width / 2 :
                        Direction == SlideDirection.Up ? Height / 2 : Height / 2;
        }

        if (Direction == SlideDirection.Left || Direction == SlideDirection.Right)
        {
            animatedElement.OffsetXState = CreateState(endOffset, startOffset);
        }
        else
        {
            animatedElement.OffsetYState = CreateState(endOffset, startOffset);
        }

        animatedElement.OpacityState = CreateState(EndOpacity, StartOpacity);
    }
}

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a stacked formation, applying a rotational transformation to each
/// element.
/// </summary>
/// <remarks>This layout is typically used to create visually dynamic arrangements where elements are stacked on
/// top of each other with a rotational offset. It is suitable for scenarios such as card stacks, carousel-like
/// displays, or layered visual effects.</remarks>
public sealed class StackedRotatingLayout
    : MotionLayoutBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StackedRotatingLayout"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public StackedRotatingLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var count = items.Count;
        var angleStep = 360.0 / count;

        for (var i = 0; i < count; i++)
        {
            var t = items[i].LayoutTransition.Target;
            t.Opacity = 1.0 - (i / (double)(count + 1));
            t.Rotation = angleStep * i;
        }
    }
}

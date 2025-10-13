namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout that arranges elements in a stacked formation, applying a rotational transformation to each
/// element.
/// </summary>
/// <remarks>This layout is typically used to create visually dynamic arrangements where elements are stacked on
/// top of each other with a rotational offset. It is suitable for scenarios such as card stacks, carousel-like
/// displays, or layered visual effects.</remarks>
public sealed class StackedRotatingLayout
    : AnimatedLayoutBase
{
    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        var angleStep = 360.0 / count;
        var rotationAngle = angleStep * index;

        animatedElement.RotationState = CreateState(rotationAngle);
        animatedElement.OpacityState = CreateState(1.0 - (index / (double)(count + 1)));
    }
}

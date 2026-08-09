using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for defining custom motion behaviors to be used within a MotionDynamicLayoutBase component.
/// </summary>
/// <remarks>This abstract class is intended to be inherited by components that implement specific motion
/// behaviors for items managed by a MotionDynamicLayoutBase. It ensures that the behavior is registered with its parent
/// layout and enforces correct usage by requiring a parent MotionDynamicLayoutBase in the component
/// hierarchy.</remarks>
public abstract class MotionBehaviorBase : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MotionBehaviorBase"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this component.</param>
    public MotionBehaviorBase(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the parent layout component that provides motion dynamics context for this component.
    /// </summary>
    /// <remarks>This property is typically set automatically via Blazor's cascading parameter mechanism. It
    /// allows the component to participate in coordinated motion or layout behaviors defined by the parent
    /// MotionDynamicLayoutBase instance.</remarks>
    [CascadingParameter]
    private MotionDynamicLayoutBase? Parent { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException(
                $"{GetType().Name} must be used inside a MotionDynamicLayoutBase.");
        }

        Parent.Add(this);
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        Parent?.Remove(this);

        return base.DisposeAsync();
    }

    /// <summary>
    /// Applies the current motion effect to the specified item at the given index, using the provided elapsed time.
    /// </summary>
    /// <remarks>Override this method in a derived class to implement custom motion behavior for each item
    /// based on its index and the elapsed time.</remarks>
    /// <param name="item">The motion item to which the effect is applied. Cannot be null.</param>
    /// <param name="index">The zero-based index of the item within the collection of motion items.</param>
    /// <param name="elapsed">The elapsed time since the start of the motion, used to determine the current state of the effect.</param>
    protected internal abstract void Apply(MotionItem item, int index, TimeSpan elapsed);
}


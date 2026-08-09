using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the base class for components that support dynamic layout behaviors within a motion layout context.
/// </summary>
/// <remarks>Inherit from this class to create components that allow additional behaviors or interactive content
/// to be rendered dynamically. This class extends MotionLayoutBase and provides a Behaviors property for injecting
/// custom content.</remarks>
public abstract class MotionDynamicLayoutBase : MotionLayoutBase
{
    /// <summary>
    /// Represents the collection of motion behaviors that can be applied to this layout. 
    /// </summary>
    private readonly List<MotionBehaviorBase> _behaviors = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="MotionDynamicLayoutBase"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component.</param>
    public MotionDynamicLayoutBase(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the collection of additional behaviors to render within the component.
    /// </summary>
    /// <remarks>Use this property to provide custom content or interactive elements that extend the
    /// component's functionality. The specified content is rendered in the context of the component and can include
    /// other Blazor components or markup as needed.</remarks>
    [Parameter]
    public RenderFragment? Behaviors { get; set; }

    /// <summary>
    /// Gets the collection of motion behaviors associated with this instance.
    /// </summary>
    /// <remarks>The returned list provides read-only access to the motion behaviors currently configured.
    /// Modifications to the collection must be performed through the appropriate methods on the containing
    /// class.</remarks>
    internal IReadOnlyList<MotionBehaviorBase> MotionBehaviors => _behaviors;

    /// <summary>
    /// Adds the specified motion behavior to the collection if it is not already present.
    /// </summary>
    /// <param name="motionBehavior">The motion behavior to add to the collection. Cannot be null.</param>
    internal void Add(MotionBehaviorBase motionBehavior)
    {
        if (!_behaviors.Contains(motionBehavior))
        {
            _behaviors.Add(motionBehavior);
        }
    }

    /// <summary>
    /// Removes the specified motion behavior from the collection of behaviors.
    /// </summary>
    /// <param name="motionBehavior">The motion behavior to remove from the collection. Cannot be null.</param>
    internal void Remove(MotionBehaviorBase motionBehavior)
    {
        _behaviors.Remove(motionBehavior);
    }
}

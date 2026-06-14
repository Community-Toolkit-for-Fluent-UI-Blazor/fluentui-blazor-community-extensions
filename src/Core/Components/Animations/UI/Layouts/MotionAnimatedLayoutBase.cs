using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Serves as the base class for layouts that provide motion-based animations for their child components.
/// </summary>
/// <remarks>Inherit from this class to implement custom animated layouts that apply motion effects to their
/// content. This class extends MotionDynamicLayoutBase, enabling advanced animation scenarios in UI
/// components.</remarks>
public abstract class MotionAnimatedLayoutBase : MotionDynamicLayoutBase
{
    /// <summary>
    /// Represents the total elapsed time since the layout started animating.
    /// </summary>
    private TimeSpan _elapsed;

    /// <summary>
    /// Initializes a new instance of the <see cref="MotionAnimatedLayoutBase"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component.</param>
    public MotionAnimatedLayoutBase(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets the total elapsed time measured by the current instance.
    /// </summary>
    protected TimeSpan Elapsed => _elapsed;

    /// <summary>
    /// Invoked to perform periodic processing based on the elapsed time since the last tick.
    /// </summary>
    /// <remarks>Override this method in a derived class to implement custom logic that should execute on each
    /// tick. This method is typically called by a timer or scheduler to update state or perform recurring
    /// actions.</remarks>
    /// <param name="elapsed">The time interval that has elapsed since the previous tick. Represents the duration to account for in periodic
    /// operations.</param>
    internal void OnTick(TimeSpan elapsed)
    {
        _elapsed += elapsed;
    }
}

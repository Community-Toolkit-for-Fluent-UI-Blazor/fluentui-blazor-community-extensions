using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a container for managing and coordinating a group of motion items within a Fluent UI Blazor component.
/// </summary>
/// <remarks>Use this class to group multiple motion items so that their animations or transitions can be managed
/// collectively. The group propagates timing updates to all contained items, enabling synchronized motion effects. This
/// class is typically used as a parent component for motion-related child components in a Blazor application.</remarks>
public sealed partial class MotionGroup : MotionNode
{
    /// <summary>
    /// Represents the width of the component.
    /// </summary>
    private double _width;

    /// <summary>
    /// Represents the height of the component.
    /// </summary>
    private double _height;

    /// <summary>
    /// Represents the collection of motion items that are part of this motion group. Each item in this list is expected to
    /// </summary>
    private readonly List<MotionItem> _items = [];

    /// <summary>
    /// Represents the set of motion items that were present during the previous render cycle.
    /// </summary>
    private readonly HashSet<MotionItem> _previous = [];

    /// <summary>
    /// Contains the collection of motion items that are currently in the process of exiting.
    /// </summary>
    private readonly List<MotionItem> _exitingItems = [];

    /// <summary>
    /// Represents the layout engine used to manage motion-based layout operations.
    /// </summary>
    private IMotionLayout? _layout;

    /// <summary>
    /// Represents the amount of elapsed time.
    /// </summary>
    private TimeSpan _elapsed = TimeSpan.Zero;

    /// <summary>
    /// Initializes a new instance of the <see cref="MotionGroup"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this motion group.</param>
    public MotionGroup(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent FluentCxMotion component that provides cascading values to child components.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a FluentCxMotion context. It enables child components to access shared motion-related configuration
    /// or state from their parent.</remarks>
    [CascadingParameter]
    private FluentCxMotion? Parent { get; set; }

    /// <summary>
    /// Represents the initial state of the motion items when they are first rendered.
    /// </summary>
    [Parameter]
    public MotionVariant? Initial { get; set; }

    /// <summary>
    /// Gets or sets the animation variant to apply to the component's content.
    /// </summary>
    /// <remarks>Set this property to specify a predefined animation style for the component. If not set, the
    /// component will use its default animation behavior. The available variants are defined by the MotionVariant
    /// enumeration.</remarks>
    [Parameter]
    public MotionVariant? Animate { get; set; }

    /// <summary>
    /// Gets or sets the motion variant to use when the component is removed from view.
    /// </summary>
    [Parameter]
    public MotionVariant? Exit { get; set; }

    /// <summary>
    /// Gets or sets the transition animation to apply when the component's visibility changes.
    /// </summary>
    [Parameter]
    public MotionTransition? Transition { get; set; }

    /// <summary>
    /// Gets or sets the time interval to wait before starting the animation of each child element in sequence.
    /// </summary>
    /// <remarks>Use this property to create a staggered animation effect, where each child element begins its
    /// animation after the specified delay. This can enhance the visual appearance of sequentially animated
    /// components.</remarks>
    [Parameter]
    public TimeSpan StaggerChildren { get; set; }

    /// <summary>
    /// Gets or sets the delay applied before rendering child components.
    /// </summary>
    /// <remarks>Use this property to introduce a delay before child elements are displayed, which can be
    /// useful for coordinating animations or staged rendering scenarios.</remarks>
    [Parameter]
    public TimeSpan DelayChildren { get; set; }

    /// <summary>
    /// Gets or sets the direction in which the staggered animation is applied.
    /// </summary>
    /// <remarks>A positive value applies the stagger in the forward direction, while a negative value applies
    /// it in reverse. The default value is 1, indicating a forward direction.</remarks>
    [Parameter]
    public StaggerDirection StaggerDirection { get; set; } = StaggerDirection.Forward;

    /// <summary>
    /// Gets or sets the timing for when the motion group should be executed relative to its child elements.
    /// </summary>
    [Parameter]
    public MotionGroupOrder When { get; set; } = MotionGroupOrder.BeforeChildren;

    /// <summary>
    /// Gets or sets the render fragment used to define the layout strategy for motion calculations.
    /// </summary>
    [Parameter]
    public RenderFragment? Layout { get; set; }

    /// <summary>
    /// Adds the specified motion item to the collection if it is not already present.
    /// </summary>
    /// <param name="item">The motion item to add to the collection. Cannot be null.</param>
    internal void Register(MotionItem item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
        }
    }

    /// <summary>
    /// Removes the specified item from the collection.
    /// </summary>
    /// <param name="item">The item to remove from the collection. Cannot be null.</param>
    internal void Unregister(MotionItem item)
    {
        if (Exit is not null && Transition is not null)
        {
            _exitingItems.Add(item);
            _ = AnimateExitAsync(item);
        }

        _items.Remove(item);
    }

    /// <summary>
    /// Advances the state of all managed items by the specified time interval.
    /// </summary>
    /// <param name="delta">The amount of time to advance each item, represented as a <see cref="TimeSpan"/>.</param>
    internal override void OnTick(TimeSpan delta)
    {
        base.OnTick(delta);

        foreach (var item in _items)
        {
            item.OnTick(delta);
        }

        foreach (var item in _exitingItems)
        {
            item.OnTick(delta);
        }

        if (_layout is not null &&
            Transition is not null)
        {
            SnapshotLayoutSources();
            ApplyAnimation(delta);
            ApplyLayout();
            UpdateLayoutTracks();
            ApplyBehaviors(delta);
        }

        foreach (var item in _items)
        {
            InvokeAsync(item.ApplyStyleAsync);
        }
    }

    /// <summary>
    /// Applies an animation update to the current layout using the specified time interval.
    /// </summary>
    /// <remarks>This method updates the animation state of the layout if it supports motion-based animations.
    /// It should be called regularly, such as within a timer or rendering loop, to ensure smooth animation
    /// progression.</remarks>
    /// <param name="delta">The time interval since the last animation update. Specifies how much time has elapsed and is used to advance
    /// the animation state.</param>
    private void ApplyAnimation(TimeSpan delta)
    {
        if (_layout is MotionAnimatedLayoutBase l)
        {
            l.OnTick(delta);
        }
    }

    /// <summary>
    /// Apply all motion behaviors defined in the layout to the collection of items based on the elapsed time.
    /// </summary>
    /// <param name="delta"></param>
    private void ApplyBehaviors(TimeSpan delta)
    {
        if (_layout is not MotionDynamicLayoutBase layout)
        {
            return;
        }

        _elapsed += delta;

        for (var i = 0; i < _items.Count; i++)
        {
            var item = _items[i];

            foreach (var behavior in layout.MotionBehaviors)
            {
                behavior.Apply(item, i, _elapsed);
            }
        }
    }

    /// <summary>
    /// Updates the layout animation tracks for all items whose layout has changed and whose timeline is not currently
    /// running.
    /// </summary>
    /// <remarks>This method clears and recreates animation tracks for items that require a layout transition,
    /// then starts their timelines. Items with running timelines or unchanged layouts are not affected.</remarks>
    private void UpdateLayoutTracks()
    {
        if (Transition is null)
        {
            return;
        }

        foreach (var item in _items)
        {
            if (item.Timeline.State == MotionTimelineState.Running)
            {
                continue;
            }

            if (!HasLayoutChanged(item))
            {
                continue;
            }

            item.Timeline.ClearTracks();

            foreach (var key in item.LayoutTransition.Target.Keys)
            {
                var from = item.LayoutTransition.Source.GetOrDefault<double>(key);
                var to = item.LayoutTransition.Target.GetOrDefault<double>(key);

                var track = new MotionTrack<double>(
                    property: key,
                    state: item.State,
                    startValue: from,
                    endValue: to,
                    curve: Transition.ToCurve(),
                    interpolator: new DoubleInterpolator(),
                    delay: Transition.Delay
                );

                item.Timeline.AddTrack(track);
            }

            item.Timeline.Start();
        }
    }

    /// <summary>
    /// Determines whether the layout transition of the specified motion item has changed between its source and target
    /// states.
    /// </summary>
    /// <remarks>Use this method to detect whether a layout update is necessary based on changes in the item's
    /// layout transition properties.</remarks>
    /// <param name="item">The motion item whose layout transition is evaluated for changes.</param>
    /// <returns>true if any layout property value differs between the source and target states; otherwise, false.</returns>
    private static bool HasLayoutChanged(MotionItem item)
    {
        var source = item.LayoutTransition.Source;
        var target = item.LayoutTransition.Target;

        foreach (var key in target.Keys)
        {
            var a = source.GetOrDefault<double>(key);
            var b = target.GetOrDefault<double>(key);

            if (!a.Equals(b))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException(
                $"{nameof(MotionGroup)} must be placed inside {nameof(FluentCxMotion)}.");
        }

        Parent.Register(this);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (Initial is not null && Transition is not null)
            {
                foreach (var item in _items)
                {
                    await item.Actions.ToVariantAsync(Initial, Transition);
                }
            }

            if (Animate is not null && Transition is not null)
            {
                if (When == MotionGroupOrder.BeforeChildren)
                {
                    await AnimateGroupAsync();
                    await ApplyStaggerAsync(Animate, Transition);
                }
                else
                {
                    await ApplyStaggerAsync(Animate, Transition);
                    await AnimateGroupAsync();
                }
            }

            foreach (var item in _items)
            {
                _previous.Add(item);
            }

            return;
        }

        var removed = _previous.Except(_items).ToList();

        foreach (var item in removed)
        {
            if (Exit is not null && Transition is not null)
            {
                _exitingItems.Add(item);
                _ = AnimateExitAsync(item);
            }
        }

        _previous.Clear();

        foreach (var item in _items)
        {
            _previous.Add(item);
        }
    }

    /// <summary>
    /// Performs an asynchronous animation on the group using the specified animation and transition settings, if
    /// available.
    /// </summary>
    /// <remarks>The animation is only performed if both the Actions and Transition properties are not null.
    /// If either is null, the method completes without performing any action.</remarks>
    /// <returns>A task that represents the asynchronous animation operation.</returns>
    private async Task AnimateGroupAsync()
    {
        if (Animate is null || Transition is null)
        {
            return;
        }

        await Actions.ToVariantAsync(Animate, Transition);
    }

    /// <summary>
    /// Animates the exit transition for the specified motion item and updates the component state.
    /// </summary>
    /// <remarks>This method removes the item from the exiting items collection after the animation completes
    /// and triggers a state update. Use this method to ensure exit animations are properly handled before the item is
    /// removed from the UI.</remarks>
    /// <param name="item">The motion item to animate during the exit transition. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation of animating the exit transition.</returns>
    private async Task AnimateExitAsync(MotionItem item)
    {
        await item.Actions.ToVariantAsync(Exit!, Transition!);

        _exitingItems.Remove(item);
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Apply a staggered animation to the child items based on the specified motion variant and transition.
    /// </summary>
    /// <param name="variant">Variant to which the child items should animate. This defines the target state for the animation.</param>
    /// <param name="transition">Transition to apply when animating the child items. This defines the timing and easing of the animation.</param>
    /// <returns>Returns a task that represents the asynchronous operation of applying the staggered animation to the child items.</returns>
    private async Task ApplyStaggerAsync(MotionVariant variant, MotionTransition transition)
    {
        if (_items.Count == 0)
        {
            return;
        }

        if (StaggerDirection == StaggerDirection.Backward)
        {
            _items.Reverse();
        }

        var delay = DelayChildren;

        foreach (var item in _items)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(delay);
                await item.Actions.ToVariantAsync(variant, transition);
            });

            delay += StaggerChildren;
        }
    }

    /// <summary>
    /// Sets the layout strategy used for motion calculations.
    /// </summary>
    /// <param name="layout">The layout strategy to apply.</param>
    internal void SetLayout(IMotionLayout? layout)
    {
        if (_layout != layout)
        {
            foreach (var item in _items)
            {
                item.ResetTransition();
            }

            _layout = layout;
            _layout?.SetDimensions(_width, _height);
        }
    }

    /// <summary>
    /// Captures the current motion state of each item and stores it in the corresponding layout transition source.
    /// </summary>
    /// <remarks>Call this method to take a snapshot of all items' motion states before performing layout
    /// transitions. This ensures that the source state is available for transition calculations or
    /// animations.</remarks>
    private void SnapshotLayoutSources()
    {
        foreach (var item in _items)
        {
            CopyMotionState(item.State, item.LayoutTransition.Source);
        }
    }

    /// <summary>
    /// Applies the current layout to the collection of items.
    /// </summary>
    /// <remarks>This method computes the layout for the items using the configured layout strategy. If no
    /// layout is set, the method performs no action.</remarks>
    private void ApplyLayout()
    {
        if (_layout is null)
        {
            return;
        }

        _layout.ComputeLayout(_items);
    }

    /// <summary>
    /// Copies all key-value pairs from the specified source motion state to the target motion state.
    /// </summary>
    /// <remarks>Existing values in the target motion state with matching keys will be overwritten by values
    /// from the source.</remarks>
    /// <param name="from">The source MotionState instance from which key-value pairs are copied.</param>
    /// <param name="to">The target MotionState instance to which key-value pairs are copied.</param>
    private static void CopyMotionState(MotionState from, MotionState to)
    {
        foreach (var key in from.Keys)
        {
            to.Set(key, from.GetOrDefault<object>(key));
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        _items.Clear();
        _exitingItems.Clear();
        _previous.Clear();
        SetLayout(null);

        await Parent!.UnregisterResizeObserverAsync(Id);

        await base.DisposeAsync();
    }

    /// <summary>
    /// Sets the width and height dimensions for the layout.
    /// </summary>
    /// <param name="width">The width to set for the layout, in device-independent units.</param>
    /// <param name="height">The height to set for the layout, in device-independent units.</param>
    internal void SetDimensions(double width, double height)
    {
        _width = width;
        _height = height;

        _layout?.SetDimensions(width, height);
    }
}

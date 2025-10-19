using System.Collections.Concurrent;
using FluentUI.Blazor.Community.Components;

namespace FluentUI.Blazor.Community.Animations;

/// <summary>
/// Represents the core engine responsible for managing and updating animated elements and groups.
/// </summary>
public sealed class AnimationEngine
{
    /// <summary>
    /// Represents the maximum number of items to be displayed in the animation.
    /// </summary>
    private int _maxDisplayedItems = 10;

    /// <summary>
    /// Current state of the animation engine.
    /// </summary>
    private AnimationEngineState _state = AnimationEngineState.NotStarted;

    /// <summary>
    /// Represents the previous state of animated elements to compute differences during animation frames.
    /// </summary>
    private readonly Dictionary<string, AnimatedElement> _previousElements = [];

    /// <summary>
    /// Represents the currently registered animated elements.
    /// </summary>
    private readonly List<AnimatedElement> _elements = [];

    /// <summary>
    /// Represents the currently registered animation groups.
    /// </summary>
    private readonly List<AnimatedElementGroup> _groups = [];

    /// <summary>
    /// Represents the layout strategy used to arrange animated elements.
    /// </summary>
    private ILayoutStrategy? _layoutStrategy;

    /// <summary>
    /// A thread-safe queue that stores instances of <see cref="JsonAnimatedElement"/>.
    /// </summary>
    /// <remarks>This queue is used to manage <see cref="JsonAnimatedElement"/> objects in a concurrent
    /// environment,  ensuring safe access and modification across multiple threads.</remarks>
    private readonly ConcurrentQueue<JsonAnimatedElement> jsonAnimatedElements = new();

    /// <summary>
    /// Gets or sets the event that is triggered when the state of the animation engine changes.
    /// </summary>
    public event EventHandler<AnimationEngineState>? StateChanged;

    /// <summary>
    /// Gets the current state of the animation engine.
    /// </summary>
    public AnimationEngineState State => _state;

    /// <summary>
    /// Registers an animated element for tracking and ensures it is added to the collection.
    /// </summary>
    /// <remarks>If the specified element is already registered, the method does nothing.  Otherwise, the
    /// element is added to the internal collection, and a clone of the element is stored for future
    /// reference.</remarks>
    /// <param name="item">The animated element to register. Cannot be <see langword="null"/>.</param>
    internal void Register(AnimatedElement item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (_elements.Contains(item))
        {
            return;
        }

        _elements.Add(item);
        _previousElements.Add(item.Id!, item.Clone());
    }

    /// <summary>
    /// Registers an animation group and stores a snapshot of its animated elements.
    /// </summary>
    /// <remarks>This method adds the specified animation group to the internal collection of groups. 
    /// Additionally, it creates and stores a snapshot of the current state of each animated element  within the group,
    /// keyed by their unique identifier.</remarks>
    /// <param name="group">The animation group to register. Must not be <see langword="null"/>.</param>
    internal void RegisterGroup(AnimatedElementGroup group)
    {
        _groups.Add(group);

        foreach (var item in group.AnimatedElements)
        {
            _previousElements.Add(item.Id!, item.Clone());
        }
    }

    /// <summary>
    /// Sets the layout strategy to be used for arranging items.
    /// </summary>
    /// <param name="layoutStrategy">The layout strategy to apply. If <see langword="null"/>, a default grid layout is used.</param>
    internal void SetLayout(ILayoutStrategy? layoutStrategy)
    {
        _layoutStrategy = layoutStrategy ?? new BindStackLayout();
        _elements.ForEach(e => e.ResetStates());
        _previousElements.Values.ToList().ForEach(e => e.ResetStates());
    }

    /// <summary>
    /// Unregisters the specified animated element, removing it from the collection of tracked elements.
    /// </summary>
    /// <param name="item">The animated element to unregister. Cannot be <see langword="null"/>.</param>
    internal void Unregister(AnimatedElement item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _previousElements.Remove(item.Id!);
        _elements.RemoveAll(x => x.Id == item.Id);
    }

    /// <summary>
    /// Unregisters the specified animation group, removing it and its associated animated elements from the internal
    /// collections.
    /// </summary>
    /// <remarks>This method removes the animation group from the internal collection of groups and also
    /// removes all animated elements associated with the group from the internal tracking collection.</remarks>
    /// <param name="group">The animation group to unregister. This parameter cannot be <see langword="null"/>.</param>
    internal void UnregisterGroup(AnimatedElementGroup group)
    {
        _groups.Remove(group);

        foreach (var item in group.AnimatedElements)
        {
            _previousElements.Remove(item.Id!);
        }
    }

    /// <summary>
    /// Updates the state of animated elements and returns a list of the updated elements.
    /// </summary>
    /// <remarks>This method processes both grouped and non-grouped animated elements, updating their state
    /// based on the current time.</remarks>
    /// <returns>A list of <see cref="JsonAnimatedElement"/> objects representing the updated animated elements.</returns>
    internal List<JsonAnimatedElement> Update()
    {
        var now = DateTime.Now;

        jsonAnimatedElements.Clear();
        UpdateNoGroupElements(now, jsonAnimatedElements);
        UpdateGroupElements(now, jsonAnimatedElements);

        return [.. jsonAnimatedElements];
    }

    /// <summary>
    /// Updates the elements of all groups by applying the layout strategy and calculating the differences between the
    /// current and previous states.
    /// </summary>
    /// <remarks>This method processes all groups in parallel. For each group, it applies the layout strategy
    /// and computes the differences between the current state and the previous state. The calculated differences are
    /// added to the provided <paramref name="jsonAnimatedElements"/> queue.</remarks>
    /// <param name="now">The current timestamp used to calculate differences in the group elements.</param>
    /// <param name="jsonAnimatedElements">A thread-safe queue to which the method enqueues the calculated differences as <see cref="JsonAnimatedElement"/>
    /// objects.</param>
    private void UpdateGroupElements(DateTime now, ConcurrentQueue<JsonAnimatedElement> jsonAnimatedElements)
    {
        if (_groups.Count > 0)
        {
            Parallel.ForEach(_groups, group =>
            {
                group.ApplyLayout(_layoutStrategy);
                group.GetDiff(now, jsonAnimatedElements, _previousElements);
            });
        }
    }

    /// <summary>
    /// Updates the state of elements that are not part of a group and enqueues their changes into the specified queue
    /// if any differences are detected.
    /// </summary>
    /// <remarks>This method applies the layout strategy to the elements, updates their state based on the
    /// current time, and calculates the differences between their current and previous states. If differences are
    /// found, the changes are serialized into a <see cref="JsonAnimatedElement" /> object and added to the
    /// queue.</remarks>
    /// <param name="now">The current timestamp used to update the elements.</param>
    /// <param name="queue">A thread-safe queue to which the updated elements, represented as <see cref="JsonAnimatedElement" />, are enqueued
    /// if changes are detected.</param>
    private void UpdateNoGroupElements(DateTime now, ConcurrentQueue<JsonAnimatedElement> queue)
    {
        if (_layoutStrategy is not null &&
            _elements.Count > 0)
        {
            var elementsToUpdate = _elements.Count > _maxDisplayedItems
                ? _elements[.._maxDisplayedItems]
                : _elements;

            _layoutStrategy.ApplyLayout(elementsToUpdate);

            Parallel.ForEach(elementsToUpdate, element =>
            {
                element.Update(now);
                var diff = element.GetDiff(_previousElements[element.Id]);

                if (diff.Count > 0)
                {
                    diff["id"] = element.Id;
                    _previousElements[element.Id] = element.Clone();

                    queue.Enqueue(new JsonAnimatedElement()
                    {
                        Id = element.Id,
                        Opacity = diff.TryGetValue("opacity", out var value) ? (double?)value : null,
                        X = diff.TryGetValue("offsetX", out value) ? (double?)value : null,
                        Y = diff.TryGetValue("offsetY", out value) ? (double?)value : null,
                        ScaleX = diff.TryGetValue("scaleX", out value) ? (double?)value : null,
                        ScaleY = diff.TryGetValue("scaleY", out value) ? (double?)value : null,
                        Rotation = diff.TryGetValue("rotation", out value) ? (double?)value : null,
                        BackgroundColor = diff.TryGetValue("backgroundColor", out value) ? (string?)value : null,
                        Color = diff.TryGetValue("color", out value) ? (string?)value : null,
                        Value = diff.TryGetValue("value", out value) ? (double?)value : null
                    });
                }
            });
        }
    }

    /// <summary>
    /// Applies the specified start time to the layout strategy and all groups within the collection.
    /// </summary>
    /// <param name="now">The start time to apply, typically representing the current date and time.</param>
    internal void ApplyStartTime(DateTime now)
    {
        _layoutStrategy?.ApplyStartTime(now);

        foreach (var group in _groups)
        {
            group.ApplyStartTime(now);
        }
    }

    /// <summary>
    /// Sets the maximum number of items to be displayed.
    /// </summary>
    /// <remarks>This method updates the internal limit on the number of items that can be displayed at once.
    /// Ensure that the value provided is within the acceptable range for your application.</remarks>
    /// <param name="maxDisplayedItems">The maximum number of items to display. Must be a non-negative integer.</param>
    internal void SetMaxDisplayedItems(int maxDisplayedItems)
    {
        _maxDisplayedItems = maxDisplayedItems;

        foreach (var group in _groups)
        {
            group.SetMaxDisplayedItems(maxDisplayedItems);
        }
    }

    /// <summary>
    /// Returns a distinct list of all registered animated element IDs, including those within groups.
    /// </summary>
    /// <returns></returns>
    internal IEnumerable<string> GetAll()
    {
        return [.. _groups.SelectMany(g => g.AnimatedElements).Select(e => e.Id!).Concat(_elements.Select(e => e.Id!)).Distinct()];
    }

    /// <summary>
    /// Pauses the animation engine, transitioning it to the paused state.
    /// </summary>
    /// <remarks>After calling this method, the animation engine will halt progression until resumed. This
    /// method has no effect if the engine is already paused.</remarks>
    internal void Pause()
    {
        _state = AnimationEngineState.Paused;
        StateChanged?.Invoke(this, _state);
    }

    /// <summary>
    /// Resets the animation engine to its initial state and notifies listeners of the state change.
    /// </summary>
    /// <remarks>This method sets the engine state to NotStarted and raises the StateChanged event. Use this
    /// method to reinitialize the animation engine before starting a new animation sequence.</remarks>
    internal void Reset()
    {
        _state = AnimationEngineState.NotStarted;
        StateChanged?.Invoke(this, _state);
    }

    /// <summary>
    /// Marks the animation engine as completed, updating its state accordingly.
    /// </summary>
    internal void OnCompleted()
    {
        _state = AnimationEngineState.Completed;
        StateChanged?.Invoke(this, _state);
    }

    /// <summary>
    /// Resumes the animation engine, transitioning it to the active state if it is currently paused or stopped.
    /// </summary>
    /// <remarks>This method has no effect if the engine is already running. Calling this method after a pause
    /// or stop allows animations to continue from their current position.</remarks>
    internal void Resume()
    {
        _state = AnimationEngineState.Running;
        StateChanged?.Invoke(this, _state);
    }

    /// <summary>
    /// Stops the animation engine and transitions its state to stopped.
    /// </summary>
    internal void Stop()
    {
        _state = AnimationEngineState.Stopped;
        StateChanged?.Invoke(this, _state);
    }

    /// <summary>
    /// Transitions the animation engine to the running state, allowing animations to begin or resume processing.
    /// </summary>
    internal void Start()
    {
        _state = AnimationEngineState.Running;
        StateChanged?.Invoke(this, _state);
    }
}

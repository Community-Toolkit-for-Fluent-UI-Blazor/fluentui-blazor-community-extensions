using System.Collections.Concurrent;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a group of animated elements that can be laid out and animated together.
/// </summary>
public class AnimatedElementGroup
{
    /// <summary>
    /// Represents the currently displayed animated elements in the group.
    /// </summary>
    private readonly List<AnimatedElement> _displayedItem = [];

    /// <summary>
    /// Gets or sets the unique identifier for the animated element group.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// Gets the list of animated elements in the group.
    /// </summary>
    public List<AnimatedElement> AnimatedElements { get; } = [];

    /// <summary>
    /// Gets the layout strategy used to arrange the animated elements within the group.
    /// </summary>
    public ILayoutStrategy? LayoutStrategy { get; private set; }

    /// <summary>
    /// Applies the specified layout strategy to the displayed item.
    /// </summary>
    /// <param name="layoutStrategy">The layout strategy to apply. If <see langword="null"/>, a default layout strategy is used.</param>
    internal void ApplyLayout(ILayoutStrategy? layoutStrategy)
    {
        LayoutStrategy ??= layoutStrategy ?? new BindStackLayout();
        LayoutStrategy.ApplyLayout(_displayedItem);
    }

    /// <summary>
    /// Applies the specified start time to the current layout strategy, if one is defined.
    /// </summary>
    /// <remarks>This method delegates the operation to the <see cref="LayoutStrategy"/> if it is not null.
    /// Ensure that a layout strategy is set before calling this method to avoid no-op behavior.</remarks>
    /// <param name="now">The start time to apply, represented as a <see cref="DateTime"/>.</param>
    internal void ApplyStartTime(DateTime now)
    {
        LayoutStrategy?.ApplyStartTime(now);
    }

    /// <summary>
    /// Computes the differences between the current state of animated elements and their previous state,  and enqueues
    /// the changes as JSON-serializable objects for further processing.
    /// </summary>
    /// <remarks>This method processes animated elements in parallel to compute their differences. If an
    /// element has  changes compared to its previous state, the differences are added to the queue as a <see
    /// cref="JsonAnimatedElement"/>  object. The method also updates the <paramref name="previousElements"/> dictionary
    /// with the current state  of the modified elements.</remarks>
    /// <param name="now">The current timestamp used to update the state of each animated element.</param>
    /// <param name="queue">A thread-safe queue to which the computed differences are enqueued as <see cref="JsonAnimatedElement"/> objects.</param>
    /// <param name="previousElements">A dictionary containing the previous state of animated elements, keyed by their unique identifiers.  This
    /// dictionary is updated with the current state of elements that have changes.</param>
    internal void GetDiff(
        DateTime now,
        ConcurrentQueue<JsonAnimatedElement> queue,
        Dictionary<string, AnimatedElement> previousElements)
    {
        foreach (var element in _displayedItem)
        {
            element.Update(now);
            var diff = element.GetDiff(previousElements[element.Id]);

            if (diff.Count > 0)
            {
                diff["id"] = element.Id;
                previousElements[element.Id] = element.Clone();

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
        }
    }

    /// <summary>
    /// Sets the layout strategy for arranging the animated elements within the group.
    /// </summary>
    /// <param name="layout"></param>
    internal void SetLayoutStrategy(ILayoutStrategy? layout)
    {
        LayoutStrategy = layout;
    }

    /// <summary>
    /// Sets the maximum number of animated elements to be displayed in the group.
    /// </summary>
    /// <param name="maxDisplayedItems">Maximum number of animated elements to be displayed in the group.</param>
    internal void SetMaxDisplayedItems(int? maxDisplayedItems)
    {
        _displayedItem.Clear();
        _displayedItem.AddRange(maxDisplayedItems.HasValue ? AnimatedElements.Take(maxDisplayedItems.Value) : AnimatedElements);

    }
}

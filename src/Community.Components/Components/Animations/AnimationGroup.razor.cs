using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a group of animated items within a <see cref="FluentCxAnimation"/> component.
/// </summary>
public partial class AnimationGroup
    : FluentComponentBase, IDisposable
{
    private readonly AnimatedElementGroup _group = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimationGroup"/> class.
    /// </summary>
    public AnimationGroup()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the layout to apply to the animated items in this group.
    /// </summary>
    [Parameter]
    public RenderFragment? Layout { get; set; }

    /// <summary>
    /// Gets or sets the child content of the animation group.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxAnimation"/> component.
    /// </summary>
    [CascadingParameter]
    private FluentCxAnimation? Parent { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of items to display in the group.
    /// </summary>
    [Parameter]
    public int? MaxDisplayedItems { get; set; }

    /// <summary>
    /// Gets the internal animated element group used to manage the animated items.
    /// </summary>
    internal AnimatedElementGroup AnimatedElementGroup => _group;

    /// <summary>
    /// Gets the internal CSS classes for the component.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("animation-group")
        .Build();

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveGroup(this);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            Parent?.AddGroup(this);
        }
    }

    /// <summary>
    /// Adds an animated item to the group if it is not already present.
    /// </summary>
    /// <param name="item">Item to add.</param>
    internal void AddElement(AnimationItem item)
    {
       _group?.AnimatedElements.Add(item.AnimatedElement);
    }

    /// <summary>
    /// Removes an animated item from the group.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    internal void RemoveElement(AnimationItem item)
    {
        if (_group is null)
        {
            return;
        }

        _group.AnimatedElements.RemoveAll(e => e.Id == item.AnimatedElement.Id);
    }

    /// <summary>
    /// Removes the layout strategy from the group.
    /// </summary>
    internal void RemoveLayout()
    {
        _group?.SetLayoutStrategy(null);
    }

    /// <summary>
    /// Sets the layout strategy for the group.
    /// </summary>
    /// <param name="layout">Layout to use to animate the items in the group.</param>
    internal void SetLayout(ILayoutStrategy layout)
    {
        _group?.SetLayoutStrategy(layout);
    }

    /// <summary>
    /// Applies the specified layout strategy to the collection of animated elements.
    /// </summary>
    /// <remarks>If no layout strategy is provided, the method uses the default grid layout strategy. The
    /// method processes the collection of items and applies the layout to their associated animated elements.</remarks>
    /// <param name="layoutStrategy">The layout strategy to apply. If <see langword="null"/>, a default grid layout strategy is used.</param>
    /// <returns>A list of animated elements with the applied layout.</returns>
    internal void ApplyLayout(ILayoutStrategy? layoutStrategy)
    {
        _group?.ApplyLayout(layoutStrategy);
    }

    /// <summary>
    /// Applies the specified start time to all animated elements in the group.
    /// </summary>
    /// <param name="now">Start time.</param>
    internal void ApplyStartTime(DateTime now)
    {
        _group?.LayoutStrategy?.ApplyStartTime(now);
    }

    /// <summary>
    /// Sets the maximum number of displayed items if it has not already been set.
    /// </summary>
    /// <param name="maxDisplayedItems">Maximum number of displayed items.</param>
    internal void SetMaxDisplayedItems(int maxDisplayedItems)
    {
        _group?.SetMaxDisplayedItems(MaxDisplayedItems ?? maxDisplayedItems);
    }
}

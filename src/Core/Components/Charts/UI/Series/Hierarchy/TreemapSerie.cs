using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a treemap chart series component that can be used to visualize hierarchical data in a treemap chart format.
/// </summary>
public sealed class TreemapSerie<TItem> : SerieBase where TItem : HierarchyItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TreemapSerie{TItem}"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public TreemapSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Treemap;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets a function that defines how to group the items in the treemap chart.
    /// </summary>
    [Parameter]
    public Func<TItem, string[]>? Group { get; set; }

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var hierarchySerie = new HierarchySerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            HierarchyChartType = HierarchyChartType.Treemap,
            Group = Group is null ? null : item => Group((TItem)item),
        };

        hierarchySerie.UpdateItems(Items);

        return hierarchySerie;
    }
}

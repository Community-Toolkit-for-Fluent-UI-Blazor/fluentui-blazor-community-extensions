using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an icicle chart series component that can be used to visualize hierarchical data in an icicle chart format.
/// </summary>
public sealed class IcicleSerie<TItem> : SerieBase where TItem : HierarchyItem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IcicleSerie{TItem}"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public IcicleSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Icicle;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets a function that defines how to group the items in the icicle chart.
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
            HierarchyChartType = HierarchyChartType.Icicle,
            Group = Group is null ? null : item => Group((TItem)item),
        };

        hierarchySerie.UpdateItems(Items);

        return hierarchySerie;
    }
}

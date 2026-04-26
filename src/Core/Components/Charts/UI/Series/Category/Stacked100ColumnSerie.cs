using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chart series that displays data as vertical columns within the Fluent UI Blazor charting component.
/// </summary>
public sealed class Stacked100ColumnSerie
    : SerieBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Stacked100ColumnSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public Stacked100ColumnSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Stacked100Column;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<CategoryItem> Items { get; init; } = [];

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var columnSerie = new Charts.Series.ColumnSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            IsStacked = true,
            IsFull = true
        };

        columnSerie.UpdateItems(Items);

        return columnSerie;
    }
}

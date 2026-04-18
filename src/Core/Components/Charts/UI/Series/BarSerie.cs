using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a bar series component that displays a collection of category items using the Fluent UI Blazor library.
/// </summary>
/// <remarks>Use this class to render bar charts with customizable options and data items. The series can be
/// configured through the provided options and items collections. This component is intended for use within charting
/// scenarios where bar visualization of categorical data is required.</remarks>
public sealed class BarSerie
    : SerieBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BarSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public BarSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Bar;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<CategoryItem> Items { get; init; } = [];

    /// <summary>
    /// Gets or sets the configuration options for the bar series.
    /// </summary>
    [Parameter]
    public BarSerieOptions? Options { get; init; } = new();

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var barSerie = new Charts.Series.BarSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Options = Options,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled
        };

        barSerie.UpdateItems(Items);

        return barSerie;
    }
}

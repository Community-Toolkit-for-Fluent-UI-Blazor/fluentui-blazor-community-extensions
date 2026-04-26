using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a bubble series component for displaying category data in a Fluent UI Blazor chart.
/// </summary>
public sealed class BubbleSerie : SerieBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BubbleSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public BubbleSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Bubble;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<CategoryItem> Items { get; init; } = [];

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var cls = new Charts.Series.LineSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            LineType = LineChartType.Bubble
        };

        cls.UpdateItems(Items);

        return cls;
    }
}

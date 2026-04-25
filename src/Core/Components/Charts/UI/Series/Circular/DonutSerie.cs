using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a donut component to render a donut chart series.
/// </summary>
public class DonutSerie : SerieBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DonutSerie"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the Fluent UI Blazor library.</param>
    public DonutSerie(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc />
    protected internal override ChartType ChartType => ChartType.Donut;

    /// <summary>
    /// Gets the collection of category items to display in the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<RadialSlice> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the configuration options for the bar series.
    /// </summary>
    [Parameter]
    public RadialSerieOptions? Options { get; set; } = new();

    /// <summary>
    /// Gets or sets the parent multi-donut series that this donut series belongs to, if applicable. This property is used to establish a hierarchical relationship between individual donut series and a containing multi-donut series, allowing for coordinated rendering and interaction within a multi-donut chart context.
    /// </summary>
    [CascadingParameter]
    private MultiDonutSerie? ParentMultiDonutSerie { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the animation alternates direction on each pie.
    /// </summary>
    [Parameter]
    public bool AlternateAnimation { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        ParentMultiDonutSerie?.Add(this);
    }

    /// <inheritdoc />
    protected override void DisposeOverride()
    {
        base.DisposeOverride();

        ParentMultiDonutSerie?.Remove(this);
    }

    /// <inheritdoc />
    protected internal override ChartSerie Create()
    {
        var donutSerie = new Charts.Series.DonutSerie
        {
            Id = Id!,
            Name = Name,
            Tag = Tag,
            IsVisible = IsVisible,
            Style = ItemStyle,
            Interaction = Interaction,
            Animation = GetAnimationOptions(),
            AnimationEnabled = AnimationEnabled,
            AlternateAnimation = AlternateAnimation,
            Options = Options,
            IsPartOfMultiDonut = ParentMultiDonutSerie is not null
        };

        donutSerie.UpdateItems(Items);

        return donutSerie;
    }
}

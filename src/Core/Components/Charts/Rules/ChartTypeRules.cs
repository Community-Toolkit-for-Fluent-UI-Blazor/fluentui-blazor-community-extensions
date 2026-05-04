using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Charts;

internal sealed class ChartTypeRule
{
    public ChartFamily Category { get; init; }
    public bool AllowMultipleSeries { get; init; }
    public bool RequireSameCategoryCount { get; init; }
    public bool RequireSameCategoryLabels { get; init; }
}

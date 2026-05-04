using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a radar chart component.
/// </summary>
public class RadarChartAxesOptions : ComponentBase, IDisposable
{
    /// <summary>
    /// Value indicating whether the <see cref="Grid"/> property has changed since the last render.
    /// </summary>
    private bool _hasGridChanged;

    /// <summary>
    /// Value indicating whether the Levels property has changed since the last render.
    /// </summary>
    private bool _hasLevelsChanged;

    /// <summary>
    /// Gets or sets the type of grid to be used for the radar chart axes.
    /// </summary>
    [Parameter]
    public RadarGridType Grid { get; set; }

    /// <summary>
    /// Gets or sets the number of levels in the radar chart.
    /// </summary>
    [Parameter]
    public int? Levels { get; set; }

    /// <summary>
    /// Gets or sets the parent chart component that this options instance is associated with.
    /// </summary>
    [CascadingParameter]
    private ChartOptions? Parent { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveRadarChartAxesOptions();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("RadarChartAxesOptions must be used within a FluentCxChart component.");
        }

        Parent.AddRadarChartAxesOptions(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasGridChanged = parameters.TryGetValue<RadarGridType>(nameof(Grid), out var newGrid) && newGrid != Grid;
        _hasLevelsChanged = parameters.TryGetValue<int?>(nameof(Levels), out var newLevels) && newLevels != Levels;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        if (_hasGridChanged ||
            _hasLevelsChanged)
        {
            _hasGridChanged = false;
            _hasLevelsChanged = false;

            Parent?.NotifyRadarChartAxesOptionsChanged();
        }

        return base.OnParametersSetAsync();
    }
}

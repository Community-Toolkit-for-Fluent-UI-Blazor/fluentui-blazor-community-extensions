using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a category line chart component, allowing customization of appearance and behavior.
/// </summary>
public class ChartCategoryLineOptions : ComponentBase, IDisposable
{
    /// <summary>
    /// Value indicating whether a parameter has changed since the last render.
    /// </summary>
    private bool _hasChanged;

    /// <summary>
    /// Gets a value indicating whether smooth transitions are enabled.
    /// </summary>
    [Parameter]
    public bool Smooth { get; init; }

    /// <summary>
    /// Gets a value indicating whether markers are shown at data points.
    /// </summary>
    [Parameter]
    public bool ShowMarkers { get; init; } = true;

    /// <summary>
    /// Gets the size of the marker.
    /// </summary>
    [Parameter]
    public double MarkerSize { get; init; } = 4;

    /// <summary>
    /// Gets a value indicating whether the area should be displayed.
    /// </summary>
    [Parameter]
    public bool ShowArea { get; init; }

    /// <summary>
    /// Gets a value indicating whether categories are sorted.
    /// </summary>
    [Parameter]
    public bool Sort { get; set; }

    /// <summary>
    /// Gets a value indicating whether labels are displayed alongside the associated elements.
    /// </summary>
    [Parameter]
    public bool ShowLabels { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether values are displayed alongside the corresponding elements.
    /// </summary>
    [Parameter]
    public bool ShowValues { get; set; }

    /// <summary>
    /// Gets or sets the parent chart component that this options instance is associated with.
    /// </summary>
    [CascadingParameter]
    private ChartOptions? Parent { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent!.RemoveCategoryLineOptions();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(ChartCategoryLineOptions)} must be used within a {nameof(FluentCxChart)} component.");
        }

        Parent.AddCategoryLineOptions(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasChanged = parameters.TryGetValue<bool>(nameof(Smooth), out var newSmooth) && newSmooth != Smooth ||
                      parameters.TryGetValue<bool>(nameof(ShowMarkers), out var newShowMarkers) && newShowMarkers != ShowMarkers ||
                      parameters.TryGetValue<double>(nameof(MarkerSize), out var newMarkerSize) && newMarkerSize != MarkerSize ||
                      parameters.TryGetValue<bool>(nameof(ShowArea), out var newShowArea) && newShowArea != ShowArea ||
                      parameters.TryGetValue<bool>(nameof(Sort), out var newSort) && newSort != Sort ||
                      parameters.TryGetValue<bool>(nameof(ShowLabels), out var newShowLabels) && newShowLabels != ShowLabels ||
                      parameters.TryGetValue<bool>(nameof(ShowValues), out var newShowValues) && newShowValues != ShowValues;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        if (_hasChanged)
        {
            _hasChanged = false;
            Parent!.NotifyCategoryLineOptionsChanged();
        }

        return base.OnParametersSetAsync();
    }
}

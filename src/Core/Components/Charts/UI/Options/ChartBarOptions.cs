using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a bar chart component.
/// </summary>
/// <remarks>Use this class to specify settings that control the appearance and behavior of a bar chart when
/// rendering in a Blazor application. This type is intended to be used as a parameter or configuration object for
/// chart-related components.</remarks>
public class ChartBarOptions : ComponentBase, IDisposable
{
    /// <summary>
    /// Value indicating wether a parameter has been modified since the last render.
    /// </summary>
    private bool _hasChanged;

    /// <summary>
    /// Gets or sets the height of each bar as a fraction of the available space.
    /// </summary>
    [Parameter]
    public double BarHeight { get; set; } = 0.8;

    /// <summary>
    /// Gets or sets a value indicating whether sorting is enabled for the component.
    /// </summary>
    [Parameter]
    public bool Sort { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether values are displayed alongside the component.
    /// </summary>
    [Parameter]
    public bool ShowValues { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether labels are displayed alongside the component.
    /// </summary>
    [Parameter]
    public bool ShowLabels { get; set; } = true;

    /// <summary>
    /// Gets or sets the parent chart component that this options instance is associated with.
    /// </summary>
    [CascadingParameter]
    private ChartOptions? Parent { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent!.RemoveBarOptions();

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("ChartBarOptions must be used within a FluentCxChart component.");
        }

        Parent.AddBarOptions(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasChanged = parameters.TryGetValue<double>(nameof(BarHeight), out var newBarHeight) && newBarHeight != BarHeight ||
                      parameters.TryGetValue<bool>(nameof(Sort), out var newSort) && newSort != Sort ||
                      parameters.TryGetValue<bool>(nameof(ShowValues), out var newShowValues) && newShowValues != ShowValues ||
                      parameters.TryGetValue<bool>(nameof(ShowLabels), out var newShowLabels) && newShowLabels != ShowLabels;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        if (_hasChanged)
        {
            _hasChanged = false;
            Parent!.NotifyBarOptionsChanged();
        }

        return base.OnParametersSetAsync();
    }
}

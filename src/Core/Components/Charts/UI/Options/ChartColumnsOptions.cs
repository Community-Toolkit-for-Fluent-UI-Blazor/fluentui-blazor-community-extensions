using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a column chart component.
/// </summary>
/// <remarks>Use this class to specify settings that control the appearance and behavior of a column chart when
/// rendering in a Blazor application. This type is intended to be used as a parameter or configuration object for
/// chart-related components.</remarks>
public class ChartColumnOptions : ComponentBase, IDisposable
{
    /// <summary>
    /// Value indicating whether the <see cref="ColumnWidth"/> property has changed since the last render.
    /// </summary>
    private bool _hasColumnWidthChanged;

    /// <summary>
    /// Value indicating whether the Sort property has changed since the last render.
    /// </summary>
    private bool _hasSortChanged;

    /// <summary>
    /// Value indicating whether the ShowValues property has changed since the last render.
    /// </summary>
    private bool _hasShowValuesChanged;

    /// <summary>
    /// Value indicating whether the ShowLabels property has changed since the last render.
    /// </summary>
    private bool _hasShowLabelsChanged;

    /// <summary>
    /// Gets or sets the width of each column as a fraction of the available space.
    /// </summary>
    [Parameter]
    public double ColumnWidth { get; set; } = 0.8;

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
        Parent?.RemoveColumnOptions();

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

        Parent.AddColumnOptions(this);
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasColumnWidthChanged = parameters.TryGetValue<double>(nameof(ColumnWidth), out var newColumnWidth) && newColumnWidth != ColumnWidth;
        _hasSortChanged = parameters.TryGetValue<bool>(nameof(Sort), out var newSort) && newSort != Sort;
        _hasShowValuesChanged = parameters.TryGetValue<bool>(nameof(ShowValues), out var newShowValues) && newShowValues != ShowValues;
        _hasShowLabelsChanged = parameters.TryGetValue<bool>(nameof(ShowLabels), out var newShowLabels) && newShowLabels != ShowLabels;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override Task OnParametersSetAsync()
    {
        if (_hasColumnWidthChanged ||
            _hasSortChanged ||
            _hasShowValuesChanged ||
            _hasShowLabelsChanged)
        {
            _hasColumnWidthChanged = false;
            _hasSortChanged = false;
            _hasShowLabelsChanged = false;
            _hasShowValuesChanged = false;

            Parent?.NotifyColumnOptionsChanged();
        }

        return base.OnParametersSetAsync();
    }
}

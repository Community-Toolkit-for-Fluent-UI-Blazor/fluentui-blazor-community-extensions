using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Plotly.Blazor;
using Plotly.Blazor.ConfigLib;
using Plotly.Blazor.LayoutLib;

namespace FluentUI.Blazor.Community.Components;

public abstract partial class ChartBase<T>
    : FluentComponentBase
    where T : ITrace
{
    [Parameter]
    public Layout? Layout { get; set; }

    protected readonly RenderFragment _renderChart;

    [Parameter]
    public string? Width { get; set; } = "100%";

    [Parameter]
    public string? Height { get; set; } = "100%";

    [Parameter]
    public string? MaxHeight { get; set; } = "100%";

    [Parameter]
    public bool? AutoSize { get; set; }

    [Parameter]
    public Config? Config { get; set; } = new Config()
    {
        Responsive = true,
        DisplayModeBar = DisplayModeBarEnum.True,
    };

    [Parameter]
    public bool Responsive { get; set; } = true;

    [Parameter]
    public DisplayModeBarEnum DispplayModeBar { get; set; } = DisplayModeBarEnum.True;

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? BackgroundColor { get; set; }

    [Parameter]
    public string? ForegroundColor { get; set; }

    [Parameter]
    public Font? Font { get; set; }

    [Parameter]
    public IEnumerable<T>? Items { get; set; }

    [Parameter]
    public EventCallback<IEnumerable<T>> ItemsChanged { get; set; }

    protected string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("max-height", MaxHeight, !string.IsNullOrEmpty(MaxHeight))
        .Build();

    internal IList<ITrace> GetTraces()
    {
        if (Items is null)
        {
            return [];
        }

        return [.. Items.OfType<ITrace>()];
    }

    internal async Task OnDataChangedAsync(IList<ITrace> traces)
    {
        var items = new List<T>();
        items.AddRange(traces.OfType<T>());

        Items = items;

        if (ItemsChanged.HasDelegate)
        {
            await ItemsChanged.InvokeAsync(items);
        }

        await InvokeAsync(StateHasChanged);
    }

    protected void ChangeLayout()
    {
        Layout = ChangeLayoutOverride();
        StateHasChanged();
    }

    protected void ChangeConfig()
    {
        Config = GetConfig(Config);
        StateHasChanged();
    }

    private Config? GetConfig(Config? config)
    {
        return new Config()
        {
            Responsive = Responsive,
            DisplayModeBar = DispplayModeBar,
            AutoSizable = config?.AutoSizable,
            DisplayLogo = config?.DisplayLogo,
            DoubleClick = config?.DoubleClick,
            DoubleClickDelay = config?.DoubleClickDelay,
            Editable = config?.Editable,
            Edits = config?.Edits,
            EditSelection = config?.EditSelection,
            FillFrame = config?.FillFrame,
            FrameMargins = config?.FrameMargins,
            LinkText = config?.LinkText,
            Locale = config?.Locale,
            Locales = config?.Locales,
            Logging = config?.Logging,
            MapboxAccessToken = config?.MapboxAccessToken,
            ModeBarButtons = config?.ModeBarButtons,
            ModeBarButtonsToAdd = config?.ModeBarButtonsToAdd,
            ModeBarButtonsToRemove = config?.ModeBarButtonsToRemove,
            NotifyOnLogging = config?.NotifyOnLogging,
            PlotGlPixelRatio = config?.PlotGlPixelRatio,
            PlotlyServerUrl = config?.PlotlyServerUrl,
            QueueLength = config?.QueueLength,
            ScrollZoom = config?.ScrollZoom,
            SendData = config?.SendData,
            SetBackground = config?.SetBackground,
            ShowAxisDragHandles = config?.ShowAxisDragHandles,
            ShowAxisRangeEntryBoxes = config?.ShowAxisRangeEntryBoxes,
            ShowEditInChartStudio = config?.ShowEditInChartStudio,
            ShowLink = config?.ShowLink,
            ShowSendToCloud = config?.ShowSendToCloud,
            ShowSources = config?.ShowSources,
            ShowTips = config?.ShowTips,
            StaticPlot = config?.StaticPlot,
            ToImageButtonOptions = config?.ToImageButtonOptions,
            TopoJsonUrl = config?.TopoJsonUrl,
            TypesetMath = config?.TypesetMath,
            Watermark = config?.Watermark
        };
    }

    protected virtual Layout ChangeLayoutOverride()
    {
        return new Layout
        {
            AutoSize = AutoSize,
            Title = new Title
            {
                Text = Title
            },
            Font = Font,
            PaperBgColor = BackgroundColor,
            PlotBgColor = ForegroundColor,
        };
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ChangeConfig();
        ChangeLayout();
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Title), Title) ||
            parameters.HasValueChanged(nameof(BackgroundColor), BackgroundColor) ||
            parameters.HasValueChanged(nameof(ForegroundColor), ForegroundColor) ||
            parameters.HasValueChanged(nameof(Font), Font))
        {
            ChangeLayout();
        }

        if (parameters.HasValueChanged(nameof(Responsive), Responsive) ||
            parameters.HasValueChanged(nameof(DispplayModeBar), DispplayModeBar))
        {
            ChangeConfig();
        }

        if (parameters.HasValueChanged(nameof(Items), Items))
        {
            StateHasChanged();
        }
    }
}

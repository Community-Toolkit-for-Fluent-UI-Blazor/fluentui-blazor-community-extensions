using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.Traces;

namespace FluentUI.Blazor.Community.Components;

public partial class PieChart
    : ChartBase<Pie>
{
    [Parameter]
    public bool? ExtendedPieColor { get; set; }

    [Parameter]
    public IList<object> PieColorway { get; set; } = [];

    protected override Layout ChangeLayoutOverride()
    {
        return new Layout()
        {
            Title = new Title()
            {
                Text = Title
            },
            PaperBgColor = BackgroundColor,
            PlotBgColor = ForegroundColor,
            Font = Font,
            PieColorway = PieColorway,
            ExtendPieColors = ExtendedPieColor
        };
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(ExtendedPieColor), ExtendedPieColor) ||
            parameters.HasValueChanged(nameof(PieColorway), PieColorway))
        {
            ChangeLayout();
        }
    }
}

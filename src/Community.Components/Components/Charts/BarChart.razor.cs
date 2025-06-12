// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.Traces;

namespace FluentUI.Blazor.Community.Components;

public partial class BarChart
    : ChartBase<Bar>
{
    [Parameter]
    public BarModeEnum Mode { get; set; } = BarModeEnum.Stack;

    protected override Layout ChangeLayoutOverride()
    {
        var layout = base.ChangeLayoutOverride();

        layout.BarMode = Mode;

        return layout;
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Mode), Mode))
        {
            ChangeLayout();
        }
    }
}

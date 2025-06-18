// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Plotly.Blazor;
using Plotly.Blazor.LayoutLib;
using Plotly.Blazor.Traces;

namespace FluentUI.Blazor.Community.Components;

public partial class BoxChart
    : ChartBase<Box>
{
    [Parameter]
    public decimal? Gap { get; set; }

    [Parameter]
    public decimal? GroupGap { get; set; }

    [Parameter]
    public BoxModeEnum Mode { get; set; }

    protected override Layout ChangeLayoutOverride()
    {
        var layout = base.ChangeLayoutOverride();

        layout.BoxGap = Gap;
        layout.BoxGroupGap = GroupGap;
        layout.BoxMode = Mode;

        return layout;
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Gap), Gap) ||
            parameters.HasValueChanged(nameof(GroupGap), GroupGap) ||
            parameters.HasValueChanged(nameof(Mode), Mode))
        {
            ChangeLayout();
        }
    }
}

using System.Globalization;
using System.Runtime.CompilerServices;
using FluentUI.Blazor.Community.Extensions;
using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxImageGroup
    : FluentComponentBase
{
    private readonly List<FluentCxImage> _children = [];
    private bool _isPopoverOpen;

    public FluentCxImageGroup()
    {
        Id = StringHelper.GenerateId();
    }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = default!;

    [Parameter]
    public int MaxVisibleItems { get; set; }

    [Parameter]
    public ImageSize Size { get; set; } = ImageSize.Size40;

    [Parameter]
    public ImageShape Shape { get; set; } = ImageShape.RoundSquare;

    [Parameter]
    public ImageGroupType GroupType { get; set; } = ImageGroupType.Spread;

    private int VisibleCount => MaxVisibleItems < _children.Count ? MaxVisibleItems : _children.Count;

    internal void Add(FluentCxImage image)
    {
        image.SetGroupSize((int)Size);
        _children.Add(image);
    }

    internal void Remove(FluentCxImage image)
    {
        _children.Remove(image);
    }

    internal string GetBorderRadius()
    {
        return Shape switch
        {
            ImageShape.Square => "0px",
            ImageShape.RoundSquare => "8px",
            _ => "10000px"
        };
    }

    internal int GetSpreadMarginLeft(FluentCxImage image)
    {
        return _children.IndexOf(image) <= 0 ? 0 : 16;
    }

    internal string GetStackMarginLeft(FluentCxImage image)
    {
        return _children.IndexOf(image) <= 0 ? "0" : (-6 * ((int)Size / 16f)).ToString(CultureInfo.InvariantCulture);
    }

    internal bool IsInPopover(FluentCxImage image)
    {
        return _children.IndexOf(image) >= MaxVisibleItems;
    }

    private string GetButtonStyle()
    {
        DefaultInterpolatedStringHandler handler = new();
        var size = (int)Size;

        if (GroupType == ImageGroupType.Spread)
        {
            handler.AppendLiteral("margin-left: 16px;");
        }
        else
        {
            handler.AppendLiteral("margin-left: ");
            handler.AppendFormatted(-6 * (size / 16f));
            handler.AppendLiteral("px;");
        }

        handler.AppendLiteral("border-radius: ");
        handler.AppendFormatted(GetBorderRadius());
        handler.AppendLiteral(";");

        handler.AppendLiteral("width: ");
        handler.AppendFormatted(size);
        handler.AppendLiteral("px; height: ");
        handler.AppendFormatted(size);
        handler.AppendLiteral("px;");

        return handler.ToString();
    }

    protected internal virtual void OnItemParemetersChanged(FluentCxImage image)
    {
        StateHasChanged();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Size), Size))
        {
            foreach (var item in _children)
            {
                item.SetGroupSize((int)Size);
            }
        }
    }
}

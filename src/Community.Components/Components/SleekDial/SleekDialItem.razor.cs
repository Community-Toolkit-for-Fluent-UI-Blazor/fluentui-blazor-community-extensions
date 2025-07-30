using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

public partial class SleekDialItem
    : FluentComponentBase, IDisposable
{
    private string? _angle;

    internal RenderFragment RenderComponent { get; private set; }

    [CascadingParameter]
    private FluentCxSleekDial? Parent { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public Icon? Icon { get; set; }

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public string? Title { get; set; }

    internal int Index => Parent?.Items.IndexOf(this) ?? -1;

    private string? InternalClass => new CssBuilder(Class)
        .AddClass("sleekdialitem")
        .AddClass("sleekdialitem-active", Index == Parent?.FocusedIndex)
        .AddClass("sleekdialitem-disabled", Disabled)
        .AddClass("sleekdialitem-vertical-linear", Parent?.Mode == SleekDialMode.Linear && Parent?.Direction.IsOneOf(SleekDialLinearDirection.Down, SleekDialLinearDirection.Default, SleekDialLinearDirection.Up) == true)
        .AddClass("sleekdialitem-horizontal-linear", Parent?.Mode == SleekDialMode.Linear && Parent?.Direction.IsOneOf(SleekDialLinearDirection.Left, SleekDialLinearDirection.Right) == true)
        .AddClass("sleekdialitem-radial", Parent?.Mode == SleekDialMode.Radial)
        .AddClass("sleekdialitem-radial-center", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter) ?? false))
        .AddClass("sleekdialitem-radial-middle", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight) ?? false))
        .AddClass("sleekdialitem-radial-top", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight) ?? false))
        .AddClass("sleekdialitem-radial-right", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.MiddleRight, FloatingPosition.BottomRight) ?? false))
        .AddClass("sleekdialitem-radial-bottom", Parent?.Mode == SleekDialMode.Radial && (Parent?.Position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight) ?? false))
        .Build();

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--sleekdial-radial-angle", _angle, Parent?.Mode == SleekDialMode.Radial)
        .Build();

    private static string GetAngle(FluentCxSleekDial? value, int index)
    {
        if (value is null || value.CorrectRadialSettings is null)
        {
            return string.Empty;
        }

        var radialSettings = value.CorrectRadialSettings;
        var count = value.Items.Count;
        var delta = Math.Abs(radialSettings.EndAngle - radialSettings.StartAngle);
        var itemCount = delta == 360 || delta == 0 || count == 1 ? count : count - 1;
        var stepAngle = delta / itemCount;
        var itemAngle = stepAngle * index;
        var angle = radialSettings.Direction == SleekDialRadialDirection.Clockwise ? radialSettings.StartAngle + itemAngle : radialSettings.StartAngle - itemAngle;
        var finalAngle = angle % 360;

        return $"{finalAngle}deg";
    }

    private async Task OnClickAsync()
    {
        if (Parent is not null)
        {
            Parent.FocusedIndex = Parent.Items.IndexOf(this);
            await Parent.OnItemClickAsync(this);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveChild(this);

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.AddChild(this);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && Parent is not null)
        {
            await Parent.OnCreatedAsync(this);
        }
    }

    internal void UpdateAngle()
    {
        _angle = GetAngle(Parent, Index);
    }
}

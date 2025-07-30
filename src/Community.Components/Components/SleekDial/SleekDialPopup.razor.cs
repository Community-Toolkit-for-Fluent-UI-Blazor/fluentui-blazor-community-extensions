using System.Drawing;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class SleekDialPopup
    : FluentComponentBase
{
    private enum FocusElementMove
    {
        Up,
        Down,
        Left,
        Right
    }

    private struct LinearPositionOptions
    {
        public bool IsVertical { get; set; }
        public bool IsTop { get; set; }
        public bool IsLeft { get; set; }
        public bool IsCenter { get; set; }
        public bool IsMiddle { get; set; }
        public bool IsFixed { get; set; }
        public SleekDialLinearDirection Direction { get; set; }
    }

    private struct RadialPositionOptions
    {
        public bool IsTop { get; set; }
        public bool IsBottom { get; set; }
        public bool IsLeft { get; set; }
        public bool IsCenter { get; set; }
        public bool IsMiddle { get; set; }
        public bool IsFixed { get; set; }
        public bool IsRight { get; set; }
    }

    private IJSObjectReference? _module;
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/SleekDial/SleekDialPopup.razor.js";
    private LinearPositionOptions _linearPositionOptions;
    private RadialPositionOptions _radialPositionOptions;
    private readonly DotNetObjectReference<SleekDialPopup> _popupReference;
    private bool _isLinear;
    private float _xOffset;
    private float _yOffset;
    private float _width;
    private float _height;

    /// <summary>
    /// Initializes a new instance of the <see cref="SleekDialPopup"/> class.
    /// </summary>
    public SleekDialPopup()
    {
        Id = Identifier.NewId();
        _popupReference = DotNetObjectReference.Create(this);
    }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public bool IsOpen { get; set; }

    [CascadingParameter]
    private FluentCxSleekDial? Parent { get; set; }

    [Parameter]
    public EventCallback<bool> OnAnimationCompleted { get; set; }

    private string? InternalCss => new CssBuilder(Class)
        .AddClass("sleekdial-popup")
        .AddClass("sleekdial-popup-linear-top", _isLinear && _linearPositionOptions.IsTop)
        .AddClass("sleekdial-popup-linear-bottom", _isLinear && !_linearPositionOptions.IsTop)
        .AddClass("sleekdial-popup-linear-left", _isLinear && _linearPositionOptions.IsLeft && !_linearPositionOptions.IsCenter)
        .AddClass("sleekdial-popup-linear-right", _isLinear && !_linearPositionOptions.IsLeft && !_linearPositionOptions.IsCenter)
        .AddClass("sleekdial-popup-linear-center", (_isLinear && _linearPositionOptions.IsCenter))
        .AddClass("sleekdial-popup-linear-middle", (_isLinear && _linearPositionOptions.IsMiddle))
        .AddClass("sleekdial-popup-linear-direction-left", _isLinear && _linearPositionOptions.Direction == SleekDialLinearDirection.Left)
        .AddClass("sleekdial-popup-linear-direction-right", _isLinear && _linearPositionOptions.Direction == SleekDialLinearDirection.Right)
        .AddClass("sleekdial-popup-linear-direction-down", _isLinear && _linearPositionOptions.IsTop)
        .AddClass("sleekdial-popup-radial", !_isLinear)
        .AddClass("sleekdial-popup-radial-left", !_isLinear && _radialPositionOptions.IsLeft)
        .AddClass("sleekdial-popup-radial-right", !_isLinear && _radialPositionOptions.IsRight)
        .AddClass("sleekdial-popup-radial-bottom", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight))
        .AddClass("sleekdial-popup-radial-top", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight))
        .AddClass("sleekdial-popup-radial-top-left", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter))
        .AddClass("sleekdial-popup-radial-top-right", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.TopCenter, FloatingPosition.MiddleRight, FloatingPosition.MiddleCenter))
        .AddClass("sleekdial-popup-radial-bottom-left", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter))
        .AddClass("sleekdial-popup-radial-bottom-right", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.BottomRight, FloatingPosition.BottomCenter, FloatingPosition.MiddleRight, FloatingPosition.MiddleCenter))
        .AddClass("sleekdial-popup-radial-center", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter))
        .AddClass("sleekdial-popup-hidden", !IsOpen)
        .AddClass("sleekdial-popup-radial-middle", !_isLinear && Parent!.Position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight))
        .Build();

    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--sleekdial-radial-offset", Parent?.CorrectRadialSettings?.Offset ?? "110px", !_isLinear)
        .AddStyle("--sleekdial-radial-min-width", $"{_width}px", !_isLinear && _width > 0)
        .AddStyle("--sleekdial-radial-min-height", $"{_height}px", !_isLinear && _height > 0)
        .AddStyle("--sleekdial-vertical-offset", $"{_yOffset}px", !_isLinear && _yOffset > 0)
        .AddStyle("--sleekdial-horizontal-offset", $"{_xOffset}px", !_isLinear && _xOffset > 0)
        .Build();

    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    private async Task OnOverlayCloseAsync()
    {
        if (Parent is not null)
        {
            await Parent.ShowOrHidePopupAsync(false);
        }
    }

    internal async Task HandleKeyAsync(FluentKeyCodeEventArgs e)
    {
        switch (e.Key)
        {
            case KeyCode.End:
                await FocusLastElementAsync();
                break;

            case KeyCode.Home:
                await FocusFirstElementAsync();
                break;

            case KeyCode.Right:
                await FocusElementAsync(FocusElementMove.Right);
                break;

            case KeyCode.Down:
                await FocusElementAsync(FocusElementMove.Down);
                break;

            case KeyCode.Up:
                await FocusElementAsync(FocusElementMove.Up);
                break;

            case KeyCode.Left:
                await FocusElementAsync(FocusElementMove.Left);
                break;
        }
    }

    private async Task FocusFirstElementAsync()
    {
        if (Parent is not null)
        {
            var index = 0;
            var items = Parent.Items;

            while (items[index].Disabled)
            {
                ++index;

                if (index == items.Count)
                {
                    return;
                }
            }

            Parent.FocusedIndex = index;
            await Parent.FocusAsync();
        }
    }

    private async Task FocusLastElementAsync()
    {
        if (Parent is not null)
        {
            var items = Parent.Items;
            var index = Parent.Items.Count - 1;

            while (items[index].Disabled)
            {
                --index;

                if (index < 0)
                {
                    return;
                }
            }

            Parent.FocusedIndex = index;
            await Parent.FocusAsync();
        }
    }

    private async Task FocusPreviousElementAsync()
    {
        if (Parent is not null)
        {
            var focusedIndex = Parent.FocusedIndex;
            do
            {
                ++focusedIndex;

                if (focusedIndex == Parent.Items.Count)
                {
                    return;
                }
            }
            while (Parent.Items[focusedIndex].Disabled);

            Parent.FocusedIndex = focusedIndex;
            await Parent.FocusAsync();
        }
    }

    private async Task FocusElementAsync(FocusElementMove value)
    {
        if (Parent is not null)
        {
            var isLinear = Parent.Mode == SleekDialMode.Linear;

            switch (value)
            {
                case FocusElementMove.Up:
                case FocusElementMove.Right:
                    if (isLinear)
                    {
                        await FocusNextElementAsync();
                    }
                    else if (Parent.RadialSettings?.Direction == SleekDialRadialDirection.Clockwise)
                    {
                        await FocusNextElementAsync();
                    }
                    else
                    {
                        await FocusPreviousElementAsync();
                    }

                    break;

                case FocusElementMove.Down:
                case FocusElementMove.Left:
                    if (isLinear)
                    {
                        await FocusPreviousElementAsync();
                    }
                    else if (Parent.RadialSettings?.Direction == SleekDialRadialDirection.Clockwise)
                    {
                        await FocusPreviousElementAsync();
                    }
                    else
                    {
                        await FocusNextElementAsync();
                    }

                    break;
            }
        }
    }

    private async Task FocusNextElementAsync()
    {
        if (Parent is not null)
        {
            var focusedIndex = Parent.FocusedIndex;

            do
            {
                --focusedIndex;

                if (focusedIndex < 0)
                {
                    Parent.FocusedIndex = -1;
                    return;
                }
            }
            while (Parent.Items[focusedIndex].Disabled);

            Parent.FocusedIndex = focusedIndex;
            await Parent.FocusAsync();
        }
    }

    private async Task InvokeScriptAsync(string method, params object?[] args)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync(method, args);
        }
    }

    internal async Task OnKeyDownHandlerAsync(FluentKeyCodeEventArgs e)
    {
        if (e.Key != KeyCode.Escape)
        {
            return;
        }

        if (Parent is not null)
        {
            await Parent.ShowOrHidePopupAsync(false);
        }
    }

    internal async Task UpdatePositionAsync()
    {
        ApplyPosition();

        if (_isLinear)
        {
            await InvokeScriptAsync("updateLinearPosition", Id, _linearPositionOptions);
            await InvokeScriptAsync("setLinearPosition", Id);
        }
        else
        {
            await InvokeScriptAsync("updateRadialPosition", Id, _radialPositionOptions);
            await InvokeScriptAsync("setRadialPosition", Id);
        }

        await InvokeAsync(StateHasChanged);
    }

    private void ApplyPosition()
    {
        if (Parent is not null)
        {
            _isLinear = Parent.Mode == SleekDialMode.Linear;

            if (_isLinear)
            {
                var position = Parent.Position;
                var actualDirection = Parent.Direction switch
                {
                    SleekDialLinearDirection.Up => position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight) ? SleekDialLinearDirection.Default : SleekDialLinearDirection.Up,
                    SleekDialLinearDirection.Down => position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight) ? SleekDialLinearDirection.Default : SleekDialLinearDirection.Down,
                    SleekDialLinearDirection.Right => position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.MiddleRight, FloatingPosition.BottomRight) ? SleekDialLinearDirection.Default : SleekDialLinearDirection.Right,
                    SleekDialLinearDirection.Left => position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.MiddleLeft, FloatingPosition.BottomLeft) ? SleekDialLinearDirection.Default : SleekDialLinearDirection.Left,
                    _ => SleekDialLinearDirection.Default
                };

                var isVertical = actualDirection != SleekDialLinearDirection.Left && actualDirection != SleekDialLinearDirection.Right;

                _linearPositionOptions.IsVertical = isVertical;

                _linearPositionOptions.IsTop = actualDirection == SleekDialLinearDirection.Down ||
                         actualDirection == SleekDialLinearDirection.Default && (position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight) ||
                         !_linearPositionOptions.IsVertical && !position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight));

                _linearPositionOptions.IsLeft = (actualDirection == SleekDialLinearDirection.Default && position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.MiddleLeft, FloatingPosition.BottomLeft)) ||
                                                (actualDirection == SleekDialLinearDirection.Left && GlobalState.Dir == LocalizationDirection.RightToLeft) ||
                                                (actualDirection == SleekDialLinearDirection.Right && GlobalState.Dir == LocalizationDirection.LeftToRight) ||
                                                (isVertical && position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.MiddleRight, FloatingPosition.BottomRight));
                _linearPositionOptions.Direction = actualDirection;
                _linearPositionOptions.IsCenter = position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter);
                _linearPositionOptions.IsMiddle = position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight);
            }
            else
            {
                var position = Parent.Position;
                Parent.CorrectRadialSettings = GetRadialSettings();
                _radialPositionOptions.IsTop = position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight);
                _radialPositionOptions.IsBottom = position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight);
                _radialPositionOptions.IsLeft = position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.MiddleLeft, FloatingPosition.BottomLeft);
                _radialPositionOptions.IsCenter = position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter);
                _radialPositionOptions.IsMiddle = position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight);
                _radialPositionOptions.IsRight = position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.MiddleRight, FloatingPosition.BottomRight);

                Parent?.UpdateItemsPosition();
            }
        }
    }

    private SleekDialRadialSettings GetRadialSettings()
    {
        if (Parent is null)
        {
            return new();
        }

        var originalRadialSettings = Parent.RadialSettings;

        SleekDialRadialSettings settings = new()
        {
            Offset = originalRadialSettings.Offset
        };

        var startAngle = originalRadialSettings.StartAngle;
        var endAngle = originalRadialSettings.EndAngle;
        var isClock = originalRadialSettings.Direction == SleekDialRadialDirection.Clockwise;

        switch (Parent.Position)
        {
            case FloatingPosition.TopLeft:
            case FloatingPosition.TopRight:
                {
                    if (Parent.Position == FloatingPosition.TopLeft && GlobalState.Dir == LocalizationDirection.LeftToRight)
                    {
                        CheckAngleRange(startAngle, endAngle, settings, isClock, 0, 90, false);
                        break;
                    }

                    CheckAngleRange(startAngle, endAngle, settings, isClock, 90, 180, false);
                }

                break;

            case FloatingPosition.TopCenter:
                {
                    settings.Offset = "70px";
                    CheckAngleRange(startAngle, endAngle, settings, isClock, 0, 180, false);
                }

                break;

            case FloatingPosition.MiddleLeft:
            case FloatingPosition.MiddleRight:
                {
                    settings.Offset = "70px";

                    if (FloatingPosition.MiddleLeft == Parent.Position && GlobalState.Dir == LocalizationDirection.LeftToRight)
                    {
                        var sa = startAngle < 0 || startAngle > 360 || startAngle > 90 && startAngle < 270 ? (isClock ? 270 : 90) : startAngle;
                        var ea = endAngle < 0 || endAngle > 360 || endAngle > 90 && endAngle < 270 ? (isClock ? 90 : 270) : endAngle;
                        var finalStartAngle = sa < 91 ? sa + 360 : sa;
                        var finalEndAngle = ea < 91 ? ea + 360 : ea;
                        var incorrectAngle = isClock && finalEndAngle < finalStartAngle || !isClock && finalEndAngle > finalStartAngle;
                        settings.StartAngle = incorrectAngle ? finalEndAngle : finalStartAngle;
                        settings.EndAngle = incorrectAngle ? finalStartAngle : finalEndAngle;
                        break;
                    }

                    CheckAngleRange(startAngle, endAngle, settings, isClock, 90, 270, false);
                }

                break;

            case FloatingPosition.MiddleCenter:
                {
                    settings.Offset = "70px";
                    var finalStartAngle = startAngle < 0 || startAngle > 360 ? (isClock ? 0 : 360) : startAngle;
                    var finalEndAngle = endAngle < 0 || endAngle > 360 ? (isClock ? 360 : 0) : endAngle;
                    settings.StartAngle = isClock || finalStartAngle > finalEndAngle ? finalStartAngle : finalStartAngle + 360;
                    settings.EndAngle = !isClock || finalEndAngle > finalStartAngle ? finalEndAngle : finalEndAngle + 360;
                }

                break;

            case FloatingPosition.BottomLeft:
            case FloatingPosition.BottomRight:
                {
                    if (FloatingPosition.BottomLeft == Parent.Position && GlobalState.Dir == LocalizationDirection.LeftToRight)
                    {
                        CheckAngleRange(startAngle, endAngle, settings, isClock, 270, 360, true);
                        break;
                    }

                    CheckAngleRange(startAngle, endAngle, settings, isClock, 180, 270, true);
                }

                break;

            case FloatingPosition.BottomCenter:
                {
                    settings.Offset = "70px";
                    CheckAngleRange(startAngle, endAngle, settings, isClock, 180, 360, true);
                }

                break;
        }

        settings.Direction = isClock ? SleekDialRadialDirection.Clockwise : SleekDialRadialDirection.Counterclockwise;

        return settings;
    }

    private static void CheckAngleRange(
        int startAngle,
        int endAngle,
        SleekDialRadialSettings settings,
        bool isClock,
        int minAngle,
        int maxAngle,
        bool reverse)
    {
        startAngle = CheckAngle(startAngle, isClock, minAngle, maxAngle, reverse);
        endAngle = CheckAngle(endAngle, !isClock, minAngle, maxAngle, reverse);
        var incorrectAngle = isClock && endAngle < startAngle || !isClock && endAngle > startAngle;
        settings.StartAngle = incorrectAngle ? endAngle : startAngle;
        settings.EndAngle = incorrectAngle ? startAngle : endAngle;
    }

    private static int CheckAngle(
        int value,
        bool isClock,
        int minAngle,
        int maxAngle,
        bool reverse)
    {
        if (value < 0 || value > 360)
        {
            return !isClock ? maxAngle : minAngle;
        }

        value = reverse ? (value == 0 ? 360 : value) : (value == 360 ? 0 : value);

        if (value >= minAngle &&
            value <= maxAngle)
        {
            return value;
        }

        return !isClock ? maxAngle : minAngle;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFilename);
            await _module.InvokeVoidAsync("initialize", Id, _popupReference, Parent?.FloatingButtonId, Parent?.Target, _linearPositionOptions, _radialPositionOptions);
            await UpdatePositionAsync();
            await Task.Delay(10);
            await UpdatePositionAsync();
        }
    }

    internal async Task AnimateOpenAsync(SleekDialAnimationSettings animationSettings)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("animateOpen", Id, animationSettings.Animation.ToString().ToLowerInvariant(), animationSettings.Duration.TotalMilliseconds, animationSettings.Delay.TotalMilliseconds);
        }
    }

    internal async Task AnimateCloseAsync(SleekDialAnimationSettings animationSettings)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("animateClose", Id, animationSettings.Animation.ToString().ToLowerInvariant(), animationSettings.Duration.TotalMilliseconds, animationSettings.Delay.TotalMilliseconds);
        }
    }

    [JSInvokable]
    public async Task OnAnimationCompletedAsync(bool isOpen)
    {
        if (OnAnimationCompleted.HasDelegate)
        {
            await OnAnimationCompleted.InvokeAsync(isOpen);
        }
    }

    [JSInvokable]
    public void RadialPositionUpdated(RectangleF rectangle)
    {
        _xOffset = rectangle.X;
        _yOffset = rectangle.Y;
        _width = rectangle.Width;
        _height = rectangle.Height;
        StateHasChanged();
    }
}

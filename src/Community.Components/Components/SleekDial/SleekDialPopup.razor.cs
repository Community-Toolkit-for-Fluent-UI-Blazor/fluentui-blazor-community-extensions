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
        public SleekDialLinearDirection Direction { get; set; }
    }

    private SleekDialRadialSettings? _radialSettings;
    private IJSObjectReference? _module;
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/SleekDial/SleekDialPopup.razor.js";
    private LinearPositionOptions _linearPositionOptions;
    private RadialPositionOptions _radialPositionOptions;
    private readonly DotNetObjectReference<SleekDialPopup> _popupReference;
    private bool _isLinear;

    /// <summary>
    /// Initializes a new instance of the <see cref="SleekDialPopup"/> class.
    /// </summary>
    public SleekDialPopup()
    {
        Id = Identifier.NewId();
        _popupReference = DotNetObjectReference.Create(this);
    }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default;

    [Parameter]
    public bool IsOpen { get; set; }

    [CascadingParameter]
    private FluentCxSleekDial? Parent { get; set; }

    [Parameter]
    public EventCallback<bool> OnAnimationCompleted { get; set; }

    private string? InternalCss => new CssBuilder(Class)
        .AddClass("sleekdial-popup")
        .AddClass("sleekdial-popup-hidden", !IsOpen)
        .AddClass("sleekdial-popup-linear-top", (_isLinear && _linearPositionOptions.IsTop) || (!_isLinear && _radialPositionOptions.IsTop))
        .AddClass("sleekdial-popup-linear-bottom",(_isLinear && !_linearPositionOptions.IsTop && !_linearPositionOptions.IsMiddle) || (!_isLinear && Parent!.Position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight)))
        .AddClass("sleekdial-popup-linear-left", _isLinear && _linearPositionOptions.IsLeft && !_linearPositionOptions.IsCenter)
        .AddClass("sleekdial-popup-linear-right", _isLinear && !_linearPositionOptions.IsLeft && !_linearPositionOptions.IsCenter)
        .AddClass("sleekdial-popup-linear-center", _isLinear && _linearPositionOptions.IsCenter)
        .AddClass("sleekdial-popup-linear-middle", _isLinear && _linearPositionOptions.IsMiddle)
        .AddClass("sleekdial-popup-linear-direction-left", _isLinear && _linearPositionOptions.Direction == SleekDialLinearDirection.Left)
        .AddClass("sleekdial-popup-linear-direction-right", _isLinear && _linearPositionOptions.Direction == SleekDialLinearDirection.Right)
        .AddClass("sleekdial-popup-linear-direction-down", _isLinear && _linearPositionOptions.IsTop)
        .Build();

    [Inject]
    public GlobalState GlobalState { get; set; } = default!;

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
                _radialPositionOptions.IsTop = position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight);
                _radialPositionOptions.IsBottom = position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight);
                _radialPositionOptions.IsLeft = position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.MiddleLeft, FloatingPosition.BottomLeft);
                _radialPositionOptions.IsCenter = position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter);
                _radialPositionOptions.IsMiddle = position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight);

            }
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFilename);
            await _module.InvokeVoidAsync("initialize", Id, _popupReference, Parent?.FloatingButtonId, Parent?.Target, _linearPositionOptions);
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
}

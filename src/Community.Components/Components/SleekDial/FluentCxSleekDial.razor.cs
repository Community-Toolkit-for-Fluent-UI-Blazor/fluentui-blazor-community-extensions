using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

public partial class FluentCxSleekDial
    : FluentComponentBase
{
    private bool _isOpen;
    private bool _preventDefault;
    private bool _linearDirectionChanged;
    private SleekDialPopup? _popup;
    private FluentCxFloatingButton? _floatingButton;
    private readonly RenderFragment _renderText;
    private IJSObjectReference? _module;
    private readonly DotNetObjectReference<FluentCxSleekDial> _dotNetObjectReference;
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/SleekDial/FluentCxSleekDial.razor.js";
    private static readonly List<KeyCode> _preventDefaultKeys =
    [
        KeyCode.Space,
        KeyCode.Enter,
        KeyCode.Escape,
        KeyCode.Up,
        KeyCode.Down,
        KeyCode.Right,
        KeyCode.Left,
        KeyCode.Home,
        KeyCode.End
    ];

    [Parameter]
    public RenderFragment<SleekDialItem>? ItemTemplate { get; set; }

    [Parameter]
    public SleekDialMode Mode { get; set; } = SleekDialMode.Linear;

    [Parameter]
    public FloatingPosition Position { get; set; } = FloatingPosition.BottomRight;

    [Parameter]
    public string? Target { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public Icon? CloseIcon { get; set; }

    [Parameter]
    public Icon? OpenIcon { get; set; }

    [Parameter]
    public bool OpensOnHover { get; set; }

    [Parameter]
    public string? Text { get; set; }

    [Parameter]
    public SleekDialLinearDirection Direction { get; set; } = SleekDialLinearDirection.Default;

    [Parameter]
    public EventCallback<SleekDialItem> ItemRendered { get; set; }

    [Parameter]
    public EventCallback<bool> IsVisibleChanged { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback Opened { get; set; }

    [Parameter]
    public EventCallback Closed { get; set; }

    [Parameter]
    public EventCallback Opening { get; set; }

    [Parameter]
    public EventCallback Closing { get; set; }

    [Parameter]
    public EventCallback<SleekDialItem> ItemSelected { get; set; }

    [Parameter]
    public bool IsModal { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    internal bool IsOpen => _isOpen;

    internal List<SleekDialItem> Items { get; private set; } = [];

    internal int FocusedIndex { get; set; } = -1;

    [Parameter]
    public SleekDialRadialSettings RadialSettings { get; set; } = new();

    [Parameter]
    public SleekDialAnimationSettings AnimationSettings { get; set; } = new();

    internal string? FloatingButtonId => _floatingButton?.Id;

    private string? InternalClass => new CssBuilder(Class).Build();

    /// <summary>
    /// Represents the correct radial settings after correction inside the popup.
    /// </summary>
    internal SleekDialRadialSettings CorrectRadialSettings { get; set; }

    [JSInvokable]
    public async Task OnClickAsync()
    {
        if (_popup is null || OpensOnHover || Disabled)
        {
            return;
        }

        await ShowOrHidePopupAsync(!_isOpen);
    }

    [JSInvokable]
    public async Task ShowOrHidePopupAsync(bool isOpen)
    {
        if (_isOpen == isOpen)
        {
            return;
        }

        if (isOpen && Opening.HasDelegate)
        {
            await Opening.InvokeAsync();
        }
        else if (!isOpen && Closing.HasDelegate)
        {
            await Closing.InvokeAsync();
        }

        if (AnimationSettings is not null &&
            AnimationSettings.Animation != SleekDialAnimation.None &&
            _popup is not null)
        {
            if (isOpen)
            {
                await _popup.AnimateOpenAsync(AnimationSettings);
            }
            else
            {
                await _popup.AnimateCloseAsync(AnimationSettings);
            }
        }
        else
        {
            await OnAnimationCompletedAsync(isOpen);
        }
    }

    private async Task OnKeyDownHandlerAsync(FluentKeyCodeEventArgs e)
    {
        _preventDefault = _preventDefaultKeys.Contains(e.Key);

        if (_popup is null)
        {
            return;
        }

        switch (e.Key)
        {
            case KeyCode.Space:
            case KeyCode.Enter:
                {
                    if (_isOpen && FocusedIndex != -1)
                    {
                        await OnItemClickAsync(Items[FocusedIndex]);
                    }

                    if (OpensOnHover)
                    {
                        return;
                    }

                    await ShowOrHidePopupAsync(!_isOpen);
                }
                break;

            case KeyCode.Escape:
                {
                    await ShowOrHidePopupAsync(false);
                }
                break;

            case KeyCode.Tab:
                {
                    FocusedIndex++;
                    FocusedIndex %= Items.Count;
                }
                break;

            default:
                {
                    if (!_isOpen || _popup is null)
                    {
                        return;
                    }

                    await _popup.HandleKeyAsync(e);
                }
                break;
        }
    }

    private async Task UpdatePopupPositionAsync()
    {
        if (_linearDirectionChanged && _popup is not null)
        {
            _linearDirectionChanged = false;
            await _popup.UpdatePositionAsync();
        }
    }

    internal async Task OnAnimationCompletedAsync(bool isOpen)
    {
        _isOpen = isOpen;
        await InvokeAsync(StateHasChanged);
    }

    internal void AddAnimationSettings(SleekDialAnimationSettings animationSettings)
    {
        AnimationSettings = animationSettings;
    }

    internal void AddChild(SleekDialItem value)
    {
        Items.Add(value);
    }

    internal void AddRadialSettings(SleekDialRadialSettings value)
    {
        RadialSettings = value;
    }

    internal async Task OnItemClickAsync(SleekDialItem sleekDialItem)
    {
        if (ItemSelected.HasDelegate)
        {
            await ItemSelected.InvokeAsync(sleekDialItem);
        }

        await ShowOrHidePopupAsync(false);
    }

    internal void RemoveAnimationSettings()
    {
        AnimationSettings = null;
    }

    internal void RemoveChild(SleekDialItem value)
    {
        Items.Remove(value);
    }

    internal void RemoveRadialSettings()
    {
        RadialSettings = null;
    }

    internal async Task OnCreatedAsync(SleekDialItem value)
    {
        if (ItemRendered.HasDelegate)
        {
            await ItemRendered.InvokeAsync(value);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFilename);
            await _module.InvokeVoidAsync("initialize", Id, _dotNetObjectReference);
            _linearDirectionChanged = true;
        }

        await UpdatePopupPositionAsync();
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(Direction), Direction))
        {
            _linearDirectionChanged = true;
        }
    }

    internal async Task FocusAsync()
    {
        await Items[FocusedIndex].Element.FocusAsync();
    }

    internal void UpdateItemsPosition()
    {
        foreach (var item in Items)
        {
            item.UpdateAngle();
        }
    }
}

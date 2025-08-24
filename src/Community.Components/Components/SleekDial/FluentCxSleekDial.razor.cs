using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a sleek dial.
/// </summary>
public partial class FluentCxSleekDial
    : FluentComponentBase
{
    /// <summary>
    /// Value indicating if the dial is open.
    /// </summary>
    private bool _isOpen;

    /// <summary>
    /// Represents if it's the first render.
    /// </summary>
    private bool _firstRender;

    /// <summary>
    /// Represents the correct radial settings to position the items on the radial panel.
    /// </summary>
    private SleekDialRadialSettings? _correctRadialSettings;

    /// <summary>
    /// Value indicating if the click is prevented.
    /// </summary>
    private bool _preventDefault;

    /// <summary>
    /// Value indicating if a direction change in linear mode.
    /// </summary>
    private bool _linearDirectionChanged;

    /// <summary>
    /// Represents the popup.
    /// </summary>
    private SleekDialPopup? _popup;

    /// <summary>
    /// Represents the floating button.
    /// </summary>
    private FluentCxFloatingButton? _floatingButton;

    /// <summary>
    /// Represents the fragment to render text.
    /// </summary>
    private readonly RenderFragment _renderText;

    /// <summary>
    /// Represents the javascript module.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the reference of the current dot net object.
    /// </summary>
    private readonly DotNetObjectReference<FluentCxSleekDial> _dotNetObjectReference;

    /// <summary>
    /// Represents the javascript file.
    /// </summary>
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/SleekDial/FluentCxSleekDial.razor.js";

    /// <summary>
    /// Represents the default keys to not prevent.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the item template.
    /// </summary>
    [Parameter]
    public RenderFragment<SleekDialItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the rendering mode of the <see cref="FluentCxSleekDial"/>.
    /// </summary>
    [Parameter]
    public SleekDialMode Mode { get; set; } = SleekDialMode.Linear;

    /// <summary>
    /// Gets or sets the position of the <see cref="FluentCxSleekDial"/>.
    /// </summary>
    [Parameter]
    public FloatingPosition Position { get; set; } = FloatingPosition.BottomRight;

    /// <summary>
    /// Gets or sets the target of the <see cref="FluentCxSleekDial"/>.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// Gets or sets if the <see cref="FluentCxSleekDial"/> is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the icon to close the popup.
    /// </summary>
    [Parameter]
    public Icon? CloseIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon to open the popup.
    /// </summary>
    [Parameter]
    public Icon? OpenIcon { get; set; }

    /// <summary>
    /// Gets or sets if the popup opens on hover.
    /// </summary>
    [Parameter]
    public bool OpensOnHover { get; set; }

    /// <summary>
    /// Gets or sets the text of the <see cref="FluentCxSleekDial"/>.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the linear direction of the items.
    /// </summary>
    [Parameter]
    public SleekDialLinearDirection Direction { get; set; } = SleekDialLinearDirection.Default;

    /// <summary>
    /// Gets or sets the callback when an item is rendered.
    /// </summary>
    [Parameter]
    public EventCallback<SleekDialItem> ItemRendered { get; set; }

    /// <summary>
    /// Gets or sets the callback when the visibility has changed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsVisibleChanged { get; set; }

    /// <summary>
    /// Gets or sets the child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the callback when the popup is opened.
    /// </summary>
    [Parameter]
    public EventCallback Opened { get; set; }

    /// <summary>
    /// Gets or sets the callback when the popup is closed.
    /// </summary>
    [Parameter]
    public EventCallback Closed { get; set; }

    /// <summary>
    /// Gets or sets the callback when the popup is opening.
    /// </summary>
    [Parameter]
    public EventCallback Opening { get; set; }

    /// <summary>
    /// Gets or sets the callback when the popup is closing.
    /// </summary>
    [Parameter]
    public EventCallback Closing { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if the <see cref="FluentCxSleekDial"/> is modal.
    /// </summary>
    [Parameter]
    public bool IsModal { get; set; }

    /// <summary>
    /// Gets or sets the javascript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary>
    /// Gets if the popup is open.
    /// </summary>
    internal bool IsOpen => _isOpen;

    /// <summary>
    /// Gets the <see cref="SleekDialItem"/> inside this instance of <see cref="FluentCxSleekDial"/>.
    /// </summary>
    internal List<SleekDialItem> InternalItems { get; private set; } = [];

    /// <summary>
    /// Gets or sets the viewer of the dial.
    /// </summary>
    internal SleekDialView Viewer { get; set; } = default!;

    /// <summary>
    /// Gets or sets the focused index.
    /// </summary>
    internal int FocusedIndex { get; set; } = -1;

    /// <summary>
    /// Gets or sets the settings of the radial menu.
    /// </summary>
    [Parameter]
    public SleekDialRadialSettings RadialSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets the settings of the animation.
    /// </summary>
    [Parameter]
    public SleekDialAnimationSettings AnimationSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating if the dial stay open when an item is clicked.
    /// </summary>
    [Parameter]
    public bool StayOpen { get; set; }

    /// <summary>
    /// Gets or sets the hide mode of the dial.
    /// </summary>
    [Parameter]
    public SleekDialHideMode HideMode { get; set; } = SleekDialHideMode.None;

    /// <summary>
    /// Gets the identifier of the <see cref="FluentCxFloatingButton"/>.
    /// </summary>
    internal string? FloatingButtonId => _floatingButton?.Id;

    /// <summary>
    /// Gets the internal css.
    /// </summary>
    private string? InternalClass => new CssBuilder(Class).Build();

    /// <summary>
    /// Represents the correct radial settings after correction inside the popup.
    /// </summary>
    internal SleekDialRadialSettings? CorrectRadialSettings
    {
        get => _correctRadialSettings;
        set
        { 
            _correctRadialSettings = value;
            RadialSettingsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Event raised when a radial settings has changed.
    /// </summary>
    internal event EventHandler? RadialSettingsChanged;

    /// <summary>
    /// Gets a value indicating if the dial is visible.
    /// </summary>
    private bool IsVisible
    {
        get
        {
            return HideMode switch
            {
                SleekDialHideMode.None => true,
                SleekDialHideMode.WhenEmpty => !_firstRender || InternalItems.Count > 0,
                SleekDialHideMode.WhenNoVisible => !_firstRender || InternalItems.Any(i => i.IsVisible),
                SleekDialHideMode.WhenEmptyOrNoVisible => !_firstRender || InternalItems.Count > 0 && InternalItems.Any(i => i.IsVisible),
                _ => true,
            };
        }
    }

    /// <summary>
    /// Occurs on a click on the <see cref="FluentCxFloatingButton"/>.
    /// </summary>
    /// <returns>Returns a task which show or hide the dial when completed.</returns>
    [JSInvokable]
    public async Task OnClickAsync()
    {
        if (_popup is null || OpensOnHover || Disabled)
        {
            return;
        }

        await ShowOrHidePopupInternalAsync(!_isOpen, false);
    }

    /// <summary>
    /// Shows or hides the dial in an asynchronous way.
    /// </summary>
    /// <param name="isOpen">Value indicating if the dial is open or hide.</param>
    /// <returns>Returns a task which hides or shows the dial when completed.</returns>
    [JSInvokable]
    public async Task ShowOrHidePopupAsync(bool isOpen)
    {
        await ShowOrHidePopupInternalAsync(isOpen, StayOpen);
    }

    /// <summary>
    /// Shows or hides the dial in an asynchronous way.
    /// </summary>
    /// <param name="isOpen">Value indicating if the dial is open or hide.</param>
    /// <param name="stayOpen">Value indicating if the dial stay open when the item is clicked.</param>
    /// <returns>Returns a task which shows or hides the popup when completed.</returns>
    private async Task ShowOrHidePopupInternalAsync(bool isOpen, bool stayOpen)
    {
        if (stayOpen && _isOpen)
        {
            return;
        }

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

    /// <summary>
    /// Occurs when a key is tapped.
    /// </summary>
    /// <param name="e">Event args associated to the method.</param>
    /// <returns>Returns a task which handles the pressed key when completed.</returns>
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
                        await InternalItems[FocusedIndex].OnClickAsync();
                    }

                    if (OpensOnHover)
                    {
                        return;
                    }

                    await ShowOrHidePopupInternalAsync(!_isOpen, false);
                }

                break;

            case KeyCode.Escape:
                {
                    await ShowOrHidePopupInternalAsync(false, false);
                }

                break;

            case KeyCode.Tab:
                {
                    FocusedIndex++;
                    FocusedIndex %= InternalItems.Count;
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

    /// <summary>
    /// Update the position of the popup in an asynchronous way.
    /// </summary>
    /// <returns></returns>
    private async Task UpdatePopupPositionAsync()
    {
        if (_linearDirectionChanged && _popup is not null)
        {
            _linearDirectionChanged = false;
            await _popup.UpdatePositionAsync();
        }
    }

    /// <summary>
    /// Occurs when an animation is completed.
    /// </summary>
    /// <param name="isOpen">Value indicating if the dial is shown or hidden.</param>
    /// <returns>Return a task which rerender the component when completed.</returns>
    internal async Task OnAnimationCompletedAsync(bool isOpen)
    {
        _isOpen = isOpen;
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Adds a child into the component.
    /// </summary>
    /// <param name="value">Item to add.</param>
    internal void AddChild(SleekDialItem value)
    {
        InternalItems.Add(value);
        StateHasChanged();
    }

    /// <summary>
    /// Removes a child from the component.
    /// </summary>
    /// <param name="value">Item to remove.</param>
    internal void RemoveChild(SleekDialItem value)
    {
        InternalItems.Remove(value);
        StateHasChanged();
    }

    /// <summary>
    /// Occurs when an item is created.
    /// </summary>
    /// <param name="value">Represents the created item.</param>
    /// <returns>Returns a task which invokes the <see cref="ItemRendered"/> callback.</returns>
    internal async Task OnCreatedAsync(SleekDialItem value)
    {
        if (ItemRendered.HasDelegate)
        {
            await ItemRendered.InvokeAsync(value);
        }
    }

    /// <summary>
    /// Focus the selected index.
    /// </summary>
    /// <returns>Returns a task which focus the selected element when completed.</returns>
    internal async Task FocusAsync()
    {
        await InternalItems[FocusedIndex].Element.FocusAsync();
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            _firstRender = true;
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _linearDirectionChanged = parameters.HasValueChanged(nameof(Direction), Direction);

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Refreshes the <see cref="FluentCxSleekDial"/>.
    /// </summary>
    public void Refresh()
    {
        Viewer?.Refresh();
        StateHasChanged();
    }
}

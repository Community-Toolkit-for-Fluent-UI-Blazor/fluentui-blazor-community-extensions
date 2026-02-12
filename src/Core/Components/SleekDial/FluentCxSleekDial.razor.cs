using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a customizable, interactive dial component that provides floating action and selection capabilities
/// within a user interface.
/// </summary>
/// <remarks>The FluentCxSleekDial component supports a variety of configuration options, including visual style,
/// position, animation, and interaction modes. It enables developers to define custom item templates, handle rendering
/// and visibility events, and control the dial's behavior through parameters such as modal state, hover activation, and
/// hide modes. This component is suitable for scenarios where a modern, floating dial or menu is required to enhance
/// user experience and streamline access to actions or options.</remarks>
public partial class FluentCxSleekDial
{
    private bool _isOpen;
    private int _popupWidth;
    private int _popupHeight;
    private FluentCxPopupPlacement? _popupRef;
    private int _fabWidth;
    private int _fabHeight;
    private bool _hasLinearSettingsChanged;

    /// <summary>
    /// Initializes a new instance of the FluentCxSleekDial class using the specified library configuration.
    /// </summary>
    /// <remarks>This constructor assigns a unique identifier to the dial and configures the rendering logic
    /// to display the dial's text content if the Text property is set.</remarks>
    /// <param name="configuration">The configuration settings that define the behavior and properties of the dial. Cannot be null.</param>
    public FluentCxSleekDial(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the mode of the sleek dial, which determines its visual style and interaction behavior.
    /// </summary>
    /// <remarks>The default value is <see cref="SleekDialMode.Linear"/>, which provides a linear visual
    /// representation. Other modes may offer alternative visual styles and user interactions.</remarks>
    [Parameter]
    public SleekDialMode Mode { get; set; } = SleekDialMode.Linear;

    /// <summary>
    /// Gets or sets the position of the floating element relative to its parent container.
    /// </summary>
    /// <remarks>The default value is <see cref="FloatingPosition.BottomRight"/>. Use this property to control
    /// where the floating element appears on the screen, such as top-left, bottom-right, or other supported
    /// positions.</remarks>
    [Parameter]
    public FloatingPosition Position { get; set; } = FloatingPosition.BottomRight;

    /// <summary>
    /// Gets or sets a value indicating whether the component is disabled and cannot be interacted with.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the component is rendered in a disabled state, preventing
    /// user interaction. Use this property to control the availability of the component based on application
    /// logic.</remarks>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed for closing the component.
    /// </summary>
    /// <remarks>If not set, a default close icon is used. The icon should be of type <see cref="Icon"/> to
    /// ensure proper rendering.</remarks>
    [Parameter]
    public Icon? CloseIcon { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed when the item is in an open state.
    /// </summary>
    /// <remarks>This property allows customization of the icon shown for open items. If not set, a default
    /// icon will be used.</remarks>
    [Parameter]
    public Icon? OpenIcon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the content opens when the user hovers over the component.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the content is displayed in response to a pointer hover
    /// event, rather than requiring a click. This behavior is commonly used for tooltips, dropdowns, or other
    /// interactive UI elements to enhance usability.</remarks>
    [Parameter] public bool OpensOnHover { get; set; }

    /// <summary>
    /// Gets or sets the text content to be displayed.
    /// </summary>
    /// <remarks>This property can be set to null, in which case no text will be displayed. Ensure that the
    /// text is properly formatted for the intended display context.</remarks>
    [Parameter] public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the direction of the dial, which determines its orientation and behavior.
    /// </summary>
    /// <remarks>The default value is SleekDialLinearDirection.Default, which provides a standard orientation.
    /// Changing this property may affect the visual representation and interaction of the dial.</remarks>
    [Parameter] public SleekDialLinearDirection Direction { get; set; } = SleekDialLinearDirection.Default;

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback Opening { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback Opened { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback Closing { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public EventCallback Closed { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public bool IsModal { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public SleekDialLinearSettings LinearSettings { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public SleekDialRadialSettings RadialSettings { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public SleekDialAnimationSettings AnimationSettings { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public bool StayOpen { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public SleekDialHideMode HideMode { get; set; } = SleekDialHideMode.None;

    /// <summary>
    /// 
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is in a restricted area.
    /// </summary>
    [Parameter]
    public bool RestrictedArea { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal List<SleekDialItem> InternalItems { get; private set; } = [];

    /// <summary>
    /// 
    /// </summary>
    internal List<SleekDialItemLayout> Layouts { get; private set; } = [];

    internal int FocusedIndex { get; set; } = -1;

    internal bool IsOpen => _isOpen;

    private bool IsDialVisible => HideMode switch
    {
        SleekDialHideMode.None => true,

        SleekDialHideMode.WhenEmpty =>
            InternalItems.Count > 0,

        SleekDialHideMode.WhenNoVisible =>
            InternalItems.Any(i => i.IsVisible),

        SleekDialHideMode.WhenEmptyOrNoVisible =>
            InternalItems.Count > 0 &&
            InternalItems.Any(i => i.IsVisible),

        _ => true
    };

    private string? InternalClass => DefaultClassBuilder.Build();

    private PopupPlacement PopupPlacementForDial => Mode == SleekDialMode.Linear ? MapLinearDirectionToPopupPlacement(Direction) : PopupPlacement.BottomCenter;

    private static PopupPlacement MapLinearDirectionToPopupPlacement(SleekDialLinearDirection direction)
    {
        return direction switch
        {
            SleekDialLinearDirection.Left => PopupPlacement.LeftCenter,
            SleekDialLinearDirection.Right => PopupPlacement.RightCenter,
            SleekDialLinearDirection.Up => PopupPlacement.TopCenter,
            SleekDialLinearDirection.Down => PopupPlacement.BottomCenter,
            _ => PopupPlacement.Auto
        };
    }

    private static PreferredPopupDirection MapDirectionToPreferred(SleekDialLinearDirection dir)
    {
        return dir switch
        {
            SleekDialLinearDirection.Left => PreferredPopupDirection.Left,
            SleekDialLinearDirection.Right => PreferredPopupDirection.Right,
            SleekDialLinearDirection.Up => PreferredPopupDirection.Up,
            SleekDialLinearDirection.Down => PreferredPopupDirection.Down,
            _ => PreferredPopupDirection.Default
        };
    }

    private static AnchorLogicalPosition MapFloatingPosition(FloatingPosition pos)
    {
        return pos switch
        {
            FloatingPosition.TopLeft => AnchorLogicalPosition.TopLeft,
            FloatingPosition.TopCenter => AnchorLogicalPosition.TopCenter,
            FloatingPosition.TopRight => AnchorLogicalPosition.TopRight,

            FloatingPosition.MiddleLeft => AnchorLogicalPosition.MiddleLeft,
            FloatingPosition.MiddleCenter => AnchorLogicalPosition.MiddleCenter,
            FloatingPosition.MiddleRight => AnchorLogicalPosition.MiddleRight,

            FloatingPosition.BottomLeft => AnchorLogicalPosition.BottomLeft,
            FloatingPosition.BottomCenter => AnchorLogicalPosition.BottomCenter,
            FloatingPosition.BottomRight => AnchorLogicalPosition.BottomRight,

            _ => AnchorLogicalPosition.BottomRight
        };
    }

    internal async Task OnFloatingButtonClickAsync()
    {
        if (Disabled)
        {
            return;
        }

        if (OpensOnHover)
        {
            return;
        }

        await ShowOrHidePopupAsync(!_isOpen);
    }

    internal async Task OnKeyDownHandlerAsync(FluentKeyCodeEventArgs e)
    {
        if (Disabled)
        {
            return;
        }

        switch (e.Key)
        {
            case KeyCode.Space:
            case KeyCode.Enter:
                if (!_isOpen)
                {
                    await ShowOrHidePopupAsync(true);
                }
                else if (FocusedIndex != -1)
                {
                    await InternalItems[FocusedIndex].OnClickAsync();
                }

                break;

            case KeyCode.Escape:
                await ShowOrHidePopupAsync(false);

                break;
        }
    }

    internal async Task ShowOrHidePopupAsync(bool isOpen)
    {
        if (Disabled)
        {
            return;
        }

        if (StayOpen && _isOpen)
        {
            return;
        }

        if (_isOpen == isOpen)
        {
            return;
        }

        if (isOpen)
        {
            if (Opening.HasDelegate)
            {
                await Opening.InvokeAsync();
            }

            ComputeLayout();

            _isOpen = true;

            if (Opened.HasDelegate)
            {
                await Opened.InvokeAsync();
            }
        }
        else
        {
            if (Closing.HasDelegate)
            {
                await Closing.InvokeAsync();
            }

            _isOpen = false;

            if (Closed.HasDelegate)
            {
                await Closed.InvokeAsync();
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    internal async Task OnHoverAsync(bool entering)
    {
        if (!OpensOnHover || Disabled)
        {
            return;
        }

        await ShowOrHidePopupAsync(entering);
    }

    internal async Task HandleNavigationKeyAsync(FluentKeyCodeEventArgs e)
    {
        if (!_isOpen)
        {
            return;
        }

        switch (e.Key)
        {
            case KeyCode.Home:
                FocusedIndex = FirstVisibleIndex();
                break;

            case KeyCode.End:
                FocusedIndex = LastVisibleIndex();
                break;

            case KeyCode.Up:
            case KeyCode.Left:
                FocusedIndex = PreviousVisibleIndex(FocusedIndex);
                break;

            case KeyCode.Down:
            case KeyCode.Right:
                FocusedIndex = NextVisibleIndex(FocusedIndex);
                break;

            case KeyCode.Enter:
            case KeyCode.Space:
                if (FocusedIndex != -1)
                {
                    await InternalItems[FocusedIndex].OnClickAsync();
                }

                break;

            case KeyCode.Escape:
                await ShowOrHidePopupAsync(false);
                break;
        }

        await InvokeAsync(StateHasChanged);
    }

    private int FirstVisibleIndex() => InternalItems.FindIndex(i => i.IsVisible);

    private int LastVisibleIndex() => InternalItems.FindLastIndex(i => i.IsVisible);

    private int NextVisibleIndex(int index)
    {
        for (var i = index + 1; i < InternalItems.Count; i++)
        {
            if (InternalItems[i].IsVisible)
            {
                return i;
            }
        }

        return index;
    }

    private int PreviousVisibleIndex(int index)
    {
        for (var i = index - 1; i >= 0; i--)
        {
            if (InternalItems[i].IsVisible)
            {
                return i;
            }
        }

        return index;
    }

    internal void AddChild(SleekDialItem item)
    {
        InternalItems.Add(item);
        ComputeLayout(true);
        StateHasChanged();
    }

    internal void RemoveChild(SleekDialItem item)
    {
        InternalItems.Remove(item);
        ComputeLayout(true);
        StateHasChanged();
    }

    private void ComputeLayout(bool reset = false)
    {
        var result = SleekDialLayoutEngine.ComputeLayout(
            InternalItems,
            itemSize: 40,
            Mode,
            linearSettings: LinearSettings,
            linearDirection: Direction,
            radialSettings: RadialSettings,
            animation: AnimationSettings,
            position: Position,
            width: Mode == SleekDialMode.Radial ? 0 : reset ? 0 : _popupWidth,
            height: Mode == SleekDialMode.Radial ? 0 : reset ? 0 : _popupHeight,
            fabWidth: _fabWidth,
            fabHeight: _fabHeight);

        Layouts = result.Layouts;
        _popupWidth = result.PopupWidth;
        _popupHeight = result.PopupHeight;
    }

    private async Task OnOverlayClick()
    {
        if (IsModal)
        {
            await ShowOrHidePopupAsync(false);
        }
    }

    /// <inheritdoc />
    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            ComputeLayout();
            StateHasChanged();
        }
    }

    private async Task OnPositionChangedAsync(FloatingPosition position)
    {
        Position = position;
        ComputeLayout();

        await InvokeAsync(StateHasChanged);

        if (_popupRef is not null)
        {
            await _popupRef.UpdatePositionAsync();
        }
    }

    private void OnPositionUpdated(PopupPlacementResult result)
    {
        _fabWidth = result.AnchorSize.Width;
        _fabHeight = result.AnchorSize.Height;
        _popupWidth = result.PopupSize.Width;
        _popupHeight = result.PopupSize.Height;
        ComputeLayout(true);
        StateHasChanged();
    }

    private string GetGap()
    {
        return Mode switch
        {
            SleekDialMode.Linear => LinearSettings.Gap,
            _ => "0px"
        };
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasLinearSettingsChanged)
        {
            LinearSettings.OnSettingsChanged += OnLinearSettingsChanged;
        }
    }

    private void OnLinearSettingsChanged(object? sender, (string, string) e)
    {
        if (e.Item1 == nameof(SleekDialLinearSettings.ItemOffset))
        {
            ComputeLayout(true);
            StateHasChanged();
        }
        else if (e.Item1 == nameof(SleekDialLinearSettings.Gap))
        {
            InvokeAsync(async () =>
            {
                if (_popupRef is not null)
                {
                    await _popupRef.UpdateGapAsync(LinearSettings.Gap);
                    await _popupRef.UpdatePositionAsync();
                }

                await InvokeAsync(StateHasChanged);
            });
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasLinearSettingsChanged = parameters.TryGetValue<SleekDialLinearSettings>(nameof(LinearSettings), out var newValue) && newValue != LinearSettings;

        if (_hasLinearSettingsChanged)
        {
            LinearSettings.OnSettingsChanged -= OnLinearSettingsChanged;
        }

        return base.SetParametersAsync(parameters);
    }
}

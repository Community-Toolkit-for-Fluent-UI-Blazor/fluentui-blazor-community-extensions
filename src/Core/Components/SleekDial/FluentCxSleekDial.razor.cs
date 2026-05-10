using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
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
    private static readonly Icon s_openIcon = new Size24.List();

    private static readonly Icon s_closeIcon = new Size24.Dismiss();

    /// <summary>
    /// Represents a value indicating whether the dial is currently open.
    /// </summary>
    private bool _isOpen;

    /// <summary>
    /// Represents the width of the popup.
    /// </summary>
    private int _popupWidth;

    /// <summary>
    /// Represents the height of the popup.
    /// </summary>
    private int _popupHeight;

    /// <summary>
    /// Represents the popup element.
    /// </summary>
    private FluentCxPopupPlacement? _popupRef;

    /// <summary>
    /// Represents the width of the floating action button element.
    /// </summary>
    private int _fabWidth;

    /// <summary>
    /// Represents the height of the floating action button element.
    /// </summary>
    private int _fabHeight;

    /// <summary>
    /// Represents a value indicating whether the linear settings have changed and require re-subscription to change events.
    /// </summary>
    private bool _hasLinearSettingsChanged;

    /// <summary>
    /// Represents a value indicating whether the radial settings have changed and require re-subscription to change events.
    /// </summary>
    private bool _hasRadialSettingsChanged;

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
    public Icon? CloseIcon { get; set; } = s_closeIcon;

    /// <summary>
    /// Gets or sets the icon displayed when the item is in an open state.
    /// </summary>
    /// <remarks>This property allows customization of the icon shown for open items. If not set, a default
    /// icon will be used.</remarks>
    [Parameter]
    public Icon? OpenIcon { get; set; } = s_openIcon;

    /// <summary>
    /// Gets or sets the text content to be displayed.
    /// </summary>
    /// <remarks>This property can be set to null, in which case no text will be displayed. Ensure that the
    /// text is properly formatted for the intended display context.</remarks>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the direction of the dial, which determines its orientation and behavior.
    /// </summary>
    /// <remarks>The default value is SleekDialLinearDirection.Default, which provides a standard orientation.
    /// Changing this property may affect the visual representation and interaction of the dial.</remarks>
    [Parameter]
    public SleekDialLinearDirection Direction { get; set; } = SleekDialLinearDirection.Default;

    /// <summary>
    /// Gets or sets the event callback that is triggered when the dial is about to open.
    /// </summary>
    [Parameter]
    public EventCallback Opening { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is triggered after the dial has opened.
    /// </summary>
    [Parameter]
    public EventCallback Opened { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is triggered when the dial is about to close.
    /// </summary>
    [Parameter]
    public EventCallback Closing { get; set; }

    /// <summary>
    /// Getsor sets the event callback that is triggered after the dial has closed.
    /// </summary>
    [Parameter]
    public EventCallback Closed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dial should be treated as a modal element.
    /// </summary>
    [Parameter]
    public bool IsModal { get; set; }

    /// <summary>
    /// Gets or sets the settings specific to the linear mode of the dial, which control its layout and behavior when in linear mode.
    /// </summary>
    [Parameter]
    public SleekDialLinearSettings LinearSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets the settings specific to the radial mode of the dial, which control its layout and behavior when in radial mode.
    /// </summary>
    [Parameter]
    public SleekDialRadialSettings RadialSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets the animation settings for the dial, which control the visual effects applied during opening, closing, and item interactions.
    /// </summary>
    [Parameter]
    public SleekDialAnimationSettings AnimationSettings { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the dial should remain open after an item is clicked.
    /// </summary>
    [Parameter]
    public bool StayOpen { get; set; }

    /// <summary>
    /// Gets or sets the hide mode for the dial, which determines the conditions under which the dial should be hidden from view.
    /// </summary>
    [Parameter]
    public SleekDialHideMode HideMode { get; set; } = SleekDialHideMode.None;

    /// <summary>
    /// Gets or sets the content to be rendered inside the dial, allowing for custom item templates and additional UI elements.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is in a restricted area.
    /// </summary>
    [Parameter]
    public bool RestrictedArea { get; set; }

    /// <summary>
    /// Gets or sets the width of the dial when in linear mode.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dial should be rendered as a fullscreen overlay.
    /// </summary>
    [Parameter]
    public bool FullscreenModal { get; set; }

    /// <summary>
    /// Gets the internal list of items contained within the dial, which are represented as <see cref="SleekDialItem"/> instances. This collection is used to manage the items that are displayed in the dial and their associated properties such as visibility, click handlers, and layout information.
    /// </summary>
    internal List<SleekDialItem> InternalItems { get; private set; } = [];

    /// <summary>
    /// Gets the list of layout information for each item in the dial, which is computed based on the current mode, settings, and item properties.
    /// This layout information is used to position and animate the items correctly within the dial when it is rendered.
    /// </summary>
    internal List<SleekDialItemLayout> Layouts { get; private set; } = [];

    /// <summary>
    /// Gets a value indicating whether the component is currently open.
    /// </summary>
    internal bool IsOpen => _isOpen;

    /// <summary>
    /// Gets if there are any visible items in the internal items collection.
    /// </summary>
    private bool HasVisibleItems
    {
        get
        {
            var items = InternalItems;

            for (var i = 0; i < items.Count; i++)
            {
                if (items[i].IsVisible)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the dial is currently visible based on the configured hide mode and the state of
    /// the internal items.
    /// </summary>
    /// <remarks>The visibility of the dial is determined by the value of the HideMode property and the
    /// contents of the internal items collection. Different hide modes may cause the dial to be hidden when there are
    /// no items, when no items are visible, or under other specific conditions.</remarks>
    private bool IsDialVisible => HideMode switch
    {
        SleekDialHideMode.None => true,
        SleekDialHideMode.WhenEmpty => InternalItems.Count > 0,
        SleekDialHideMode.WhenNoVisible => HasVisibleItems,
        SleekDialHideMode.WhenEmptyOrNoVisible => HasVisibleItems,
        _ => true
    };

    /// <summary>
    /// Gets the internally generated CSS class string for the component.
    /// </summary>
    private string? InternalStyle => DefaultStyleBuilder
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Gets the popup placement used for the dial based on the current mode and direction.
    /// </summary>
    /// <remarks>When the mode is set to linear, the popup placement is determined by mapping the dial's
    /// direction. Otherwise, the popup is placed at the bottom center. This property is intended for internal layout
    /// logic and is not user-settable.</remarks>
    private PopupPlacement PopupPlacementForDial => Mode == SleekDialMode.Linear ? SleekDialMapper.MapLinearDirectionToPopupPlacement(Direction) : PopupPlacement.BottomCenter;

    /// <summary>
    /// Handles changes to the radial settings and updates the component layout accordingly.
    /// </summary>
    /// <param name="sender">The source of the event that triggered the settings change.</param>
    /// <param name="e">An object that contains the event data.</param>
    private void OnRadialSettingsChanged(object? sender, EventArgs e)
    {
        ComputeLayout(true);
        StateHasChanged();
    }

    /// <summary>
    /// Handles changes to linear settings and updates the component layout or popup position as needed.
    /// </summary>
    /// <remarks>This method responds to changes in specific linear settings by recalculating the layout or
    /// updating the popup's gap and position. It ensures the component reflects the latest configuration.</remarks>
    /// <param name="sender">The source of the event that triggered the settings change.</param>
    /// <param name="e">A tuple containing the name of the changed setting and its new value.</param>
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

    /// <summary>
    /// Handles the click event for the floating button, toggling the popup's visibility if the button is enabled and
    /// not configured to open on hover.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task OnFloatingButtonClickAsync()
    {
        if (Disabled)
        {
            return;
        }

        await ShowOrHidePopupAsync(!_isOpen);
    }

    /// <summary>
    /// Handles key down events to control the popup and item selection based on keyboard input.
    /// </summary>
    /// <remarks>Handles the Space, Enter, and Escape keys to open or close the popup and trigger item actions
    /// as appropriate. No action is taken if the component is disabled.</remarks>
    /// <param name="e">The event data containing information about the key that was pressed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

                break;

            case KeyCode.Escape:
                await ShowOrHidePopupAsync(false);

                break;
        }
    }

    /// <summary>
    /// Opens or closes the popup asynchronously based on the specified state.
    /// </summary>
    /// <remarks>This method triggers the appropriate opening or closing events and updates the popup's state.
    /// If the popup is disabled, already in the requested state, or configured to stay open, no action is
    /// taken.</remarks>
    /// <param name="isOpen">true to open the popup; false to close it.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles navigation key events to control the visibility of the popup based on user input.
    /// </summary>
    /// <remarks>This method processes navigation keys such as Escape to close the popup when it is open. It
    /// should be called in response to relevant keyboard events to ensure proper popup behavior.</remarks>
    /// <param name="e">The event arguments containing information about the key pressed, used to determine the navigation action.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    internal async Task HandleNavigationKeyAsync(FluentKeyCodeEventArgs e)
    {
        if (!_isOpen)
        {
            return;
        }

        switch (e.Key)
        {
            case KeyCode.Escape:
                await ShowOrHidePopupAsync(false);
                break;
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Adds a child item to the internal collection of items and updates the layout and rendering of the component accordingly.
    /// </summary>
    /// <param name="item">Item to add.</param>
    internal void AddChild(SleekDialItem item)
    {
        InternalItems.Add(item);
        ComputeLayout(true);
        StateHasChanged();
    }

    /// <summary>
    /// Removes the specified child item from the collection of dial items.
    /// </summary>
    /// <remarks>If the specified item is not present in the collection, no action is taken.</remarks>
    /// <param name="item">The child item to remove from the collection. Cannot be null.</param>
    internal void RemoveChild(SleekDialItem item)
    {
        InternalItems.Remove(item);
        ComputeLayout(true);
        StateHasChanged();
    }

    /// <summary>
    /// Calculates and updates the layout for the dial component based on the current mode, settings, and item
    /// collection.
    /// </summary>
    /// <remarks>This method updates the internal layout state and popup dimensions according to the current
    /// configuration. It should be called whenever the dial's items or settings change to ensure the layout remains
    /// accurate.</remarks>
    /// <param name="reset">true to reset the layout dimensions before computing the layout; otherwise, false to retain the current
    /// dimensions.</param>
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

    /// <summary>
    /// Handles the click event on the overlay and closes the popup if it is modal.
    /// </summary>
    /// <remarks>This method is typically invoked when the user clicks outside the modal content area. It only
    /// closes the popup if the current popup is modal.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnOverlayClick()
    {
        if (IsModal)
        {
            await ShowOrHidePopupAsync(false);
        }
    }

    /// <summary>
    /// Handles updates to the floating element's position and triggers layout recalculation and UI refresh.
    /// </summary>
    /// <remarks>This method updates the component's state and layout in response to a position change, and
    /// ensures the UI is refreshed. If a popup reference is available, its position is also updated
    /// asynchronously.</remarks>
    /// <param name="position">The new position to apply to the floating element.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Updates the layout and state of the popup based on the latest placement result.
    /// </summary>
    /// <remarks>Call this method when the popup's position or size changes to ensure the layout and visual
    /// state remain consistent.</remarks>
    /// <param name="result">The result containing the updated anchor and popup size information used to recalculate the layout.</param>
    private void OnPositionUpdated(PopupPlacementResult result)
    {
        _fabWidth = result.AnchorSize.Width;
        _fabHeight = result.AnchorSize.Height;
        _popupWidth = result.PopupSize.Width;
        _popupHeight = result.PopupSize.Height;
        ComputeLayout(true);
        StateHasChanged();
    }

    /// <summary>
    /// Retrieves the gap value to use based on the current dial mode.
    /// </summary>
    /// <returns>A string representing the gap value. Returns the value from LinearSettings.Gap if the mode is linear; otherwise,
    /// returns "0px".</returns>
    private string GetGap()
    {
        return Mode switch
        {
            SleekDialMode.Linear => LinearSettings.Gap,
            _ => "0px"
        };
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

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasLinearSettingsChanged)
        {
            LinearSettings.OnSettingsChanged += OnLinearSettingsChanged;
        }

        if (_hasRadialSettingsChanged)
        {
            RadialSettings.OnSettingsChanged += OnRadialSettingsChanged;
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasLinearSettingsChanged = parameters.TryGetValue<SleekDialLinearSettings>(nameof(LinearSettings), out var newValue) && newValue != LinearSettings;
        _hasRadialSettingsChanged = parameters.TryGetValue<SleekDialRadialSettings>(nameof(RadialSettings), out var newRadialValue) && newRadialValue != RadialSettings;

        if (_hasLinearSettingsChanged)
        {
            LinearSettings.OnSettingsChanged -= OnLinearSettingsChanged;
        }

        if (_hasRadialSettingsChanged)
        {
            RadialSettings.OnSettingsChanged -= OnRadialSettingsChanged;
        }

        return base.SetParametersAsync(parameters);
    }
}

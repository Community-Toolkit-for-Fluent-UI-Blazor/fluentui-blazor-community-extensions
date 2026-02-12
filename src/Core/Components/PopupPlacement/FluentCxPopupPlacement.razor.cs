using System.Globalization;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that manages the placement of a popup element relative to an anchor element.
/// This component calculates the optimal position for the popup based on the specified placement,
///  preferred direction, and other parameters, ensuring that the popup is displayed in a visually appropriate
///  location while considering factors such as available space and viewport boundaries.
/// </summary>
public partial class FluentCxPopupPlacement : FluentComponentBase
{
    /// <summary>
    /// Represents the left coordinate of the popup element in pixels.
    /// </summary>
    private int _left;

    /// <summary>
    /// Represents the top coordinate of the popup element in pixels.
    /// </summary>
    private int _top;

    /// <summary>
    /// Represents a reference to the JavaScript module responsible for handling popup placement logic on the client side.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Holds a reference to a .NET object for JavaScript interop with the FluentCxPopupPlacement component.
    /// </summary>
    /// <remarks>This reference enables JavaScript code to invoke .NET methods on the associated
    /// FluentCxPopupPlacement instance. It should be disposed of properly to avoid memory leaks.</remarks>
    private DotNetObjectReference<FluentCxPopupPlacement>? _dotNetRef;

    /// <summary>
    /// Represents a unique identifier for the popup element, used for DOM interactions and JavaScript interop.
    /// </summary>
    private readonly string _popupId;

    /// <summary>
    /// Represents the effective placement of the popup, which may differ from the initially specified placement due to dynamic adjustments based on available space and layout constraints.
    /// </summary>
    private PopupPlacement _effectivePlacement;

    /// <summary>
    /// Represents the file path to the JavaScript resource used for popup placement functionality.
    /// </summary>
    /// <remarks>This constant combines the root JavaScript directory with the specific file name for the
    /// FluentCx popup placement script. It is intended for internal use when referencing the required JavaScript file
    /// in component logic.</remarks>
    private const string JavascriptFilename = FluentCxConstants.JAVASCRIPT_ROOT + "PopupPlacement/FluentCxPopupPlacement.razor.js";

    /// <summary>
    /// Represents a value indicating whether the direction of the popup has changed.
    /// </summary>
    private bool _hasDirectionChanged;

    /// <summary>
    /// Represents a value indicating whether the anchor position has changed.
    /// </summary>
    private bool _hasAnchorPositionChanged;

    /// <summary>
    /// Represents a value indicating whether the placement of the popup has changed.
    /// </summary>
    private bool _hasPlacementChanged;

    /// <summary>
    /// Represents a value indicating whether the restricted area setting has changed.
    /// </summary>
    private bool _hasRestrictedAreaChanged;

    /// <summary>
    /// Represents a value indicating whether the radial mode setting has changed.
    /// </summary>
    private bool _hasRadialChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxPopupPlacement"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration used to initialize the component.</param>
    public FluentCxPopupPlacement(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
        _popupId = $"fluentcx-popup-{Id}";
    }

    /// <summary>
    /// Gets or sets the identifier of the anchor element to which the popup will be positioned relative to.
    /// </summary>
    [Parameter]
    public string AnchorId { get; set; } = default!;

    /// <summary>
    /// Gets or sets the preferred placement of the popup relative to its anchor element.
    /// </summary>
    [Parameter]
    public PopupPlacement Placement { get; set; } = PopupPlacement.Auto;

    /// <summary>
    /// Gets or sets a value indicating whether the popup is currently open and visible to the user.
    /// </summary>
    [Parameter]
    public bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets an event callback that is invoked when the open state of the popup changes,
    ///  allowing parent components to respond to visibility changes and update their state accordingly.
    /// </summary>
    [Parameter] public EventCallback<bool> IsOpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the popup.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the animation style to be applied when the popup is shown or hidden.
    /// </summary>
    [Parameter]
    public PopupAnimation Animation { get; set; } = PopupAnimation.FadeScale;

    /// <summary>
    /// Gets or sets a value indicating whether directional animation classes should be applied based on the popup's placement.
    /// </summary>
    [Parameter]
    public bool UseDirectionalAnimation { get; set; } = true;

    /// <summary>
    /// Gets or sets a custom CSS class to be applied for animation when the <see cref="Animation"/> property is set to <see cref="PopupAnimation.Custom"/>.
    /// </summary>
    [Parameter] public string? CustomAnimationClass { get; set; }

    /// <summary>
    /// Gets or sets an event callback that is invoked when the position of the popup is updated, providing the new placement result to allow parent components to react to changes in the popup's position and adjust their state or layout accordingly.
    /// </summary>
    [Parameter]
    public EventCallback<PopupPlacementResult> OnPositionUpdated { get; set; }

    /// <summary>
    /// Gets or sets the preferred direction for displaying the popup relative to its target element, allowing developers to indicate the desired placement of the popup
    ///  while still enabling dynamic adjustments based on available space and layout constraints.
    /// </summary>
    [Parameter]
    public PreferredPopupDirection PreferredDirection { get; set; } = PreferredPopupDirection.Default;

    /// <summary>
    /// Gets or sets the logical position of the anchor point on the target element to which the popup will be aligned.
    /// </summary>
    [Parameter]
    public AnchorLogicalPosition AnchorLogicalPosition { get; set; } = AnchorLogicalPosition.BottomRight;

    /// <summary>
    /// Gets or sets the width of the component, in pixels.
    /// </summary>
    [Parameter]
    public int Width  { get; set; }

    /// <summary>
    /// Gets or sets the height of the component, in pixels.
    /// </summary>
    [Parameter]
    public int Height { get; set; }

    /// <summary>
    /// Gets or sets the spacing between the anchor and the component.
    /// </summary>
    /// <remarks>The value should be a valid CSS gap expression, such as "8px", "1rem", or "10px 20px". This
    /// property allows customization of layout spacing for flexible UI arrangements.</remarks>
    [Parameter]
    public string? Gap { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the popup should be confined within the viewport or a specific container, preventing it from being positioned outside of visible boundaries.
    /// </summary>
    [Parameter]
    public bool RestrictedArea { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is displayed in a radial layout.
    /// </summary>
    [Parameter]
    public bool IsRadial { get; set; }

    /// <summary>
    /// Gets the CSS class that represents the direction of the animation based on the current placement settings.
    /// </summary>
    /// <remarks>Returns an empty string if directional animation is not used or if the direction cannot be
    /// determined. The returned class can be used to apply direction-specific styles or animations in the UI.</remarks>
    private string DirectionClass
    {
        get
        {
            if (!UseDirectionalAnimation)
            {
                return string.Empty;
            }

            var dir = GetDirectionFromPlacement(_effectivePlacement == PopupPlacement.Auto ? Placement : _effectivePlacement);

            return dir is null ? string.Empty : $"dir-{dir}";
        }
    }

    /// <summary>
    /// Determines the direction string corresponding to the specified popup placement.
    /// </summary>
    /// <param name="placement">The popup placement value for which to determine the direction.</param>
    /// <returns>A string representing the direction ("up", "down", "left", or "right") associated with the specified placement;
    /// or null if the placement does not correspond to a known direction.</returns>
    private static string? GetDirectionFromPlacement(PopupPlacement placement)
    {
        return placement switch
        {
            PopupPlacement.TopLeft or PopupPlacement.TopCenter or PopupPlacement.TopRight => "up",
            PopupPlacement.BottomLeft or PopupPlacement.BottomCenter or PopupPlacement.BottomRight => "down",
            PopupPlacement.LeftTop or PopupPlacement.LeftCenter or PopupPlacement.LeftBottom => "left",
            PopupPlacement.RightTop or PopupPlacement.RightCenter or PopupPlacement.RightBottom => "right",
            _ => null
        };
    }

    /// <summary>
    /// Gets the computed CSS class string for the popup element based on its current state and configuration.
    /// </summary>
    /// <remarks>The returned CSS class string reflects the popup's open state, animation type, and direction.
    /// This value is intended for internal use when rendering the popup's HTML markup.</remarks>
    private string? InternalCss =>
        DefaultClassBuilder
            .AddClass("fluentcx-popup")
            .AddClass("open", IsOpen)
            .AddClass(CustomAnimationClass, Animation == PopupAnimation.Custom)
            .AddClass($"anim-{Animation.ToString().ToLowerInvariant()}", Animation != PopupAnimation.Custom && Animation != PopupAnimation.None)
            .AddClass(DirectionClass, !string.IsNullOrEmpty(DirectionClass))
        .Build();

    /// <summary>
    /// Gets the computed inline CSS style string representing the element's position and size.
    /// </summary>
    /// <remarks>The returned style includes the 'left' and 'top' positions, and conditionally includes
    /// 'width' and 'height' if their values are greater than zero. The values are formatted using invariant culture to
    /// ensure consistent number formatting.</remarks>
    private string? InternalStyle => DefaultStyleBuilder
            .AddStyle("left", $"{_left.ToString(CultureInfo.InvariantCulture)}px")
            .AddStyle("top", $"{_top.ToString(CultureInfo.InvariantCulture)}px")
            .AddStyle("width", $"{Width.ToString(CultureInfo.InvariantCulture)}px", Width > 0)
            .AddStyle("height", $"{Height.ToString(CultureInfo.InvariantCulture)}px", Height > 0)
            .Build();

    /// <summary>
    /// Updates the position of the element by setting its left and top coordinates in pixels.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript to synchronize the element's position
    /// with client-side changes. Invoking this method triggers a re-render of the component to reflect the updated
    /// position.</remarks>
    /// <param name="result">The <see cref="PopupPlacementResult"/> containing the new position and placement information.</param>
    [JSInvokable]
    public async Task OnPositionChanged(PopupPlacementResult result)
    {
        _left = result.Position.X;
        _top = result.Position.Y;
        _effectivePlacement = result.Placement;

        if (OnPositionUpdated.HasDelegate)
        {
            await OnPositionUpdated.InvokeAsync(result);
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef ??= DotNetObjectReference.Create(this);

            _module = await JSModule.ImportJavaScriptModuleAsync(JavascriptFilename);

            await _module.InvokeVoidAsync(
                "FluentUI.Blazor.Community.PopupPlacement.Initialize",
                Id,
                AnchorId,
                _popupId,
                (int)Placement,
                _dotNetRef,
                Gap,
                IsRadial);

            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.PopupPlacement.UpdateDirection", Id, (int)PreferredDirection);
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.PopupPlacement.UpdateAnchorPosition", Id, (int)AnchorLogicalPosition);
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.PopupPlacement.UpdateRestrictedArea", Id, RestrictedArea);
            await InvokeAsync(StateHasChanged);
        }
        else
        {
            if (_module is not null)
            {
                var stateHasChanged = false;

                if (_hasDirectionChanged)
                {
                    stateHasChanged = true;
                    _hasDirectionChanged = false;
                    await _module.InvokeVoidAsync(
                        "FluentUI.Blazor.Community.PopupPlacement.UpdateDirection",
                        Id,
                        (int)PreferredDirection);
                }

                if (_hasAnchorPositionChanged)
                {
                    stateHasChanged = true;
                    _hasAnchorPositionChanged = false;
                    await _module.InvokeVoidAsync(
                        "FluentUI.Blazor.Community.PopupPlacement.UpdateAnchorPosition",
                        Id,
                        (int)AnchorLogicalPosition);
                }

                if (_hasPlacementChanged)
                {
                    stateHasChanged = true;
                    _hasPlacementChanged = false;
                    await _module.InvokeVoidAsync(
                        "FluentUI.Blazor.Community.PopupPlacement.UpdatePlacement",
                        Id,
                        (int)Placement);
                }

                if (_hasRestrictedAreaChanged)
                {
                    stateHasChanged = true;
                    _hasRestrictedAreaChanged = false;
                    await _module.InvokeVoidAsync(
                        "FluentUI.Blazor.Community.PopupPlacement.UpdateRestrictedArea",
                        Id,
                        RestrictedArea);
                }

                if (_hasRadialChanged)
                {
                    _hasRadialChanged = false;
                    await _module.InvokeVoidAsync(
                        "FluentUI.Blazor.Community.PopupPlacement.UpdateRadialMode",
                        Id,
                        IsRadial);
                }

                if (stateHasChanged)
                {
                    await InvokeAsync(StateHasChanged);
                }
            }
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasDirectionChanged = parameters.HasValueChanged(nameof(PreferredDirection), PreferredDirection);
        _hasAnchorPositionChanged = parameters.HasValueChanged(nameof(AnchorLogicalPosition), AnchorLogicalPosition);
        _hasPlacementChanged = parameters.HasValueChanged(nameof(Placement), Placement);
        _hasRestrictedAreaChanged = parameters.HasValueChanged(nameof(RestrictedArea), RestrictedArea);
        _hasRadialChanged = parameters.HasValueChanged(nameof(IsRadial), IsRadial);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.PopupPlacement.Dispose");
        }

        await base.DisposeAsync(jsModule);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        _dotNetRef?.Dispose();
        _dotNetRef = null;

        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Asynchronously updates the position of the popup element by invoking the corresponding JavaScript function.
    /// </summary>
    /// <remarks>This method should be called when the popup's position needs to be recalculated, such as
    /// after layout changes or user interactions. The update is performed only if the JavaScript module has been
    /// initialized.</remarks>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    internal async Task UpdatePositionAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.PopupPlacement.UpdatePosition", Id);
        }
    }

    /// <summary>
    /// Asynchronously updates the gap value for the popup placement configuration.
    /// </summary>
    /// <param name="gap">The new gap value to apply to the popup placement. This value determines the spacing between the popup and its
    /// target element.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    internal async Task UpdateGapAsync(string gap)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("FluentUI.Blazor.Community.PopupPlacement.UpdateGap", Id, gap);
        }
    }
}

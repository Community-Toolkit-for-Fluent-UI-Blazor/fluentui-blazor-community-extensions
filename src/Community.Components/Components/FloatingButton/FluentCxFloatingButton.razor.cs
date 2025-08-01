using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a floating button.
/// </summary>
public partial class FluentCxFloatingButton
    : FluentButton
{
    private delegate Task OnMouseEnterDelegate(MouseEventArgs e);

    /// <summary>
    /// Represents a value indicating if the floating button is fixed position.
    /// </summary>
    private bool _isFixed;

    /// <summary>
    /// Represents the javascript file to use.
    /// </summary>
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/FloatingButton/FluentCxFloatingButton.razor.js";

    /// <summary>
    /// Represents the loaded javascript module.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents indicating if the target has changed and we need to check if the new target is valid.
    /// </summary>
    private bool _updateTarget;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxFloatingButton"/> class.
    /// </summary>
    public FluentCxFloatingButton()
    {
        Id = Identifier.NewId();
        AdditionalAttributes = new Dictionary<string, object>()
        {
            ["onmouseenter"] = new OnMouseEnterDelegate(OnMouseEnterAsync),
            ["tabindex"] = -1
        };
    }

    /// <summary>
    /// Gets the style of the button.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("top", "16px", Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight))
        .AddStyle("bottom", "16px", Position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight))
        .AddStyle("left", "16px", Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.MiddleLeft, FloatingPosition.BottomLeft))
        .AddStyle("right", "16px", Position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.MiddleRight, FloatingPosition.BottomRight))
        .AddStyle("position", _isFixed ? "fixed" : "absolute")
        .AddStyle("z-index", "997")
        .AddStyle("width", "52px")
        .AddStyle("height", "52px")
        .AddStyle("left", "50%", Position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter))
        .AddStyle("top", "50%", Position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight))
        .AddStyle("transform", "translateX(-50%)", Position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.BottomCenter))
        .AddStyle("transform", "translateY(-50%)", Position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleRight))
        .AddStyle("transform", "translate(-50%, -50%)", Position == FloatingPosition.MiddleCenter)
        .AddStyle("border-radius", "9999px")
        .AddStyle("box-shadow", "0 14px 28.8px 0 rgba(0, 0, 0, .24), 0 0 8px 0 rgba(0, 0, 0, .2)")
        .Build();

    /// <summary>
    /// Gets or sets if the button is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets the position of the floating button.
    /// </summary>
    [Parameter]
    public FloatingPosition Position { get; set; } = FloatingPosition.BottomRight;

    /// <summary>
    /// Gets or sets the identifier of the relative container the current button belongs to.
    /// </summary>
    [Parameter]
    public string? RelativeContainerId { get; set; }

    /// <summary>
    /// Gets or sets the callback when the mouse enters the button.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnMouseEnter { get; set; }

    /// <summary>
    /// Gets or sets the callback when a key is pressed when the button is focused.
    /// </summary>
    [Parameter]
    public EventCallback<FluentKeyCodeEventArgs> OnKeyDown {  get; set; }

    /// <summary>
    /// Gets or sets the javascript runtime to use.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    private async Task OnMouseEnterAsync(MouseEventArgs e)
    {
        if (OnMouseEnter.HasDelegate)
        {
            await OnMouseEnter.InvokeAsync(e);
        }
    }

    /// <summary>
    /// Gets if the position of the button is fixed or not.
    /// </summary>
    /// <returns>Returns a task which set the position of the button when completed.</returns>
    private async Task GetIsFixedAsync()
    {
        if (_module is not null && !string.IsNullOrEmpty(RelativeContainerId))
        {
            var isValid = await _module.InvokeAsync<bool>("hasValidTarget", RelativeContainerId);
            _isFixed = !isValid;
        }
        else
        {
            _isFixed = true;
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _isFixed = string.IsNullOrEmpty(RelativeContainerId);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JavascriptFilename);
            await GetIsFixedAsync();
        }

        if (_updateTarget)
        {
            _updateTarget = false;
            await GetIsFixedAsync();
        }
    }

    /// <inheritdoc />
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.HasValueChanged(nameof(RelativeContainerId), RelativeContainerId))
        {
            _updateTarget = true;
        }
    }
}

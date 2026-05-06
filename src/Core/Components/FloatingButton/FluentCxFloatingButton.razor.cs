using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a floating button.
/// </summary>
public partial class FluentCxFloatingButton : FluentButton
{
    /// <summary>
    /// Represents a value indicating whether the position of the floating element has changed.
    /// </summary>
    private bool _hasFloatingPositionChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxFloatingButton"/> class.
    /// </summary>
    public FluentCxFloatingButton(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override string? StyleValue
    {
        get
        {
            var transform = Position switch
            {
                FloatingPosition.TopCenter or FloatingPosition.BottomCenter => "translateX(-50%)",
                FloatingPosition.MiddleLeft or FloatingPosition.MiddleRight => "translateY(-50%)",
                FloatingPosition.MiddleCenter => "translate(-50%, -50%)",
                _ => null
            };

            return DefaultStyleBuilder
            .AddStyle(base.StyleValue)
            .AddStyle("top", "16px", Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.TopCenter, FloatingPosition.TopRight))
            .AddStyle("bottom", "16px", Position.IsOneOf(FloatingPosition.BottomLeft, FloatingPosition.BottomCenter, FloatingPosition.BottomRight))
            .AddStyle("left", "16px", Position.IsOneOf(FloatingPosition.TopLeft, FloatingPosition.MiddleLeft, FloatingPosition.BottomLeft))
            .AddStyle("right", "16px", Position.IsOneOf(FloatingPosition.TopRight, FloatingPosition.MiddleRight, FloatingPosition.BottomRight))
            .AddStyle("position", IsFixed ? "fixed" : "absolute")
            .AddStyle("z-index", "997")
            .AddStyle("left", "50%", Position.IsOneOf(FloatingPosition.TopCenter, FloatingPosition.MiddleCenter, FloatingPosition.BottomCenter))
            .AddStyle("top", "50%", Position.IsOneOf(FloatingPosition.MiddleLeft, FloatingPosition.MiddleCenter, FloatingPosition.MiddleRight))
            .AddStyle("border-radius", "9999px")
            .AddStyle("box-shadow", "0 14px 28.8px 0 rgba(0, 0, 0, .24), 0 0 8px 0 rgba(0, 0, 0, .2)")
            .AddStyle("transform", transform, !string.IsNullOrEmpty(transform))
            .Build();
        }
    }

    /// <summary>
    /// Gets or sets if the button is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets if the button should be displayed at a fixed position instead of an absolute one.
    /// </summary>
    [Parameter]
    public bool IsFixed { get; set; }

    /// <summary>
    /// Gets or sets the position of the floating button.
    /// </summary>
    [Parameter]
    public FloatingPosition Position { get; set; } = FloatingPosition.BottomRight;

    /// <summary>
    /// Gets or sets the callback that is invoked when the position of the floating element changes.
    /// </summary>
    /// <remarks>Use this event callback to respond to changes in the floating element's position, such as
    /// updating related UI elements or triggering additional logic in the parent component. The callback receives a
    /// <see cref="FloatingPosition"/> value representing the new position.</remarks>
    [Parameter]
    public EventCallback<FloatingPosition> PositionChanged { get; set; }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasFloatingPositionChanged = parameters.HasValueChanged(nameof(Position), Position);

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (_hasFloatingPositionChanged)
        {
            _hasFloatingPositionChanged = false;
            await PositionChanged.InvokeAsync(Position);
        }
    }
}

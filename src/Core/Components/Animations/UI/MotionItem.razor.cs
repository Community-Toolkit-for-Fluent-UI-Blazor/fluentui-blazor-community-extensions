using System.Text;
using FluentUI.Blazor.Community.Components.Enums;
using FluentUI.Blazor.Community.Components.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a UI component that supports advanced motion, animation, and gesture behaviors within a Fluent UI Blazor
/// context.
/// </summary>
/// <remarks>Use this component to enable presence-based animations, gesture-driven transitions, and coordinated
/// motion scenarios in Blazor applications. The component can participate in motion groups for synchronized animations
/// and provides properties for customizing gestures, presence, and styles. Child content can be rendered within the
/// component, and motion actions are exposed for programmatic animation control.</remarks>
public partial class MotionItem
    : MotionNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MotionItem"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component, providing necessary settings and services for proper operation.</param>
    public MotionItem(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether the component should use style-only rendering for motion effects.
    /// </summary>
    [Parameter]
    public bool UseStyleOnlyRendering { get; set; } = true;

    /// <summary>
    /// Gets or sets the parent motion group for the current component.
    /// </summary>
    /// <remarks>This property is typically set via cascading parameters in Blazor to enable coordinated
    /// motion or animation behaviors within a group context. It allows the component to participate in group-based
    /// motion scenarios, such as synchronized transitions or shared animation state.</remarks>
    [CascadingParameter]
    private MotionGroup? Group { get; set; }

    /// <summary>
    /// Gets or sets the parent motion container that provides motion context for child components.
    /// </summary>
    /// <remarks>This property is typically set by the framework through cascading parameters and is used to
    /// coordinate motion or animation behaviors within a component hierarchy.</remarks>
    [CascadingParameter]
    private FluentCxMotion? MotionContainer { get; set; }

    /// <summary>
    /// Gets or sets a delegate that provides a custom CSS style string based on the specified motion state.
    /// </summary>
    /// <remarks>Use this property to dynamically apply styles to the component depending on its current
    /// motion state. The delegate should return a valid CSS style string or null if no custom style is
    /// required.</remarks>
    [Parameter]
    public Func<MotionState, string?>? CustomStyle { get; set; }

    /// <summary>
    /// Gets or sets the motion presence behavior for the component.
    /// </summary>
    /// <remarks>Use this property to specify how the component should handle presence-based animations or
    /// transitions. If not set, the default presence behavior is applied.</remarks>
    [Parameter]
    public MotionPresence? Presence { get; set; }

    /// <summary>
    /// Gets or sets the motion gestures configuration for the component.
    /// </summary>
    /// <remarks>Use this property to specify custom gesture behaviors, such as swipe or tap actions, that the
    /// component should recognize. If not set, default gesture handling is applied.</remarks>
    [Parameter]
    public MotionGestures? Gestures { get; set; }

    /// <summary>
    /// Gets the current state of the layout transition for the component.
    /// </summary>
    internal MotionTransitionState LayoutTransition { get; } = new();

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Group?.Register(this);
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        Group?.Unregister(this);
        GC.SuppressFinalize(this);

        return base.DisposeAsync();
    }

    /// <summary>
    /// Handles the hover-in gesture by applying the associated transition if available.
    /// </summary>
    /// <remarks>This method checks for the presence of both a hover gesture and a transition before
    /// attempting to apply the gesture. If either is not available, the method completes without performing any
    /// action.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task is completed immediately if no hover gesture or
    /// transition is defined.</returns>
    private Task HandleHoverIn()
        => Gestures?.Hover is null || Presence?.Transition is null
            ? Task.CompletedTask
            : Gestures.ApplyGestureAsync(MotionGestureName.Hover, this, Presence.Transition);

    /// <summary>
    /// Handles the logic to be executed when a hover-out event occurs, triggering the appropriate presence transition
    /// if available.
    /// </summary>
    /// <remarks>This method checks for the existence of presence transitions before attempting to apply them.
    /// If no transition is defined, the method completes without performing any action.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task is completed immediately if no presence transition
    /// is defined.</returns>
    private Task HandleHoverOut()
        => Presence?.Enter is null || Presence?.Transition is null
            ? Task.CompletedTask
            : Presence.ApplyEnterAsync(this);

    /// <summary>
    /// Handles the press-in gesture by applying the appropriate gesture asynchronously if gesture and transition
    /// handlers are available.
    /// </summary>
    /// <remarks>This method is typically used to trigger visual or behavioral feedback when a press-in
    /// gesture is detected. If gesture or transition handlers are not set, the method completes without performing any
    /// action.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task is completed immediately if no gesture or transition
    /// handler is present.</returns>
    private Task HandlePressIn()
        => Gestures?.Press is null || Presence?.Transition is null
            ? Task.CompletedTask
            : Gestures.ApplyGestureAsync(MotionGestureName.Press, this, Presence.Transition);

    /// <summary>
    /// Handles the press-out gesture by applying the 'tap' gesture asynchronously if gesture and transition information
    /// are available.
    /// </summary>
    /// <remarks>This method performs no action if either the gesture handler or the transition information is
    /// missing.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task is completed immediately if gesture or transition
    /// information is unavailable.</returns>
    private Task HandlePressOut()
        => Gestures?.Tap is null || Presence?.Transition is null
            ? Task.CompletedTask
            : Gestures.ApplyGestureAsync(MotionGestureName.Tap, this, Presence.Transition);

    /// <summary>
    /// Handles a tap gesture by applying the associated gesture asynchronously if available.
    /// </summary>
    /// <remarks>This method checks for the presence of both a tap gesture and a transition before attempting
    /// to apply the gesture. If either is not set, the method completes without performing any action.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task is completed immediately if no tap gesture or
    /// transition is defined.</returns>
    private Task HandleTap()
        => Gestures?.Tap is null || Presence?.Transition is null
            ? Task.CompletedTask
            : Gestures.ApplyGestureAsync(MotionGestureName.Tap, this, Presence.Transition);

    /// <summary>
    /// Initiates a drag gesture if gesture and transition handlers are available.
    /// </summary>
    /// <remarks>This method checks for the presence of both a drag gesture handler and a transition handler
    /// before attempting to apply the drag gesture. If either is missing, the method completes without performing any
    /// action.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task is completed immediately if no drag gesture or
    /// transition handler is present; otherwise, it completes when the drag gesture has been applied.</returns>
    private Task HandleDrag()
        => Gestures?.Drag is null || Presence?.Transition is null
            ? Task.CompletedTask
            : Gestures.ApplyGestureAsync(MotionGestureName.Drag, this, Presence.Transition);

    /// <inheritdoc />
    protected override string BuildStyle()
    {
        var s = State;
        var sb = new StringBuilder(base.BuildStyle());

        if (CustomStyle is not null)
        {
            var extra = CustomStyle(s);

            if (!string.IsNullOrWhiteSpace(extra))
            {
                sb.Append(extra);
            }
        }

        return sb.ToString();
    }

    /// <inheritdoc />
    protected override bool ShouldRender()
    {
        if (!UseStyleOnlyRendering)
        {
            return base.ShouldRender();
        }

        return Timeline.State != MotionTimelineState.Running;
    }

    /// <summary>
    /// Applies the current style to the component asynchronously, updating its visual appearance as needed.
    /// </summary>
    /// <remarks>If style-only rendering is not enabled, this method triggers a component re-render.
    /// Otherwise, it applies the computed style using JavaScript interop.</remarks>
    /// <returns>A ValueTask that represents the asynchronous operation.</returns>
    internal async Task ApplyStyleAsync()
    {
        if (!UseStyleOnlyRendering || MotionContainer is null)
        {
            await InvokeAsync(StateHasChanged);
            return;
        }

        var style = BuildStyle();
        await MotionContainer.SetStyleAsync(Id, style);
    }

    /// <summary>
    /// Resets the transition state to its initial configuration.
    /// </summary>
    internal void ResetTransition()
    {
        LayoutTransition.Reset();
        State.Reset();
    }
}

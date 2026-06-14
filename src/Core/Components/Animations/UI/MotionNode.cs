using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a motion node that can be animated using the MotionTimeline and MotionState.
/// </summary>
public abstract class MotionNode : FluentComponentBase
{
    /// <summary>
    /// Represents the render fragment used to create a clone of the component instance.
    /// </summary>
    private readonly RenderFragment _cloneInstance;

    /// <summary>
    /// Represents the invariant culture, which is culture-insensitive and associated with the English language but not
    /// with any country or region.
    /// </summary>
    /// <remarks>The invariant culture is used in operations that require culture-independent results, such as
    /// formatting and parsing operations that must yield consistent results regardless of the user's locale.</remarks>
    private static readonly CultureInfo s_invariantCulture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Represents a StringBuilder instance used for constructing the CSS style based on the current motion state.
    /// </summary>
    private readonly StringBuilder _styleBuilder = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MotionNode"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this motion node.</param>
    public MotionNode(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
        Actions = new MotionActions(this);
        _cloneInstance = builder =>
        {
            builder.OpenComponent(0, GetType());
            builder.AddAttribute(1, "ChildContent", ChildContent);
            builder.CloseComponent();
        };
    }

    /// <summary>
    /// Gets the current motion state for the component.
    /// </summary>
    public MotionState State { get; } = new();

    /// <summary>
    /// Gets the timeline that defines the sequence and timing of motion animations.
    /// </summary>
    public MotionTimeline Timeline { get; } = new();

    /// <summary>
    /// Gets the set of motion actions available for animating UI elements.
    /// </summary>
    /// <remarks>Use this property to access predefined animation actions that can be applied to components
    /// for visual transitions or effects. The available actions depend on the implementation of the IMotionActions
    /// interface.</remarks>
    public IMotionActions Actions { get; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    /// <remarks>Use this property to specify the child elements or markup that will be displayed within the
    /// component. Typically set in Razor markup using child content syntax.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the rendered content for this component instance.
    /// </summary>
    public RenderFragment RenderFragment => _cloneInstance;

    /// <summary>
    /// Advances the timeline and applies the current state based on the specified time interval.
    /// </summary>
    /// <param name="delta">The amount of time to advance the timeline. Represents the elapsed time since the last update.</param>
    internal virtual void OnTick(TimeSpan delta)
    {
        Timeline.Update(delta);
    }

    /// <summary>
    /// Builds a CSS style string representing the current transformation and opacity state, optionally including custom
    /// user-defined styles.
    /// </summary>
    /// <remarks>The returned style string combines translation, scaling, rotation, and opacity based on the
    /// current state. If a custom style delegate is provided, its output is appended to the style string. This method
    /// is intended for use in scenarios where dynamic styling is required, such as rendering components with
    /// interactive transformations.</remarks>
    /// <returns>A string containing the CSS style for transform and opacity, with any additional custom styles appended.</returns>
    protected virtual string BuildStyle()
    {
        var x = State.X;
        var y = State.Y;
        var sx = State.ScaleX;
        var sy = State.ScaleY;
        var r = State.Rotation;
        var o = State.Opacity;

        _styleBuilder.Clear();
        _styleBuilder.Append("transform: translate(")
                     .Append(s_invariantCulture, $"{x}")
                     .Append("px, ")
                     .Append(s_invariantCulture, $"{y}")
                     .Append("px) scale(")
                     .Append(s_invariantCulture, $"{sx}")
                     .Append(',')
                     .Append(' ')
                     .Append(s_invariantCulture, $"{sy}")
                     .Append(") rotate(")
                     .Append(s_invariantCulture, $"{r}")
                     .Append("deg); opacity: ")
                     .Append(s_invariantCulture, $"{o}")
                     .Append(';');

        return _styleBuilder.ToString();
    }
}

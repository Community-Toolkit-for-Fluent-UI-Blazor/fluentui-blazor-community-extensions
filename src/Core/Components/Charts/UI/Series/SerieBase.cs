using FluentUI.Blazor.Community.Components.Charts.Options;
using FluentUI.Blazor.Community.Components.Charts.Series;
using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a chart series that displays data as vertical columns within the Fluent UI Blazor charting component.
/// </summary>
public abstract class SerieBase : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SerieBase"/> class with the specified library configuration.
    /// </summary>
    /// <param name="libraryConfiguration">The configuration settings for the Fluent UI Blazor library.</param>
    public SerieBase(LibraryConfiguration libraryConfiguration)
        : base(libraryConfiguration)
    { }

    /// <summary>
    /// Gets the type of chart represented by the current instance.
    /// </summary>
    protected internal abstract ChartType ChartType { get; }

    /// <summary>
    /// Gets or sets the effect to apply when animating the chart series.
    /// </summary>
    [Parameter]
    public ChartAnimationEffect Effect { get; set; } = ChartAnimationEffect.Fade;

    /// <summary>
    /// Gets or sets a value indicating whether animations are enabled for the component.
    /// </summary>
    [Parameter]
    public bool AnimationEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the duration of the animation or transition effect.
    /// </summary>
    [Parameter]
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(0.5);

    /// <summary>
    /// Gets or sets the delay interval before the associated action is executed.
    /// </summary>
    /// <remarks>Set this property to specify how long to wait before triggering the action. The default value
    /// is zero, which means the action occurs immediately.</remarks>
    [Parameter]
    public TimeSpan Delay { get; set; } = TimeSpan.Zero;

    /// <summary>
    /// Gets or sets the easing function used to interpolate values during SVG animations.
    /// </summary>
    /// <remarks>The easing function determines the rate of change of the animation over time, affecting how
    /// the animation accelerates or decelerates. Use this property to customize the animation's pacing for a smoother
    /// or more dynamic effect.</remarks>
    [Parameter]
    public SvgEasing Easing { get; set; } = SvgEasing.EaseInOut;

    /// <summary>
    /// Optional custom cubic-bezier curve (x1,y1,x2,y2).
    /// Used only when Easing = CustomCubicBezier.
    /// </summary>
    [Parameter]
    public (double x1, double y1, double x2, double y2)? CustomBezier { get; init; }

    /// <summary>
    /// Optional steps configuration.
    /// Used only when Easing = Steps.
    /// </summary>
    [Parameter]
    public (int count, bool start)? Steps { get; init; }

    /// <summary>
    /// Gets the name associated with the current instance.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets an arbitrary object value that can be used to associate custom data with this instance.
    /// </summary>
    [Parameter]
    public object? Tag { get; set; }

    /// <summary>
    /// Gets a value indicating whether the component is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the component is enabled and can respond to user interaction.
    /// </summary>
    [Parameter]
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets the style settings applied to the chart series.
    /// </summary>
    /// <remarks>Use this property to customize the appearance of the chart series, such as colors, line
    /// styles, or markers. If not set, the default style for the chart series is used.</remarks>
    [Parameter]
    public ChartItemStyle? ItemStyle { get; set; }

    /// <summary>
    /// Gets the interaction behavior for the chart series, such as how user input is handled.
    /// </summary>
    /// <remarks>Set this property to specify how the chart series responds to user interactions, such as
    /// selection, highlighting, or tooltips. If not set, the default interaction behavior is used.</remarks>
    [Parameter]
    public ChartSerieInteraction? Interaction { get; set; }

    /// <summary>
    /// Gets or sets the parent chart component that this series belongs to.
    /// </summary>
    [CascadingParameter]
    internal FluentCxChart? Parent { get; set; }

    /// <summary>
    /// Creates a new instance of a chart series configured according to the current settings.
    /// </summary>
    /// <remarks>Derived classes should implement this method to return a specific type of chart series based
    /// on their configuration.</remarks>
    /// <returns>A new <see cref="ChartSerie"/> instance representing the configured chart series.</returns>
    protected internal abstract ChartSerie Create();

    /// <summary>
    /// Gets a value indicating whether the current series has a parent chart component.
    /// </summary>
    protected internal virtual bool HasParent => false;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"The {GetType().Name} component must be used within a {nameof(FluentCxChart)} component.");
        }

        Parent.AddSerie(this);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.RemoveSerie(this);
        DisposeOverride();

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of resources used by the component.
    /// </summary>
    protected virtual void DisposeOverride()
    {
    }

    /// <summary>
    /// Refreshes the parent chart component to reflect changes made to this series.
    /// </summary>
    protected void Refresh()
    {
        Parent?.Refresh();
    }

    /// <summary>
    /// Retrieves the animation options to be used for chart rendering based on the current configuration.
    /// </summary>
    /// <remarks>If no custom animation options are provided, a default set of options is returned. Animation
    /// is only applied when <see cref="AnimationEnabled"/> is <see langword="true"/>.</remarks>
    /// <returns>A <see cref="ChartAnimationOptions"/> instance containing the animation settings if animation is enabled;
    /// otherwise, <see langword="null"/>.</returns>
    protected ChartAnimationOptions? GetAnimationOptions()
    {
        if (!AnimationEnabled)
        {
            return null;
        }

        return new ChartAnimationOptions
        {
            Effect = Effect,
            Duration = Duration,
            Delay = Delay,
            Easing = Easing,
            Steps = Steps,
            CustomBezier = CustomBezier
        };
    }
}

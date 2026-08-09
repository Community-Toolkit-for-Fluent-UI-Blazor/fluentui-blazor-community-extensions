using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a layout that interpolates between multiple motion layouts, enabling smooth transitions between different
/// layout states.
/// </summary>
/// <remarks>MorphingLayout is useful for creating animated transitions between distinct layout configurations. It
/// blends the properties of two adjacent layouts based on the Morph value, allowing for fluid morphing effects in UI
/// components. The Index property determines the starting layout, and Morph controls the interpolation factor between
/// the current and next layout. At least two layouts must be provided in the Layouts array for morphing to
/// occur.</remarks>
public sealed partial class MorphingLayout : MotionLayoutBase
{
    /// <summary>
    /// Represents the collection of motion layout instances managed by this class.
    /// </summary>
    private readonly List<MotionLayoutBase> _layouts = [];

    /// <summary>
    /// Represents the current progres of the morphing transition.
    /// </summary>
    private double _morph;

    /// <summary>
    /// Gets or sets the index of the current layout in the collection.
    /// </summary>
    private int _index;

    /// <summary>
    /// Represents the motion curve used for interpolating between layouts during the morphing transition.
    /// </summary>
    private MotionCurve? _curve;

    /// <summary>
    /// Represents the elapsed time since the start of the current morphing transition.
    /// </summary>
    private TimeSpan _elapsed;

    /// <summary>
    /// Initializes a new instance of the MorphingLayout class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings to use for initializing the layout. Cannot be null.</param>
    public MorphingLayout(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the layout content to render within the component.
    /// </summary>
    /// <remarks>Use this parameter to provide custom layout markup or components that will be displayed as
    /// part of the component's content. The value should be a RenderFragment representing the desired layout
    /// structure.</remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the content should loop automatically.
    /// </summary>
    [Parameter]
    public bool Loop { get; set; } = true;

    /// <summary>
    /// Gets or sets the duration for which the associated action or effect is applied.
    /// </summary>
    [Parameter]
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets the easing function to be used for the morphing transition.
    /// </summary>
    [Parameter]
    public EasingFunction Easing { get; set; } = EasingFunction.Linear;

    /// <summary>
    /// Gets or sets the easing mode used to control the interpolation behavior of the morphing transition.
    /// </summary>
    [Parameter]
    public EasingMode Mode { get; set; } = EasingMode.InOut;

    /// <inheritdoc />
    public override void ComputeLayout(IReadOnlyList<MotionItem> items)
    {
        var layoutCount = _layouts.Count;

        if (layoutCount < 2)
        {
            return;
        }

        var from = _layouts[_index];
        var to = _layouts[(_index + 1) % layoutCount];

        from.SetDimensions(Width, Height);
        to.SetDimensions(Width, Height);

        from.ComputeLayout(items);
        var fromTargets = items.Select(i => i.LayoutTransition.Target.Clone()).ToList();

        to.ComputeLayout(items);
        var toTargets = items.Select(i => i.LayoutTransition.Target.Clone()).ToList();

        for (var i = 0; i < items.Count; i++)
        {
            var target = items[i].LayoutTransition.Target;

            foreach (var key in toTargets[i].Keys)
            {
                var a = fromTargets[i].GetOrDefault<double>(key);
                var b = toTargets[i].GetOrDefault<double>(key);

                var v = a + (b - a) * _morph;
                target.Set(key, v);
            }
        }
    }

    /// <summary>
    /// Adds a layout to the collection of managed layouts.
    /// </summary>
    /// <param name="layout">The layout instance to add. Cannot be null.</param>
    internal void AddLayout(MotionLayoutBase layout)
    {
        _layouts.Add(layout);
    }

    /// <summary>
    /// Removes the specified layout from the collection of managed layouts.
    /// </summary>
    /// <param name="layout">The layout instance to remove from the collection. Cannot be null.</param>
    internal void RemoveLayout(MotionLayoutBase layout)
    {
        _layouts.Remove(layout);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delta"></param>
    internal void OnTick(TimeSpan delta)
    {
        var layoutCount = _layouts.Count;

        if (layoutCount < 2)
        {
            return;
        }

        var segmentCount = Loop ? layoutCount : layoutCount - 1;

        if (segmentCount <= 0)
        {
            return;
        }

        var segmentDuration = TimeSpan.FromTicks(Duration.Ticks / segmentCount);
        _curve = new MotionCurve(segmentDuration, Easing, Mode);
        _elapsed += delta;

        _morph = _curve.Evaluate(_elapsed);

        if (_elapsed >= segmentDuration)
        {
            _elapsed = TimeSpan.Zero;
            _index++;

            if (_index >= layoutCount - 1)
            {
                if (Loop)
                {
                    _index = 0;
                }
                else
                {
                    _index = layoutCount - 2;
                }
            }
        }
    }
}

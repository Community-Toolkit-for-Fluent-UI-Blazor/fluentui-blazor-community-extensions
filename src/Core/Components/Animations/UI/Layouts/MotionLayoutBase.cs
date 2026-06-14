using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a base class for components that define custom layout logic for motion items within a MotionGroup.
/// </summary>
/// <remarks>Inherit from this class to implement custom layout strategies for animating or arranging motion
/// items. This class must be used within a MotionGroup component, which manages the lifecycle and context for the
/// layout. Override the ComputeLayout method to specify how items should be positioned or animated.</remarks>
public abstract class MotionLayoutBase
    : FluentComponentBase, IMotionLayout
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MotionLayoutBase"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component. </param>
    public MotionLayoutBase(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the motion group context for the component.
    /// </summary>
    /// <remarks>This property is typically provided by a parent component using the cascading parameter
    /// mechanism. It allows child components to access shared motion group settings or behaviors.</remarks>
    [CascadingParameter]
    private MotionGroup? Group { get; set; }

    /// <summary>
    /// Gets or sets the parent MorphingLayout component from the cascading context.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// nested within a MorphingLayout. It enables child components to interact with or obtain information from the
    /// parent layout.</remarks>
    [CascadingParameter]
    private MorphingLayout? MorphingLayout { get; set; }

    /// <summary>
    /// Gets or sets the width value.
    /// </summary>
    internal double Width { get; set; }

    /// <summary>
    /// Gets or sets the height value.
    /// </summary>
    internal double Height { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (MorphingLayout is not null)
        {
            MorphingLayout.AddLayout(this);
        }
        else if (Group is not null)
        {
            Group?.SetLayout(this);
        }
    }

    /// <summary>
    /// Calculates and applies the layout for the specified collection of motion items.
    /// </summary>
    /// <remarks>Implementations should define how the layout is computed and applied to the provided items.
    /// The order and arrangement of items may vary depending on the specific layout strategy.</remarks>
    /// <param name="items">A read-only list of <see cref="MotionItem"/> objects to be arranged by the layout algorithm. Cannot be null.</param>
    public abstract void ComputeLayout(IReadOnlyList<MotionItem> items);

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        MorphingLayout?.RemoveLayout(this);

        GC.SuppressFinalize(this);

        return base.DisposeAsync();
    }

    /// <summary>
    /// Sets the width and height dimensions for the current instance.
    /// </summary>
    /// <param name="width">The new width value to assign. Must be a finite number.</param>
    /// <param name="height">The new height value to assign. Must be a finite number.</param>
    public void SetDimensions(double width, double height)
    {
        Width = width;
        Height = height;
    }
}

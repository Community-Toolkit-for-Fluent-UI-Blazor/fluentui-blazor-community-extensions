using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a single item in the SleekDial component.
/// This component is used to define the content and behavior of each item in the dial.
/// </summary>
/// <param name="configuration">The library configuration for the component.</param>
public partial class SleekDialItemView(LibraryConfiguration configuration)
    : FluentComponentBase(configuration)
{
    /// <summary>
    /// Represents a reference to the root HTML element of the component.
    /// </summary>
    private ElementReference _root;

    /// <summary>
    /// Gets or sets a value indicating whether the component currently has focus.
    /// </summary>
    /// <remarks>This property is typically used to manage focus states in user interface components, allowing
    /// for visual feedback or behavior changes based on focus.</remarks>
    [Parameter]
    public bool IsFocused { get; set; }

    /// <summary>
    /// Gets or sets the parent FluentCxSleekDial component associated with this instance.
    /// </summary>
    /// <remarks>This property is used to establish a cascading relationship between components, allowing
    /// child components to access parameters from their parent components.</remarks>
    [CascadingParameter]
    private FluentCxSleekDial? Parent { get; set; }

    /// <summary>
    /// Gets or sets the item associated with the sleek dial.
    /// </summary>
    /// <remarks>This property can be null, indicating that no item is currently set. Ensure to check for null
    /// before accessing properties of the item.</remarks>
    [Parameter]
    public SleekDialItem? Item { get; set; }

    /// <summary>
    /// Gets or sets the layout configuration for the sleek dial item.
    /// </summary>
    /// <remarks>The layout determines how the sleek dial item is visually presented. It can be set to
    /// different values to achieve various display styles.</remarks>
    [Parameter]
    public SleekDialItemLayout Layout { get; set; } = new();

    /// <summary>
    /// Gets or sets the zero-based index of the current item within the collection.
    /// </summary>
    /// <remarks>Ensure that the value assigned to this property is within the valid range of the collection
    /// to avoid runtime errors.</remarks>
    [Parameter]
    public int Index { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is currently active.
    /// </summary>
    [Parameter]
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets the CSS class string generated for the layout using the default class builder.
    /// </summary>
    /// <remarks>The resulting CSS class reflects the current layout's properties. If the layout is null, the
    /// returned class may also be null.</remarks>
    private string? InternalClass =>
            DefaultClassBuilder
            .AddClass("sleekdial-item")
            .AddClass(Layout?.CssClass)
            .Build();

    /// <summary>
    /// Gets the computed CSS style string for the layout, including position, transform, and opacity based on the
    /// current state.
    /// </summary>
    /// <remarks>If the layout is null, an empty string is returned. The style is dynamically generated from
    /// the layout's properties and the active state.</remarks>
    private string? InternalStyle
    {
        get
        {
            if (Layout is null)
            {
                return "";
            }

            var x = Layout.X;
            var y = Layout.Y;
            var transform = IsActive ? Layout.FinalTransform : Layout.Transform;
            var opacity = IsActive ? Layout.FinalOpacity : Layout.InitialOpacity;

            return DefaultStyleBuilder
                .AddStyle("left", $"{x}px")
                .AddStyle("top", $"{y}px")
                .AddStyle("transform", transform)
                .AddStyle("opacity", opacity.ToString(System.Globalization.CultureInfo.InvariantCulture))
                .AddStyle("transition", Layout.Transition)
                .Build();
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (IsFocused)
        {
            await _root.FocusAsync();
        }
    }

    /// <summary>
    /// Handles the click event asynchronously for the associated item, updating the parent focus and invoking the
    /// item's click handler if available.
    /// </summary>
    /// <remarks>If the item is not null, this method sets the parent's focused index to the current index and
    /// invokes the item's asynchronous click handler. This method should be awaited to ensure the click logic completes
    /// before proceeding.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnClickAsync()
    {
        if (Item is not null)
        {
            Parent!.FocusedIndex = Index;
            await Item.OnClickAsync();
        }
    }
}

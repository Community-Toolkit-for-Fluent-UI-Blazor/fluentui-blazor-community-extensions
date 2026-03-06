using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the menu button in a menu trail. This button is used to indicate that there are more items in the menu trail than can be displayed at once, and it typically opens a submenu when clicked.
/// </summary>
public partial class TrailMenuButton
    : FluentComponentBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="TrailMenuButton"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration object used to apply default values to the component.</param>
    public TrailMenuButton(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent of the button.
    /// </summary>
    [CascadingParameter]
    private FluentCxTrailMenu? Parent { get; set; }

    /// <summary>
    /// Gets or sets the icon associated with the component. This icon can be used to visually represent the component
    /// in the user interface.
    /// </summary>
    /// <remarks>If set to null, no icon will be displayed. The icon should be of type Icon and can be
    /// customized based on the application's requirements.</remarks>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether only the icon is displayed without accompanying text.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the control displays only the icon, which can help
    /// conserve space in the user interface. When set to <see langword="false"/>, both the icon and its associated text
    /// are shown.</remarks>
    [Parameter]
    public bool IconOnly { get; set; }

    /// <summary>
    /// Gets or sets the label text associated with the component.
    /// </summary>
    /// <remarks>Set this property to provide a descriptive name or title that is displayed alongside the
    /// component. The value can be null if no label is required.</remarks>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the collection of menu items to display in the trail menu.
    /// </summary>
    /// <remarks>The collection must not be null and should contain instances implementing the ITrailMenuItem
    /// interface. This property enables dynamic configuration of the menu items presented to the user.</remarks>
    [Parameter]
    public IEnumerable<ITrailMenuItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the callback that is invoked when the button is clicked.
    /// </summary>
    /// <remarks>Use this property to specify a method or lambda expression to handle click events for the
    /// button. The assigned callback is triggered when the user interacts with the button, allowing the parent
    /// component to respond to the click action. This property is typically used to implement custom logic or
    /// navigation in response to user input.</remarks>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("TrailOverflowButton must be used within a FluentCxTrailMenu.");
        }
    }
}

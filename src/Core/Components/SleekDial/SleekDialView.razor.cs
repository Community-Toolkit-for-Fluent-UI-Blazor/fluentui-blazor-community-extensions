using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a view component for displaying a sleek dial interface within a Fluent design system.
/// </summary>
/// <remarks>This component must be used within a parent FluentCxSleekDial component. If used outside of this
/// context, an InvalidOperationException will be thrown during initialization.</remarks>
public partial class SleekDialView
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the SleekDialView class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that define the behavior and properties of the SleekDialView instance. Cannot be
    /// null.</param>
    public SleekDialView(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the parent FluentCxSleekDial component associated with this component.
    /// </summary>
    /// <remarks>This property is set via cascading parameters, allowing it to receive its value from an
    /// ancestor FluentCxSleekDial component in the component hierarchy. Ensure that the parent component is properly
    /// initialized before accessing this property.</remarks>
    [CascadingParameter]
    internal FluentCxSleekDial Parent { get; set; } = default!;

    /// <summary>
    /// Gets or sets the collection of layouts that define the visual arrangement of dial items.
    /// </summary>
    /// <remarks>Each layout in the collection specifies how an individual dial item is presented. Modifying
    /// this collection allows dynamic customization of the dial's appearance. If set to null, the default layout will
    /// be used.</remarks>
    [Parameter]
    public List<SleekDialItemLayout> Layouts { get; set; } = [];

    /// <summary>
    /// Updates the component's state and triggers a re-render.
    /// </summary>
    /// <remarks>This method is typically called when the component's state changes and needs to be reflected
    /// in the UI. It ensures that any changes are applied and the component is displayed with the latest
    /// data.</remarks>
    internal void Refresh()
    {
        StateHasChanged();
    }

    /// <summary>
    /// Initializes the component and establishes its relationship with the parent FluentCxSleekDial component.
    /// </summary>
    /// <remarks>This method overrides the base initialization to ensure the component is properly linked to
    /// its parent. It is essential to use this component within a FluentCxSleekDial to maintain correct
    /// behavior.</remarks>
    /// <exception cref="InvalidOperationException">Thrown if the component is not contained within a FluentCxSleekDial parent component.</exception>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{GetType()} must be used inside {nameof(FluentCxSleekDial)}.");
        }
    }

    /// <summary>
    /// Handles key down events asynchronously, processing specific key actions such as the Escape key and navigation
    /// keys.
    /// </summary>
    /// <remarks>If the Escape key is pressed, the method will hide the parent popup if it is currently
    /// displayed. Additionally, if the parent is open, it will handle navigation key events accordingly.</remarks>
    /// <param name="e">The event arguments containing information about the key that was pressed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task OnKeyDownHandlerAsync(FluentKeyCodeEventArgs e)
    {
        if (e.Key == KeyCode.Escape && Parent is not null)
        {
            await Parent.ShowOrHidePopupAsync(false);
        }

        if (Parent is not null && Parent.IsOpen)
        {
            await Parent.HandleNavigationKeyAsync(e);
        }
    }
}

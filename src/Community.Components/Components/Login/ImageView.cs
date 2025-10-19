using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays an image within a login view.
/// </summary>
/// <remarks>Use this component to render an image as part of a login interface. The image source and the specific
/// login view can be configured through parameters. This component is typically used as a child of a <see
/// cref="FluentCxLogin"/> component.</remarks>
public class ImageView : ComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentCxLogin"/> parent. It enables child components to access shared state or
    /// functionality provided by the parent component.</remarks>
    [CascadingParameter]
    public FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets or sets the view mode for the account manager component.
    /// </summary>
    /// <remarks>Use this property to specify which view should be displayed in the account manager. Changing
    /// the value updates the UI to reflect the selected view.</remarks>
    [Parameter]
    public AccountManagerView View { get; set; }

    /// <summary>
    /// Gets or sets the source identifier or URI for the component's data or content.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("ImageView must be used inside a FluentCxLogin component.");
        }

        Parent.Add(this);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Parent?.Remove(this);

        GC.SuppressFinalize(this);
    }
}

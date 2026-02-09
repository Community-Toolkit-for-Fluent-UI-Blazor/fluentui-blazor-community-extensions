using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a video player component.
/// </summary>
public partial class Video
{
    /// <summary>
    /// Initializes a new instance of the Video class with a unique identifier.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by the component.</param>
    public Video(LibraryConfiguration configuration) : base(configuration)
    {
        Id = $"video-{Identifier.NewId()}";
    }

    /// <summary>
    /// Gets or sets a value indicating whether the component is visible.
    /// </summary>
    [Parameter]
    public bool IsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the component should use a compact layout.
    /// </summary>
    [Parameter]
    public bool IsCompact { get; set; }

    /// <summary>
    /// Gets the reference to the associated element in the DOM.
    /// </summary>
    /// <remarks>This property provides access to the underlying HTML element, allowing for manipulation or
    /// retrieval of its properties. It is important to note that the element is only accessible after it has been
    /// rendered in the DOM.</remarks>
    public ElementReference Element { get; private set; }
}

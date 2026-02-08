using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a responsive image component.
/// </summary>
public class ResponsiveImage
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the ResponsiveImage class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings for the library that determine how the responsive image is processed and rendered.</param>
    public ResponsiveImage(LibraryConfiguration configuration)
        : base(configuration)
    {
    }

    /// <summary>
    /// Gets or sets the parent FluentCxPicture instance associated with this component.
    /// </summary>
    /// <remarks>This property is used to establish a relationship between the current component and its
    /// parent FluentCxPicture. It is important for managing the component's state and behavior within the parent
    /// context.</remarks>
    [CascadingParameter]
    private FluentCxPicture? Parent { get; set; }

    /// <summary>
    /// Gets or sets the media type associated with the content.
    /// </summary>
    /// <remarks>This property can be used to specify the type of media, such as 'image/jpeg' or
    /// 'application/json'. If set to null, the media type is considered unspecified.</remarks>
    [Parameter]
    public string? Media { get; set; }

    /// <summary>
    /// Gets or sets the source of the data being processed.
    /// </summary>
    /// <remarks>This property can be set to a string representing the origin of the data. It may be null if
    /// no source is specified.</remarks>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the media type of the content being processed.
    /// </summary>
    /// <remarks>Specify the content type to inform consumers or downstream systems how the data should be
    /// interpreted. Common values include "image/jpeg", "image/png", or other MIME types. If not set, the default
    /// handling may apply based on context.</remarks>
    [Parameter]
    public string? ContentType { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.AddSource(this);
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        Parent?.RemoveSource(this);

        return base.DisposeAsync();
    }
}

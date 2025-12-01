using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a responsive image component.
/// </summary>
public class ResponsiveImage
    : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxPicture"/> component in the cascading parameter hierarchy.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentCxPicture"/> parent. Accessing this property allows child components to interact
    /// with or retrieve information from their parent picture component.</remarks>
    [CascadingParameter]
    private FluentCxPicture? Parent { get; set; }

    /// <summary>
    /// Gets or sets the media type or query used to determine when the component should be rendered.
    /// </summary>
    /// <remarks>Specify a media type (such as "screen" or "print") or a media query (such as "(max-width:
    /// 600px)") to control the rendering behavior based on the target device or display conditions.</remarks>
    [Parameter]
    public string? Media { get; set; }

    /// <summary>
    /// Gets or sets the source URI or path for the content to be rendered.
    /// </summary>
    /// <remarks>Specify a relative or absolute URI, or a local file path, depending on the component's
    /// requirements. If the value is null or empty, no content will be loaded.</remarks>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the content to be rendered.
    /// </summary>
    [Parameter]
    public string? ContentType { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Parent?.AddSource(this);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Parent?.RemoveSource(this);

        GC.SuppressFinalize(this);
    }
}

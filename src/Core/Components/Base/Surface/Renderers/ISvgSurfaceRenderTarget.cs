using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a render target that outputs graphics in Scalable Vector Graphics (SVG) format.
/// </summary>
/// <remarks>Implement this interface to provide SVG-based rendering capabilities for surfaces. This interface
/// extends ISurfaceRenderTarget to support scenarios where vector-based output is required, such as exporting graphics
/// for web or print use.</remarks>
public interface ISvgSurfaceRenderTarget : ISurfaceRenderTarget
{
    /// <summary>
    /// Gets the SVG markup content represented as a MarkupString.
    /// </summary>
    /// <remarks>Use this property to retrieve the raw SVG markup for rendering in Blazor components. The
    /// returned MarkupString should be rendered directly to preserve the SVG formatting and avoid HTML
    /// encoding.</remarks>
    MarkupString Svg { get; }
}

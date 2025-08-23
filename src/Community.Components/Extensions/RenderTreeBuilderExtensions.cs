using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace FluentUI.Blazor.Community.Extensions;

/// <summary>
/// Represents the extensions for the <see cref="RenderTreeBuilder"/> class.
/// </summary>
internal static class RenderTreeBuilderExtensions
{
    /// <summary>
    /// Adds a child content into the <paramref name="renderTreeBuilder"/> by passing a <see cref="RenderFragment"/> delegate.
    /// </summary>
    /// <param name="renderTreeBuilder">RenderTreeBuilder to use.</param>
    /// <param name="sequence">Number of the sequence.</param>
    /// <param name="renderFragment">RenderFragment which represents the child content.</param>
    public static void AddChildContent(this RenderTreeBuilder renderTreeBuilder,
                                       int sequence,
                                       RenderFragment? renderFragment)
    {
        renderTreeBuilder.AddComponentParameter(sequence, "ChildContent", renderFragment);
    }
}

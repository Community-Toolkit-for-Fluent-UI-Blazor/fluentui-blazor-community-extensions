using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace FluentUI.Blazor.Community.Extensions;

internal static class RenderTreeBuilderExtensions
{
    public static void AddChildContent(this RenderTreeBuilder renderTreeBuilder,
                                       int sequence,
                                       RenderFragment renderFragment)
    {
        renderTreeBuilder.AddComponentParameter(sequence, "ChildContent", renderFragment);
    }
}

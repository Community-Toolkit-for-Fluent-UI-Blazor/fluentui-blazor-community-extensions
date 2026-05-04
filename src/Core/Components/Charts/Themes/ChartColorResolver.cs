
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a resolver that provides chart colors based on the current theme and other factors.
/// </summary>
internal sealed class ChartColorResolver
    : IChartColorResolver
{
    /// <summary>
    /// Represents the JavaScript module reference used for interop operations.
    /// </summary>
    /// <remarks>This field holds a reference to a loaded JavaScript module, enabling calls to JavaScript
    /// functions from .NET code via JS interop. The module should be properly disposed when no longer needed to release
    /// resources.</remarks>
    private readonly IJSObjectReference _module;

    /// <summary>
    /// Initializes a new instance of the ChartColorResolver class using the specified JavaScript module reference.
    /// </summary>
    /// <param name="module">The JavaScript module reference used to interact with chart color resolution logic. Cannot be null.</param>
    public ChartColorResolver(IJSObjectReference module)
    {
        _module = module;
    }

    /// <inheritdoc />
    public async Task<Srgb8> ResolveAsync(
        Srgb8? srgb8Value,
        string? varValue,
        Srgb8 fallback)
    {
        if (srgb8Value.HasValue)
        {
            return srgb8Value.Value;
        }

        if (!string.IsNullOrWhiteSpace(varValue))
        {
            var css = await _module.InvokeAsync<string>(
                "FluentUI.Blazor.Community.Charts.ResolveCssColor",
                varValue);

            if (!string.IsNullOrWhiteSpace(css) &&
                Srgb8.TryParse(css, out var parsed))
            {
                return parsed;
            }
        }

        return fallback;
    }
}

using FluentUI.Blazor.Community.Components.Charts.Themes;
using FluentUI.Blazor.Community.Components.ColorSpace.Spaces;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components.Components.Charts.Themes;

internal sealed class DefaultColorResolver : IChartColorResolver
{
    private readonly IJSObjectReference _module;

    public DefaultColorResolver(IJSObjectReference module)
    {
        _module = module;
    }

    /// <inheritdoc />
    public async Task<Srgb8> ResolveAsync(
        Srgb8? srgb8Value,
        string? varValue,
        Srgb8 fallbackValue)
    {
        if (srgb8Value is not null)
        {
            return srgb8Value.Value;
        }

        if (!string.IsNullOrWhiteSpace(varValue))
        {
            var css = await _module.InvokeAsync<string>(
                "FluentUI.Blazor.Community.Charts.ResolveCssColor",
                varValue);

            if (!string.IsNullOrWhiteSpace(css))
            {
                return Srgb8.Parse(css);
            }
        }

        return fallbackValue;
    }
}

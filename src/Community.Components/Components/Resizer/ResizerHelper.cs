using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an helper for the <see cref="FluentCxResizer"/> components.
/// </summary>
internal static class ResizerHelper
{
    /// <summary>
    /// Represents the resize handler for left to right direction.
    /// </summary>
    private static readonly Dictionary<ResizerHandler, string> _ltrResize = new(EqualityComparer<ResizerHandler>.Default)
    {
        [ResizerHandler.Horizontally] = "top: 0px; right: 0px; bottom: 0px; width: 9px;",
        [ResizerHandler.Vertically] = "left: 0px; right: 0px; bottom: 0px; height: 9px;",
        [ResizerHandler.Both] = "right: 0px; bottom: 0px; width: 9px; height: 9px;"
    };

    /// <summary>
    /// Represents the resize handlers for the right to left direction.
    /// </summary>
    private static readonly Dictionary<ResizerHandler, string> _rtlResize = new(EqualityComparer<ResizerHandler>.Default)
    {
        [ResizerHandler.Horizontally] = "top: 0px; left: 0px; bottom: 0px; width: 9px;",
        [ResizerHandler.Vertically] = "left: 0px; left: 0px; bottom: 0px; height: 9px;",
        [ResizerHandler.Both] = "left: 0px; bottom: 0px; width: 9px; height: 9px;"
    };

    /// <summary>
    /// Gets the resize handlers from the specified <paramref name="localizationDirection"/>.
    /// </summary>
    /// <param name="localizationDirection">Direction used by the app.</param>
    /// <returns>Returns the <see cref="Dictionary{TKey, TValue}"/> of the resize handler to use.</returns>
    internal static Dictionary<ResizerHandler, string> GetFromLocalizationDirection(LocalizationDirection localizationDirection)
    {
        return localizationDirection switch
        {
            LocalizationDirection.RightToLeft => _rtlResize,
            _ => _ltrResize
        };
    }
}

using System.Text.RegularExpressions;

namespace FluentUI.Blazor.Community.Components.Components.Base;

/// <summary>
/// Provides methods for detecting HTML content within strings.
/// </summary>
/// <remarks>This class includes functionality to determine if a string contains HTML elements and to generate a
/// regular expression for matching common HTML tags.</remarks>
internal static partial class HtmlDetector
{
    /// <summary>
    /// Represents a regular expression used to match HTML elements.
    /// </summary>
    /// <remarks>This regular expression is generated and optimized for parsing HTML elements. It is intended
    /// for internal use and should not be modified directly.</remarks>
    private static readonly Regex HtmlElementRegex = HtmlElementGeneratedRegex();

    /// <summary>
    /// Determines whether the specified string contains any HTML elements.
    /// </summary>
    /// <remarks>This method uses a regular expression to check for the presence of HTML tags. It is
    /// case-sensitive and may not detect all variations of HTML.</remarks>
    /// <param name="text">The string to evaluate for HTML content. This parameter can be null or empty, in which case the method returns
    /// false.</param>
    /// <returns>true if the input string contains HTML elements; otherwise, false.</returns>
    public static bool ContainsHtml(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        return HtmlElementRegex.IsMatch(text);
    }

    /// <summary>
    /// Generates a regular expression that matches common HTML elements such as paragraphs, divisions, spans, and other
    /// frequently used tags.
    /// </summary>
    /// <remarks>The generated regular expression is culture-specific, using the "fr-FR" culture, and is
    /// optimized for performance with the <see cref="RegexOptions.Compiled"/> option.</remarks>
    /// <returns>A compiled, case-insensitive <see cref="Regex"/> object that can be used to identify or extract specified HTML
    /// elements from text.</returns>
    [GeneratedRegex("</?(fluent-|p|div|span|br|strong|em|ul|ol|li|h[1-6]|a|img|table|thead|tbody|tr|td|th)[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled, "fr-FR")]
    private static partial Regex HtmlElementGeneratedRegex();
}

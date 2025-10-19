using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to parse subtitle streams and convert them into structured subtitle entries according to
/// configurable options.
/// </summary>
/// <remarks>The parser supports common subtitle text formats and exposes methods for asynchronous, efficient
/// processing of large subtitle files. Parsing behavior, such as line merging, formatting preservation, and
/// normalization, is determined by the specified options. This class is not thread-safe; each instance should be used
/// by a single thread at a time.</remarks>
/// <param name="options">The options that control parsing behavior, formatting, normalization, and line wrapping for subtitle entries. Cannot
/// be null.</param>
public partial class SubtitleParser(SubtitleParserOptions options)
{
    /// <summary>
    /// Invariant culture info for consistent parsing of numbers and dates.
    /// </summary>
    private static readonly CultureInfo s_cultureInfo = CultureInfo.InvariantCulture;

    /// <summary>
    /// Asynchronously parses a subtitle stream and yields each subtitle entry as it is read.
    /// </summary>
    /// <remarks>The method reads the stream line by line and yields entries as they are parsed, allowing for
    /// efficient processing of large subtitle files. The caller is responsible for disposing the input stream after
    /// use. This method does not validate the format of the subtitle data beyond basic parsing; malformed entries may
    /// result in incomplete or skipped results.</remarks>
    /// <param name="stream">The input stream containing subtitle data in a supported text format. The stream must be readable and positioned
    /// at the start of the subtitle content.</param>
    /// <returns>An asynchronous sequence of <see cref="SubtitleEntry"/> objects, each representing a parsed subtitle entry from
    /// the stream.</returns>
    public async IAsyncEnumerable<SubtitleEntry> ParseStreamAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var buffer = new List<string>();
        double start = 0, end = 0;

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();

            if (string.IsNullOrWhiteSpace(line))
            {
                if (buffer.Count > 0)
                {
                    yield return BuildEntry(buffer, start, end);
                    buffer.Clear();
                }
            }
            else
            {
                if (buffer.Count == 0)
                {
                    buffer.Add(line);
                }
                else if (buffer.Count == 1)
                {
                    buffer.Add(line);
                    var match = TimeCodeRegex().Match(line);

                    if (match.Success)
                    {
                        start = TimeToSeconds(match.Groups[1].Value);
                        end = TimeToSeconds(match.Groups[2].Value);
                    }
                }
                else
                {
                    buffer.Add(line);
                }
            }
        }

        if (buffer.Count > 0)
        {
            yield return BuildEntry(buffer, start, end);
        }
    }

    /// <summary>
    /// Builds a subtitle entry from the specified buffer and timing information, applying formatting and wrapping
    /// options as configured.
    /// </summary>
    /// <remarks>The formatting and wrapping of the subtitle text are determined by the current options, such
    /// as merging lines, preserving formatting, maximum line length, and reading speed constraints. Ensure that the
    /// buffer contains at least two lines of metadata followed by the actual subtitle text.</remarks>
    /// <param name="buffer">A list of strings containing the raw subtitle lines to process. The first two lines are typically metadata and
    /// are skipped.</param>
    /// <param name="start">The start time, in seconds, for the subtitle entry.</param>
    /// <param name="end">The end time, in seconds, for the subtitle entry.</param>
    /// <returns>A SubtitleEntry object containing the processed subtitle text and its associated start and end times.</returns>
    private SubtitleEntry BuildEntry(List<string> buffer, double start, double end)
    {
        var textLines = buffer.Skip(2);

        var rawText = options.MergeLines
            ? string.Join(" ", textLines)
            : string.Join(Environment.NewLine, textLines);

        var text = options.KeepFormating ? rawText : CleanSubtitleText(rawText);

        text = NormalizeText(text);

        if (options.MaxLineLength > 0)
        {
            text = WrapText(text, options.MaxLineLength);
        }

        if (options.MaxCharsPerSecond > 0)
        {
            text = WrapByReadingSpeed(text, start, end, options.MaxCharsPerSecond);
        }

        return new SubtitleEntry
        {
            Start = start,
            End = end,
            Text = text
        };
    }

    /// <summary>
    /// Converts a time string in the format "HH:mm:ss" or "HH:mm:ss,fff" to its total number of seconds as a double.
    /// </summary>
    /// <remarks>If the input string includes milliseconds, they are parsed and included in the result as
    /// fractional seconds. The method expects the time components to be separated by colons, and milliseconds (if
    /// present) to be separated by a comma or period. Invalid formats may result in a parsing exception.</remarks>
    /// <param name="time">A string representing the time to convert. The format must be "HH:mm:ss" for hours, minutes, and seconds, or
    /// "HH:mm:ss,fff" for hours, minutes, seconds, and milliseconds. Milliseconds can be separated by a comma or
    /// period.</param>
    /// <returns>The total number of seconds represented by the input time string, including fractional seconds for milliseconds.</returns>
    private static double TimeToSeconds(string time)
    {
        time = time.Replace('.', ',');
        var parts = time.Split([':', ',']);

        int hours = 0, minutes = 0, seconds = 0, milliseconds = 0;

        if (parts.Length >= 3)
        {
            hours = int.Parse(parts[0], s_cultureInfo);
            minutes = int.Parse(parts[1], s_cultureInfo);
            seconds = int.Parse(parts[2], s_cultureInfo);
        }

        if (parts.Length == 4)
        {
            var msStr = parts[3].PadRight(3, '0');
            milliseconds = int.Parse(msStr, s_cultureInfo);
        }

        return new TimeSpan(0, hours, minutes, seconds, milliseconds).TotalSeconds;
    }

    /// <summary>
    /// Removes HTML tags, curly-brace enclosed content, and extra whitespace from a subtitle text string.
    /// </summary>
    /// <remarks>This method is useful for preparing subtitle text for display or further processing by
    /// stripping formatting and metadata. The returned string will not contain any HTML tags, curly-brace enclosed
    /// content, or consecutive whitespace characters.</remarks>
    /// <param name="text">The subtitle text to be cleaned. Cannot be null.</param>
    /// <returns>A string containing the cleaned subtitle text with tags and excess whitespace removed.</returns>
    private static string CleanSubtitleText(string text)
    {
        text = CleanHtmlTags().Replace(text, string.Empty);
        text = RemoveCurlyBraces().Replace(text, string.Empty);
        return RemoveSpaces().Replace(text, " ").Trim();
    }

    /// <summary>
    /// Normalizes the specified text according to the configured normalization option.
    /// </summary>
    /// <remarks>The normalization mode is determined by the current options. Supported modes include lower
    /// case, upper case, and sentence case normalization.</remarks>
    /// <param name="text">The text to be normalized. Cannot be null.</param>
    /// <returns>A normalized string based on the selected normalization mode. If the input is empty, returns the original
    /// string.</returns>
    private string NormalizeText(string text)
    {
        return options.Normalization switch
        {
            TextNormalization.LowerCase => text.ToLowerInvariant(),
            TextNormalization.UpperCase => text.ToUpperInvariant(),
            TextNormalization.Sentence =>
                text.Length > 0
                    ? char.ToUpper(text[0]) + text.Substring(1).ToLowerInvariant()
                    : text,
            _ => text
        };
    }

    /// <summary>
    /// Wraps the specified text into multiple lines so that no line exceeds the given maximum length.
    /// </summary>
    /// <remarks>Words are kept intact; lines are broken at word boundaries. If a single word exceeds the
    /// maximum length, it will appear on its own line.</remarks>
    /// <param name="text">The text to be wrapped into lines.</param>
    /// <param name="maxLength">The maximum number of characters allowed per line. Must be greater than zero.</param>
    /// <returns>A string containing the wrapped text, with lines separated by the system's newline sequence.</returns>
    private static string WrapText(string text, int maxLength)
    {
        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = new StringBuilder();

        foreach (var word in words)
        {
            if (currentLine.Length + word.Length + 1 > maxLength)
            {
                lines.Add(currentLine.ToString().Trim());
                currentLine.Clear();
            }
            currentLine.Append(word + " ");
        }

        if (currentLine.Length > 0)
        {
            lines.Add(currentLine.ToString().Trim());
        }

        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Wraps the specified text into multiple lines based on the allowed character count determined by the reading
    /// speed and duration.
    /// </summary>
    /// <remarks>If the duration is zero or negative, the method returns the original text without
    /// modification. Lines are split to fit the calculated character limit and separated by the system's newline
    /// character.</remarks>
    /// <param name="text">The text to be wrapped according to the calculated character limit.</param>
    /// <param name="start">The start time, in seconds, of the reading interval.</param>
    /// <param name="end">The end time, in seconds, of the reading interval.</param>
    /// <param name="maxCharsPerSecond">The maximum number of characters that can be read per second.</param>
    /// <returns>A string containing the original text split into lines so that each line does not exceed the allowed character
    /// count for the specified duration. If the text fits within the limit, the original text is returned unchanged.</returns>
    private static string WrapByReadingSpeed(string text, double start, double end, int maxCharsPerSecond)
    {
        var duration = end - start;

        if (duration <= 0)
        {
            return text;
        }

        var maxChars = (int)(duration * maxCharsPerSecond);

        if (text.Length <= maxChars)
        {
            return text;
        }

        var chunkSize = maxChars;
        var lines = new List<string>();

        for (var i = 0; i < text.Length; i += chunkSize)
        {
            var length = Math.Min(chunkSize, text.Length - i);
            lines.Add(text.Substring(i, length).Trim());
        }

        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Provides a compiled regular expression that matches time code ranges in the format commonly used in subtitle
    /// files.
    /// </summary>
    /// <remarks>The regular expression matches time codes in the format "hh:mm:ss,fff" or "hh:mm:ss.fff",
    /// allowing for one to three fractional digits. This is typically used to parse subtitle timing lines such as
    /// "00:01:23,456 --> 00:01:25,789".</remarks>
    /// <returns>A <see cref="Regex"/> instance that matches pairs of time codes separated by an arrow ("-->"). The first and
    /// second capturing groups correspond to the start and end time codes, respectively.</returns>
    [GeneratedRegex(@"(\d{1,2}:\d{2}:\d{2}(?:[,.]\d{1,3})?)\s*-->\s*(\d{1,2}:\d{2}:\d{2}(?:[,.]\d{1,3})?)")]
    private static partial Regex TimeCodeRegex();

    /// <summary>
    /// Creates a regular expression that matches HTML tags in a string.
    /// </summary>
    /// <remarks>The returned regular expression uses a non-greedy pattern to match tags enclosed in angle
    /// brackets. This can be used to identify or remove HTML markup from text. The regular expression does not validate
    /// the correctness of HTML syntax and may match malformed tags.</remarks>
    /// <returns>A <see cref="Regex"/> instance configured to match any HTML tag, including opening, closing, and self-closing
    /// tags.</returns>
    [GeneratedRegex("<.*?>")]
    private static partial Regex CleanHtmlTags();

    /// <summary>
    /// Creates a regular expression that matches any substring enclosed in curly braces ("{" and "}").
    /// </summary>
    /// <remarks>The returned regular expression uses the pattern "\{.*?\}" to match the shortest possible
    /// substring between an opening and closing curly brace. This can be used to identify or remove segments of text
    /// enclosed in curly braces from a string.</remarks>
    /// <returns>A <see cref="Regex"/> instance that matches text within curly braces using a non-greedy pattern.</returns>
    [GeneratedRegex(@"\{.*?\}")]
    private static partial Regex RemoveCurlyBraces();

    /// <summary>
    /// Creates a regular expression that matches one or more whitespace characters.
    /// </summary>
    /// <remarks>The returned regular expression can be used to identify or remove spaces, tabs, and other
    /// whitespace characters in strings. This method is generated at compile time and is intended for internal
    /// use.</remarks>
    /// <returns>A <see cref="Regex"/> instance configured to match sequences of whitespace characters.</returns>
    [GeneratedRegex(@"\s+")]
    private static partial Regex RemoveSpaces();
}

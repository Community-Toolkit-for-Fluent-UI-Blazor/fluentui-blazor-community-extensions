using System.Text;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to export a collection of subtitle entries to a SubRip (SRT) file format.
/// </summary>
/// <remarks>This class contains static methods for converting subtitle data into the widely used SRT format,
/// suitable for use with most media players and video editing software. All members are static and thread-safe. The
/// class is intended for use in scenarios where subtitles need to be generated or saved programmatically.</remarks>
public static class SubtitleExporter
{
    /// <summary>
    /// Exports a collection of subtitle entries to a file in SubRip (SRT) format asynchronously.
    /// </summary>
    /// <remarks>The method writes the subtitles in SRT format using UTF-8 encoding. The caller is responsible
    /// for ensuring that the entries are ordered as desired in the output file. This method does not validate the
    /// contents of the entries.</remarks>
    /// <param name="entries">The collection of subtitle entries to export. Each entry defines the start and end times, as well as the
    /// subtitle text to be written to the SRT file.</param>
    /// <param name="path">The file path where the SRT output will be written. If the file exists, it will be overwritten.</param>
    /// <returns>A task that represents the asynchronous export operation.</returns>
    public static async Task ExportToSrtAsync(IEnumerable<SubtitleEntry> entries, string path)
    {
        using var writer = new StreamWriter(path, false, Encoding.UTF8);
        var index = 1;

        foreach (var entry in entries)
        {
            await writer.WriteLineAsync($"{index}");

            var start = SecondsToTime(entry.Start);
            var end = SecondsToTime(entry.End);

            await writer.WriteLineAsync($"{start} --> {end}");
            await writer.WriteLineAsync(entry.Text);
            await writer.WriteLineAsync();

            index++;
        }
    }

    /// <summary>
    /// Converts a time interval specified in seconds to a formatted string in the format "HH:mm:ss,fff".
    /// </summary>
    /// <param name="seconds">The total number of seconds to convert. Must be greater than or equal to zero.</param>
    /// <returns>A string representing the time interval in hours, minutes, seconds, and milliseconds ("HH:mm:ss,fff").</returns>
    private static string SecondsToTime(double seconds)
    {
        var ts = TimeSpan.FromSeconds(seconds);

        return ts.ToString();
    }
}

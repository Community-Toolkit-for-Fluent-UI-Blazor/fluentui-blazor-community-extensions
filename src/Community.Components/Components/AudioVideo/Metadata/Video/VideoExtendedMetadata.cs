namespace FluentUI.Blazor.Community.Components;

public class VideoExtendedMetadata
{
    public string[] Languages { get; set; } = [];
    public string[] Subtitles { get; set; } = [];
    public string? SeriesEpisode { get; set; }
    public string? Season { get; set; }
    public string? Keywords { get; set; }
    public string? Location { get; set; }
    public DateTime? RecordedDate { get; set; }
}

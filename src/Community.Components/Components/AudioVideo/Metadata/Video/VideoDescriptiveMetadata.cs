namespace FluentUI.Blazor.Community.Components;

public class VideoDescriptiveMetadata
{
    public string? Title { get; set; }
    public string? Series { get; set; }
    public string[] Directors { get; set; } = [];
    public string[] Writers { get; set; } = [];
    public string[] Cast { get; set; } = [];
    public string[] Producers { get; set; } = [];
    public string[] Genres { get; set; } = [];
    public uint? Year { get; set; }
    public string? Description { get; set; }
    public string? Comment { get; set; }
}

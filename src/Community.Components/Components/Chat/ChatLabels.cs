namespace FluentUI.Blazor.Community.Components;

public record ChatLabels
{
    public static ChatLabels Default { get; set; } = new ChatLabels();

    public static ChatLabels French { get; set; } = new ChatLabels()
    {
        AudioLabel = "Audio",
        ImagesLabel = "Images",
        MessagesLabel = "Messages",
        DocumentLabel = "Documents",
        VideoLabel = "Vidéos",
        PinnedMessagesLabel = "Épinglés"
    };

    public string MessagesLabel { get; set; } = "Messages";

    public string PinnedMessagesLabel { get; set; } = "Pinned";

    public string AudioLabel { get; set; } = "Audio";

    public string VideoLabel { get; set; } = "Video";

    public string ImagesLabel { get; set; } = "Images";

    public string DocumentLabel { get; set; } = "Documents";
}

using FluentUI.Blazor.Community.Components.Chat.Files;
using FluentUI.Blazor.Community.Components.Components.Base;
using FluentUI.Blazor.Community.Components.Media;
using FluentUI.Blazor.Community.Components.States;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the video inside a <see cref="FluentCxSlideshow"/>.
/// </summary>
public partial class SlideshowVideo : FluentComponentBase
{
    private sealed record InternalVideoMetadata
    {
        public bool IsRevokable { get; init; }

        public string Url { get; init; } = string.Empty;

        public string Name { get; init; } = string.Empty;

        public List<Chapter> Chapters { get; init; } = [];

        public VideoMetadata? Metadata { get; init; }
    }

    /// <summary>
    /// Represents the files in url format.
    /// </summary>
    private readonly List<InternalVideoMetadata> _urlFiles = [];

    /// <summary>
    /// Represents a value indicating whether the video player is currently loading.
    /// </summary>
    private bool _isLoading = true;

    /// <summary>
    /// Initializes a new instance of the SlideshowVideo class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that determine how the slideshow video is managed and displayed. Cannot be null.</param>
    public SlideshowVideo(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the uploader service used for handling file uploads related to the slideshow.
    /// </summary>
    [Inject]
    private IFileUploader FileUploader { get; set; } = default!;

    /// <summary>
    /// Gets or sets the current state of the slideshow, which manages the presentation flow and user interactions.
    /// </summary>
    /// <remarks>The state is injected and should be configured before use to ensure proper functionality of
    /// the slideshow.</remarks>
    [Inject]
    private SlideshowState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the parent slideshow item associated with the current item.
    /// </summary>
    /// <remarks>This property is used to establish a hierarchical relationship between slideshow items. It is
    /// important to ensure that the parent item is set correctly to maintain the intended structure of the
    /// slideshow.</remarks>
    [CascadingParameter]
    private SlideshowItem? Parent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the download button is visible for the video. When set to true, a download button will be displayed, allowing users to download the video content. When set to false, the download button will be hidden, preventing users from downloading the video. The default value is true, meaning that the download button will be visible unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool IsDownloadVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the fullscreen button is disabled for the video. When set to true, the fullscreen button will be disabled, preventing users from entering fullscreen mode for the video. When set to false, the fullscreen button will be enabled, allowing users to enter fullscreen mode. The default value is false, meaning that the fullscreen button will be enabled unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool IsFullscreenDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the playback speed option is visible for the video. When set to true, a playback speed option will be displayed, allowing users to adjust the playback speed of the video. When set to false, the playback speed option will be hidden, preventing users from changing the playback speed. The default value is true, meaning that the playback speed option will be visible unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool IsPlaybackSpeedVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the playlist option is disabled for the video. When set to true, the playlist option will be disabled, preventing users from accessing the playlist. When set to false, the playlist option will be enabled, allowing users to access the playlist. The default value is false, meaning that the playlist option will be enabled unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool IsPlaylistDisabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the settings button is visible for the video. When set to true, a settings button will be displayed, allowing users to access video settings. When set to false, the settings button will be hidden, preventing users from accessing video settings. The default value is true, meaning that the settings button will be visible unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool IsSettingsVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the subtitle option is visible for the video. When set to true, a subtitle option will be displayed, allowing users to enable or disable subtitles. When set to false, the subtitle option will be hidden, preventing users from changing subtitle settings. The default value is true, meaning that the subtitle option will be visible unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool IsSubtitleOptionVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the video quality option is visible for the video. When set to true, a video quality option will be displayed, allowing users to adjust the video quality. When set to false, the video quality option will be hidden, preventing users from changing the video quality. The default value is true, meaning that the video quality option will be visible unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool IsVideoQualityOptionVisible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the thumbnails are visible for the video. When set to true, thumbnails will be displayed, allowing users to see video previews. When set to false, thumbnails will be hidden, preventing users from seeing video previews. The default value is false, meaning that thumbnails will be hidden unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public bool ShowThumbnails { get; set; }

    /// <summary>
    /// Gets or sets the multilingual subtitles for the video. This property allows you to provide subtitles in multiple languages, enhancing accessibility and user experience for a diverse audience. The default value is an empty instance of MultilingualSubtitles, meaning that no subtitles will be available unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public MultilingualSubtitles Subtitles { get; set; } = new MultilingualSubtitles();

    /// <summary>
    /// Gets or sets the render fragment for the video quality content. This property allows you to provide custom content for the video quality option, enabling you to customize the user interface and enhance the user experience when adjusting video quality settings. The default value is null, meaning that no custom content will be provided unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public RenderFragment? VideoQualityContent { get; set; }

    /// <summary>
    /// Gets or sets the render fragment for the video settings content. This property allows you to provide custom content for the video settings option, enabling you to customize the user interface and enhance the user experience when accessing video settings. The default value is null, meaning that no custom content will be provided unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public RenderFragment? VideoSettingsContent { get; set; }

    /// <summary>
    /// Gets or sets the view mode for the video player. This property allows you to specify how the video player should be displayed, such as in a standard view, theater mode, or fullscreen mode. The default value is VideoPlayerView.Standard, meaning that the video player will be displayed in standard view unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public VideoPlayerView View { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is triggered when the view mode of the video player changes. This property allows you to handle changes in the video player's view mode, enabling you to perform actions or update the user interface accordingly when the view mode is changed by the user. The default value is null, meaning that no event callback will be triggered unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public EventCallback<VideoPlayerView> ViewChanged { get; set; }

    /// <summary>
    /// Gets or sets the collection of files associated with the video player.
    /// </summary>
    [Parameter]
    public IEnumerable<IChatFile> Files { get; set; } = [];

    /// <summary>
    /// Gets or sets the event callback that is triggered when a video media requires a chapter list. This property allows you to handle the event when the video player needs to retrieve a list of chapters, enabling you to provide the necessary data or perform actions accordingly when the chapter list is requested by the video player. The default value is null, meaning that no event callback will be triggered unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public EventCallback<ChapterEventArgs> GetChapters { get; set; }

    /// <summary>
    /// Gets or sets the event callback that is triggered when a video media requires metadata information. This property allows you to handle the event when the video player needs to retrieve metadata information about the video, enabling you to provide the necessary data or perform actions accordingly when the metadata information is requested by the video player. The default value is null, meaning that no event callback will be triggered unless explicitly set otherwise.
    /// </summary>
    [Parameter]
    public EventCallback<VideoMetadataEventArgs> GetVideoMetadata { get; set; }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (!firstRender)
        {
            return;
        }

        foreach (var file in Files)
        {
            if (file is IBinaryChatFile binaryFile)
            {
                var url = await FileUploader.UploadFileAsync(Invariant.ToString(binaryFile.Id), binaryFile.Content, binaryFile.ContentType);

                if (!string.IsNullOrEmpty(url))
                {
                    var internalFile = new InternalVideoMetadata()
                    {
                        IsRevokable = true,
                        Name = binaryFile.Name,
                        Url = url
                    };

                    if (GetChapters.HasDelegate)
                    {
                        var chapters = new List<Chapter>();
                        var chapterArgs = new ChapterEventArgs(binaryFile.Name, url);
                        await GetChapters.InvokeAsync(chapterArgs);

                        if (chapterArgs.Chapters is not null)
                        {
                            chapters.AddRange(chapterArgs.Chapters);
                        }

                        internalFile = internalFile with { Chapters = chapters };
                    }

                    if (GetVideoMetadata.HasDelegate)
                    {
                        var metadataArgs = new VideoMetadataEventArgs(binaryFile.Name, url);
                        await GetVideoMetadata.InvokeAsync(metadataArgs);

                        if (metadataArgs.Metadata is not null)
                        {
                            internalFile = internalFile with { Metadata = metadataArgs.Metadata };
                        }
                    }

                    _urlFiles.Add(internalFile);
                }
            }
            else if (file is IUrlChatFile urlFile && !string.IsNullOrEmpty(urlFile.Url))
            {
                var internalFile = new InternalVideoMetadata()
                {
                    Name = urlFile.Name,
                    Url = urlFile.Url
                };

                if (GetChapters.HasDelegate)
                {
                    var chapters = new List<Chapter>();
                    var chapterArgs = new ChapterEventArgs(urlFile.Name, urlFile.Url);
                    await GetChapters.InvokeAsync(chapterArgs);

                    if (chapterArgs.Chapters is not null)
                    {
                        chapters.AddRange(chapterArgs.Chapters);
                    }

                    internalFile = internalFile with { Chapters = chapters };
                }

                if (GetVideoMetadata.HasDelegate)
                {
                    var metadataArgs = new VideoMetadataEventArgs(urlFile.Name, urlFile.Url);
                    await GetVideoMetadata.InvokeAsync(metadataArgs);

                    if (metadataArgs.Metadata is not null)
                    {
                        internalFile = internalFile with { Metadata = metadataArgs.Metadata };
                    }
                }

                _urlFiles.Add(internalFile);
            }
        }

        _isLoading = false;
        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.Add(this);
    }

    /// <inheritdoc/>
    public override async ValueTask DisposeAsync()
    {
        State?.RemoveSize(Id);

        foreach (var item in _urlFiles.Where(x => x.IsRevokable))
        {
            await FileUploader.RevokeUrlAsync(item.Url);
        }

        Parent?.Remove(this);

        await base.DisposeAsync();
    }
}

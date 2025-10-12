namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of localized labels for audio player controls and metadata fields.
/// </summary>
/// <remarks>Use this record to provide user interface text for audio playback controls and related metadata in
/// different languages. Predefined instances such as <see cref="French"/> are available for common localizations, or
/// you can create a custom set of labels as needed. All properties are immutable and can be initialized using object
/// initializer syntax.</remarks>
public record AudioVideoLabels
{
    /// <summary>
    /// Gets the default set of audio labels used by the application.
    /// </summary>
    public static AudioVideoLabels Default { get; } = new AudioVideoLabels();

    /// <summary>
    /// Gets the set of audio interface labels localized in French.
    /// </summary>
    /// <remarks>Use this property to access French translations for standard audio player controls and
    /// metadata labels. This is useful for applications that support multiple languages and need to display user
    /// interface elements in French.</remarks>
    public static AudioVideoLabels French { get; } = new AudioVideoLabels
    {
        PlayLabel = "Lire",
        PauseLabel = "Pause",
        PreviousLabel = "Piste précédente",
        NextLabel = "Piste suivante",
        VolumeLabel = "Volume",
        ShuffleOnLabel = "Mélange activé",
        ShuffleOffLabel = "Mélange désactivé",
        PlaylistLabel = "Liste de lecture",
        DownloadLabel = "Télécharger",
        StopLabel = "Arrêter",
        PlaylistLoopLabel = "Automatic playback of the next music track and repeat of the playlist",
        PlaylistOnceLabel = "Automatic playback of the next music track and stop at the end of the last track",
        SingleLoopLabel = "Play the music on loop",
        SingleOnceLabel = "Stop playing at the end of the music",
        TitleLabel = "Titre",
        AlbumArtistsLabel = "Artistes ayant collaboré",
        PropertiesLabel = "Propriétés",
        CloseLabel = "Fermer",
        AlbumLabel = "Album",
        AlbumArtistLabel = "Artiste de l'album",
        TrackNumberLabel = "Numéro de piste",
        DurationLabel = "Durée",
        GenreLabel = "Genre",
        YearLabel = "Année",
        BitrateLabel = "Débit binaire",
        MimeLabel = "Type MIME",
        PublisherLabel = "Éditeur",
        CoverLabel = "Pochette",
        VideoSettingsLabel = "Paramètres vidéo",
        PiPLabel = "Image dans l'image",
        FullscreenLabel = "Plein écran",
        UnkownVideoTitle = "Titre vidéo inconnu",
        TheaterLabel = "Mode Théâtre",
        SubtitlesLabel = "Sous-titres",
        PlaybackSpeedLabel = "Vitesse de lecture",
        VideoQualityLabel = "Qualité vidéo",
        PlaybackSpeedLabelCustomFormat = "Vitesse personnalisée : {0}x",
        NormalPlaybackSpeedLabel = "Vitesse normale",
        ApplyLabel = "Appliquer",
        CancelLabel = "Annuler",
        SubtitleBackgroundColorLabel = "Couleur de fond des sous-titres",
        SubtitleBackgroundOpacity = "Opacité de l'arrière-plan des sous-titres",
        SubtitleSolidBackgroundLabel = "Solide",
        SubtitleHalfTransparentBackgroundLabel = "Semi-transparent",
        SubtitleTransparentBackgroundLabel = "Transparent",
        SubtitleOpaqueBackgroundLabel = "Opaque",
        AudioChannelsLabel = "Canaux audio",
        DirectorsLabel = "Réalisateurs",
        ResolutionLabel = "Résolution",
        FramerateLabel = "Fréquence d'images",
        DefaultLabel = "Défaut"
    };

    /// <summary>
    /// Gets the label for the play button.
    /// </summary>
    public string PlayLabel { get; init; } = "Play";

    /// <summary>
    /// Gets the label for the pause button.
    /// </summary>
    public string PauseLabel { get; init; } = "Pause";

    /// <summary>
    /// Gets the label for the previous button.
    /// </summary>
    public string PreviousLabel { get; init; } = "Previous track";

    /// <summary>
    /// Gets the label for the next button.
    /// </summary>
    public string NextLabel { get; init; } = "Next track";

    /// <summary>
    /// Gets the label for the volume control.
    /// </summary>
    public string VolumeLabel { get; init; } = "Volume";

    /// <summary>
    /// Gets the label for the shuffle on button.
    /// </summary>
    public string ShuffleOnLabel { get; init; } = "Shuffle on";

    /// <summary>
    /// Gets the label for the shuffle off button.
    /// </summary>
    public string ShuffleOffLabel { get; init; } = "Shuffle off";

    /// <summary>
    /// Gets the label for the playlist button.
    /// </summary>
    public string PlaylistLabel { get; init; } = "Playlist";

    /// <summary>
    /// Gets the label for the download button.
    /// </summary>
    public string DownloadLabel { get; init; } = "Download";

    /// <summary>
    /// Gets the label for the stop button.
    /// </summary>
    public string StopLabel { get; init; } = "Stop";

    /// <summary>
    /// Gets the label for the playlist loop mode.
    /// </summary>
    public string PlaylistLoopLabel { get; init; } = "Playlist loop";

    /// <summary>
    /// Gets the label for the playlist once mode.
    /// </summary>
    public string PlaylistOnceLabel { get; init; } = "Playlist once";

    /// <summary>
    /// Gets the label for the single loop mode.
    /// </summary>
    public string SingleLoopLabel { get; init; } = "Single loop";

    /// <summary>
    /// Gets the label for the single once mode.
    /// </summary>
    public string SingleOnceLabel { get; init; } = "Single once";

    /// <summary>
    /// Gets the label for the title field.
    /// </summary>
    public string TitleLabel { get; init; } = "Title";

    /// <summary>
    /// Gets the label for the performers field.
    /// </summary>
    public string AlbumArtistsLabel { get; init; } = "Artists who have collaborated";

    /// <summary>
    /// Gets the label for the properties section.
    /// </summary>
    public string PropertiesLabel { get; init; } = "Properties";

    /// <summary>
    /// Gets the label for close dialog.
    /// </summary>
    public string CloseLabel { get; init; } = "Close";

    /// <summary>
    /// Gets the label for the album field.
    /// </summary>
    public string AlbumLabel { get; init; } = "Album";

    /// <summary>
    /// Gets the display label used for the album artist field.
    /// </summary>
    public string AlbumArtistLabel { get; init; } = "Album artist";

    /// <summary>
    /// Gets the label for the track number field.
    /// </summary>
    public string TrackNumberLabel { get; init; } = "Track number";

    /// <summary>
    /// Gets the label for the duration field.
    /// </summary>
    public string DurationLabel { get; init; } = "Duration";

    /// <summary>
    /// Gets the label text used to represent the genre field.
    /// </summary>
    public string GenreLabel { get; init; } = "Genre";

    /// <summary>
    /// Gets the label text used to represent a year.
    /// </summary>
    public string YearLabel { get; init; } = "Year";

    /// <summary>
    /// Gets the display label used to represent the bitrate value.
    /// </summary>
    public string BitrateLabel { get; init; } = "Bitrate";

    /// <summary>
    /// Gets the label for the MIME type field.
    /// </summary>
    public string MimeLabel { get; init; } = "MIME type";

    /// <summary>
    /// Gets the display label used for the publisher field.
    /// </summary>
    public string PublisherLabel { get; init; } = "Publisher";

    /// <summary>
    /// Gets the label for the cover field.
    /// </summary>
    public string CoverLabel { get; init; } = "Cover";

    /// <summary>
    /// Gets the label for the video settings button.
    /// </summary>
    public string VideoSettingsLabel { get; init; } = "Video Settings";

    /// <summary>
    /// Gets the label for the Picture in Picture button.
    /// </summary>
    public string PiPLabel { get; init; } = "Picture In Picture";

    /// <summary>
    /// Gets the label for the fullscreen button.
    /// </summary>
    public string FullscreenLabel { get; init; } = "Fullscreen";

    /// <summary>
    /// Gets the default title used when a video's title is unknown or unavailable.
    /// </summary>
    public string UnkownVideoTitle { get; init; } = "Unknown Video Title";

    /// <summary>
    /// Gets the display label for the theater mode option.
    /// </summary>
    public string TheaterLabel { get; init; } = "Theater Mode";

    /// <summary>
    /// Gets the label text used for subtitles in the user interface.
    /// </summary>
    public string SubtitlesLabel { get; init; } = "Subtitles";

    /// <summary>
    /// Gets the display label for the playback speed control.
    /// </summary>
    public string PlaybackSpeedLabel { get; init; } = "Playback Speed";

    /// <summary>
    /// Gets the display label for the video quality selection option.
    /// </summary>
    public string VideoQualityLabel { get; init; } = "Video Quality";

    /// <summary>
    /// Gets the custom format string used to display the playback speed label.
    /// </summary>
    /// <remarks>The format string should include a placeholder, such as "{0}", which will be replaced with
    /// the current playback speed value. This allows customization of how the playback speed is presented to
    /// users.</remarks>
    public string PlaybackSpeedLabelCustomFormat { get; init; } = "Custom Speed : {0}x";

    /// <summary>
    /// Gets the display label used to represent normal playback speed in the user interface.
    /// </summary>
    public string NormalPlaybackSpeedLabel { get; init; } = "Normal Speed";

    /// <summary>
    /// Gets the label to apply to the item.
    /// </summary>
    public string ApplyLabel { get; init; } = "Apply";

    /// <summary>
    /// Gets the text label to display for a cancel action, such as on a button or menu item.
    /// </summary>
    public string CancelLabel { get; init; } = "Cancel";

    /// <summary>
    /// Gets the label for the subtitle background style option.
    /// </summary>
    public string SubtitleBackgroundColorLabel { get; init; } = "Subtitle Background Color";

    /// <summary>
    /// Gets the label for the subtitle background opacity option.
    /// </summary>
    public string SubtitleBackgroundOpacity { get; init; } = "Subtitle Background Opacity";

    /// <summary>
    /// Gets the label for the solid subtitle background style option.
    /// </summary>
    public string SubtitleSolidBackgroundLabel { get; init; } = "Solid";

    /// <summary>
    /// Gets the label text displayed for the half-transparent background subtitle option.
    /// </summary>
    public string SubtitleHalfTransparentBackgroundLabel { get; init; } = "Semi-Transparent";

    /// <summary>
    /// Gets the label text used for the subtitle transparent background option.
    /// </summary>
    public string SubtitleTransparentBackgroundLabel { get; init; } = "Transparent";

    /// <summary>
    /// Gets the label text for the opaque background option in subtitle settings.
    /// </summary>
    public string SubtitleOpaqueBackgroundLabel { get; init; } = "Opaque";

    /// <summary>
    /// Gets the label text used to display directors in the user interface.
    /// </summary>
    public string DirectorsLabel { get; init; } = "Directors";

    /// <summary>
    /// Gets the label used to display the resolution setting.
    /// </summary>
    public string ResolutionLabel { get; init; } = "Resolution";

    /// <summary>
    /// Gets the display label for the frame rate setting.
    /// </summary>
    public string FramerateLabel { get; init; } = "Frame rate";

    /// <summary>
    /// Gets the label used to identify the audio channel.
    /// </summary>
    public string AudioChannelsLabel { get; init; } = "Audio channels";

    /// <summary>
    /// Gets the default label.
    /// </summary>
    public string DefaultLabel { get; init; } = "Default";
}

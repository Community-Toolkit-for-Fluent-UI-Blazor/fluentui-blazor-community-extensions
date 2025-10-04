namespace FluentUI.Blazor.Community.Components;

public record AudioLabels
{
    public static AudioLabels Default { get; } = new AudioLabels();

    public static AudioLabels French { get; } = new AudioLabels
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
        CoverLabel = "Pochette"
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
}

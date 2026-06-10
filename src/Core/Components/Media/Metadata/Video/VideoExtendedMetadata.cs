namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents additional metadata for a video, including language options, subtitle information, episode and season
/// details, keywords, location, and recording date.
/// </summary>
/// <remarks>This class provides extended information that can be used to categorize, search, or enhance playback
/// of video content. It supports multiple languages and subtitles, and includes fields for series and season
/// identification, which are useful for episodic content. The metadata can assist in organizing video libraries and
/// improving user experience during playback.</remarks>
public class VideoExtendedMetadata
{
    /// <summary>
    /// Gets or sets the array of programming languages supported by the application.
    /// </summary>
    /// <remarks>This property allows the user to specify multiple programming languages. The array can be
    /// empty, indicating no languages are currently supported.</remarks>
    public string[] Languages { get; set; } = [];

    /// <summary>
    /// Gets or sets the array of subtitles associated with the content.
    /// </summary>
    /// <remarks>Subtitles can be used to provide additional context or translations for the content. The
    /// array may be empty if no subtitles are available.</remarks>
    public string[] Subtitles { get; set; } = [];

    /// <summary>
    /// Gets or sets the episode title of the series.
    /// </summary>
    /// <remarks>This property can be null if the episode title is not specified. It is typically used to
    /// identify a specific episode within a series.</remarks>
    public string? SeriesEpisode { get; set; }

    /// <summary>
    /// Gets or sets the season associated with the current context. This property can be used to categorize or filter
    /// data based on seasonal information.
    /// </summary>
    /// <remarks>The value of this property may be null, indicating that no specific season is assigned. This
    /// property is useful for applications that require seasonal data representation or filtering.</remarks>
    public string? Season { get; set; }

    /// <summary>
    /// Gets or sets the keywords associated with the item, which can be used for search and categorization purposes.
    /// </summary>
    /// <remarks>This property allows for the specification of multiple keywords, which can enhance the
    /// discoverability of the item in search operations. Keywords should be separated by commas.</remarks>
    public string? Keywords { get; set; }

    /// <summary>
    /// Gets or sets the location associated with the entity.
    /// </summary>
    /// <remarks>This property can be null, indicating that no location has been specified.</remarks>
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the record was created.
    /// </summary>
    /// <remarks>This property can be null if the record date is not specified. It is important to set this
    /// value to a valid <see cref="DateTime"/> to ensure accurate record keeping.</remarks>
    public DateTime? RecordedDate { get; set; }
}

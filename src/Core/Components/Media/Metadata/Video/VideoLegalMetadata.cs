namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents legal metadata information associated with a video, including copyright, publisher, studio, license, and
/// rating details.
/// </summary>
/// <remarks>This class provides properties for storing legal and attribution information relevant to video
/// content. Such metadata can be used to ensure compliance with distribution requirements, facilitate content
/// management, and support rights management workflows.</remarks>
public class VideoLegalMetadata
{
    /// <summary>
    /// Gets or sets the copyright information associated with the entity.
    /// </summary>
    /// <remarks>This property can be used to specify the copyright details, which may include the copyright
    /// holder's name and the year of copyright. It is recommended to provide accurate and up-to-date copyright
    /// information to ensure proper legal protection.</remarks>
    public string? Copyright { get; set; }

    /// <summary>
    /// Gets or sets the name of the publisher associated with the item.
    /// </summary>
    public string? Publisher { get; set; }

    /// <summary>
    /// Gets or sets the name of the studio associated with the current context.
    /// </summary>
    /// <remarks>This property can be null if no studio is specified. It is typically used to identify the
    /// studio responsible for the production or development of the content.</remarks>
    public string? Studio { get; set; }

    /// <summary>
    /// Gets or sets the license information associated with the entity.
    /// </summary>
    /// <remarks>This property can be null if no license information is provided. Ensure to validate the
    /// license format before usage.</remarks>
    public string? License { get; set; }

    /// <summary>
    /// Gets or sets the rating associated with the item. This value can be used to represent user feedback or quality
    /// assessment.
    /// </summary>
    /// <remarks>The rating can be null, indicating that no rating has been assigned. Ensure that the rating
    /// value adheres to any expected format or constraints defined by the application.</remarks>
    public string? Rating { get; set; }
}

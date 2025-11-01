namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of metadata categories associated with an audio asset.
/// </summary>
/// <remarks>This class provides access to various types of metadata, including descriptive, technical, legal, 
/// and visual information. Each metadata category is represented by a corresponding property, which  can be used to
/// retrieve or modify the associated metadata.</remarks>
public sealed class AudioMetadata
{
    /// <summary>
    /// Gets or sets the descriptive metadata associated with the current object.
    /// </summary>
    public AudioDescriptiveMetadata Descriptive { get; set; } = new AudioDescriptiveMetadata();

    /// <summary>
    /// Gets or sets the extended metadata associated with the current object.
    /// </summary>
    public AudioExtendedMetadata Extended { get; set; } = new AudioExtendedMetadata();

    /// <summary>
    /// Gets or sets the technical metadata associated with the current object.
    /// </summary>
    public AudioTechnicalMetadata Technical { get; set; } = new AudioTechnicalMetadata();

    /// <summary>
    /// Gets or sets the legal metadata associated with the current object.
    /// </summary>
    public AudioLegalMetadata Legal { get; set; } = new AudioLegalMetadata();

    /// <summary>
    /// Gets or sets the visual metadata associated with the current object.
    /// </summary>
    public AudioVisualMetadata Visual { get; set; } = new AudioVisualMetadata();
}

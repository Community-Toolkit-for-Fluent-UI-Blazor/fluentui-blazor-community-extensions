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
    public DescriptiveMetadata Descriptive { get; set; } = new DescriptiveMetadata();

    /// <summary>
    /// Gets or sets the extended metadata associated with the current object.
    /// </summary>
    public ExtendedMetadata Extended { get; set; } = new ExtendedMetadata();

    /// <summary>
    /// Gets or sets the technical metadata associated with the current object.
    /// </summary>
    public TechnicalMetadata Technical { get; set; } = new TechnicalMetadata();

    /// <summary>
    /// Gets or sets the legal metadata associated with the current object.
    /// </summary>
    public LegalMetadata Legal { get; set; } = new LegalMetadata();

    /// <summary>
    /// Gets or sets the visual metadata associated with the current object.
    /// </summary>
    public VisualMetadata Visual { get; set; } = new VisualMetadata();
}

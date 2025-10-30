namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents metadata related to legal information, such as copyright and publisher details.
/// </summary>
/// <remarks>This class is typically used to store and manage legal information associated with a document, 
/// product, or other content. Both properties are optional and can be set to <see langword="null"/>  if the
/// corresponding information is not available.</remarks>
public class LegalMetadata
{
    /// <summary>
    /// Gets or sets the copyright information associated with the application.
    /// </summary>
    public string? Copyright { get; set; }

    /// <summary>
    /// Gets or sets the name of the publisher associated with the item.
    /// </summary>
    public string? Publisher { get; set; }
}

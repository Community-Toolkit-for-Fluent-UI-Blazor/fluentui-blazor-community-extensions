namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a set of rules used to compute the final "accept" attribute
/// for the <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
internal sealed class AcceptFileRules
{
    /// <summary>
    /// Gets or sets the file categories that are accepted by the component.
    /// </summary>
    /// <remarks>Use this property to specify which types of files the component should allow. Multiple
    /// categories can be combined using a bitwise OR operation if the underlying type supports flags.</remarks>
    public AcceptFileCategory Categories { get; set; } = AcceptFileCategory.None;

    /// <summary>
    /// Gets or sets the accepted file extensions for file selection operations.
    /// </summary>
    /// <remarks>Use this property to specify which file types are allowed when selecting files. The value
    /// determines the set of file extensions that will be accepted by the component or control. Setting this property
    /// to a specific value restricts file selection to the specified extensions.</remarks>
    public AcceptFileExtension Extensions { get; set; } = AcceptFileExtension.None;

    /// <summary>
    /// Gets the list of custom file extensions supported by the component.
    /// </summary>
    public List<string> CustomExtensions { get; } = [];

    /// <summary>
    /// Adds a custom file extension to the collection if it is not already present.
    /// </summary>
    /// <remarks>The method normalizes the extension by trimming white space, converting it to lowercase, and
    /// ensuring it starts with a period before adding it to the collection. Duplicate extensions are ignored.</remarks>
    /// <param name="extension">The file extension to add. The value can include or omit the leading period and is case-insensitive. If the
    /// value is null, empty, or consists only of white-space characters, the method does nothing.</param>
    public void AddCustom(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return;
        }

        extension = extension.Trim().ToLowerInvariant();

        if (!extension.StartsWith('.'))
        {
            extension = "." + extension;
        }

        if (!CustomExtensions.Contains(extension))
        {
            CustomExtensions.Add(extension);
        }
    }

    /// <summary>
    /// Builds a comma-separated string representing the accepted file types based on the configured categories, known
    /// extensions, and custom extensions.
    /// </summary>
    /// <remarks>The resulting string can be used to specify allowed file types in file upload controls, such
    /// as the HTML input element's accept attribute. Duplicate values are removed to ensure each file type appears only
    /// once.</remarks>
    /// <returns>A comma-separated string of accepted file types suitable for use in file input accept attributes. The string
    /// contains unique values derived from categories, known extensions, and custom extensions.</returns>
    public string ToAcceptString()
    {
        var list = new List<string>();

        list.AddRange(AcceptFileCategoryMap.Resolve(Categories));
        list.AddRange(AcceptFileExtensionMap.Resolve(Extensions));
        list.AddRange(CustomExtensions);

        return string.Join(",", list.Distinct());
    }
}

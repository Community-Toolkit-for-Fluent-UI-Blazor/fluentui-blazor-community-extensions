using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the icons used by <see cref="FluentCxFileManager{TItem}"/>.
/// </summary>
public static partial class FileIcons
{
    /// <summary>
    /// Retrieves the icon associated with the specified file extension and view mode.
    /// </summary>
    /// <param name="extension">The file extension for which to retrieve the icon. The value should include the leading period (e.g., ".txt").</param>
    /// <param name="view">The file view mode that determines the style or size of the icon to return.</param>
    /// <returns>An icon representing the specified file extension in the given view mode.</returns>
    public static Icon GetIcon(string extension, FileView view)
    {
        var key = FileIconRegistry.Resolve(extension);

        return FileIconFactory.Get(key, view);
    }
}


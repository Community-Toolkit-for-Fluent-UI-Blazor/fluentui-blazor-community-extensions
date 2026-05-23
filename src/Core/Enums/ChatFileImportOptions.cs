namespace FluentUI.Blazor.Community.Components.Enums;

/// <summary>
/// Represents the options for importing files in a chat context.
/// </summary>
public enum ChatFileImportOptions
{
    /// <summary>
    /// Only import files from the cloud.
    /// </summary>
    Cloud,

    /// <summary>
    /// Only import files from the local device.
    /// </summary>
    Local,

    /// <summary>
    /// Import files from both the cloud and the local device.
    /// </summary>
    Both
}

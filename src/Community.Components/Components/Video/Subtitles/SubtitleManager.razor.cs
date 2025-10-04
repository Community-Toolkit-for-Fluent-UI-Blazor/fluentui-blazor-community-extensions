using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality for managing subtitle languages, including tracking the currently selected language and the
/// set of available options.
/// </summary>
public partial class SubtitleManager
{
    /// <summary>
    /// Gets or sets all the available subtitle languages.
    /// </summary>
    [Parameter]
    public List<SubtitleLanguage> AvailableLanguages { get; set; } = [];

    /// <summary>
    /// Gets or sets the current state of subtitle rendering and configuration.
    /// </summary>
    /// <remarks>Use this property to access or modify subtitle settings, such as visibility, language, or
    /// style, within the component. Changes to this state may affect how subtitles are displayed to the user.</remarks>
    [Inject]
    private SubtitleState SubtitleState { get; set; } = null!;
}

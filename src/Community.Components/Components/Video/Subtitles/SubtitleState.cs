namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the state of subtitle management, including the currently selected subtitle language.
/// </summary>
internal sealed class SubtitleState
{
    /// <summary>
    /// Represents the currently selected subtitle language.
    /// </summary>
    private SubtitleLanguage? _selectedLanguage;

    /// <summary>
    /// Occurs when the selected subtitle language changes.
    /// </summary>
    /// <remarks>The event is raised whenever the subtitle language selection is updated, including when the
    /// selection is cleared. Subscribers can use the event arguments to determine the new language or detect when no
    /// language is selected.</remarks>
    public event EventHandler<SubtitleLanguage?>? SelectedLanguageChanged;

    /// <summary>
    /// Gets or sets the currently selected subtitle language.
    /// </summary>
    public SubtitleLanguage? SelectedLanguage
    {
        get => _selectedLanguage;
        set
        {
            if (_selectedLanguage != value)
            {
                _selectedLanguage = value;
                SelectedLanguageChanged?.Invoke(this, _selectedLanguage);
            }
        }
    }
}

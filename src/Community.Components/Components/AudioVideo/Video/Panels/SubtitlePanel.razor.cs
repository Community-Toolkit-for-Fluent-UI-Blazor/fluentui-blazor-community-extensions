using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel that displays subtitle options for the current context.
/// </summary>
public partial class SubtitlePanel
{
    /// <summary>
    /// Represents the currently selected subtitle language.
    /// </summary>
    private SubtitleLanguage? _selectedLanguage;

    /// <summary>
    /// Represents the currently selected subtitle background color.
    /// </summary>
    private string? _selectedBackgroundColor;

    /// <summary>
    /// Represents the currently selected subtitle background style.
    /// </summary>
    private SubtitleBackground _selectedBackgroundOpacity = SubtitleBackground.Solid;

    /// <summary>
    /// Gets or sets the audio and video labels along with subtitle options for the current context.
    /// </summary>
    [Parameter, EditorRequired]
    public (AudioVideoLabels, SubtitleOptions, IEnumerable<SubtitleLanguage>) Content { get; set; } = default!;

    /// <summary>
    /// Gets or sets the dialog instance that hosts this panel.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <summary>
    /// Gets or sets the video state.
    /// </summary>
    [Inject]
    private VideoState VideoState { get; set; } = null!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _selectedBackgroundColor = Content.Item2.BackgroundColor;
        _selectedLanguage = Content.Item2.SelectedLanguage;
        _selectedBackgroundOpacity = Content.Item2.Background;
    }

    private string? GetOptionText(SubtitleBackground value)
    {
        return value switch
        {
            SubtitleBackground.Solid => Content.Item1.SubtitleSolidBackgroundLabel,
            SubtitleBackground.SemiTransparent => Content.Item1.SubtitleHalfTransparentBackgroundLabel,
            SubtitleBackground.Transparent => Content.Item1.SubtitleTransparentBackgroundLabel,
            SubtitleBackground.Opaque => Content.Item1.SubtitleOpaqueBackgroundLabel,
            _ => null
        };
    }

    /// <summary>
    /// Performs cleanup and closes the dialog asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    private async Task OnCloseAsync()
    {
        VideoState.SubtitleOptions = new()
        {
            Background = _selectedBackgroundOpacity,
            BackgroundColor = _selectedBackgroundColor,
            SelectedLanguage = _selectedLanguage
        };

        await Dialog.CloseAsync();
    }

    /// <summary>
    /// Asynchronously cancels the current dialog operation.
    /// </summary>
    /// <returns>A task that represents the asynchronous cancel operation.</returns>
    private async Task OnCancelAsync()
    {
        await Dialog.CancelAsync();
    }
}

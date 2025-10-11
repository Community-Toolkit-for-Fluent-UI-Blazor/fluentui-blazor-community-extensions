using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a panel that displays subtitle options for the current context.
/// </summary>
public partial class SubtitlePanel
{
    /// <summary>
    /// Gets or sets the audio and video labels along with subtitle options for the current context.
    /// </summary>
    [Parameter, EditorRequired]
    public (AudioVideoLabels, SubtitleOptions) Content { get; set; } = default!;

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
}

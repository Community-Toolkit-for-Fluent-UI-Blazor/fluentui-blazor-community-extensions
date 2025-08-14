using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the slide show item containing the text to render.
/// </summary>
public partial class ChatMessageSlideShowItemText
    : FluentComponentBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatMessageSlideShowItemText"/> class.
    /// </summary>
    public ChatMessageSlideShowItemText()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the owner of the view.
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the sections of the message.
    /// </summary>
    [Parameter]
    public List<IChatMessageSection> Sections { get; set; } = [];

    private string? GetText()
    {
        if (Sections.Count == 0)
        {
            return string.Empty;
        }

        var section = Sections.Find(x => x.CultureId == Owner!.CultureId);

        if (section is not null)
        {
            return section.Content;
        }

        return Sections[0].Content;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Owner is null)
        {
            throw new InvalidOperationException("The owner of the component must be set.");
        }
    }
}

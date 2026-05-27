using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Chat.UI.Viewers;

/// <summary>
/// Represents a Blazor component for viewing and displaying documents within a chat interface.
/// </summary>
public partial class ChatDocumentViewer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatDocumentViewer"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration.</param>
    public ChatDocumentViewer(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the callback that provides chat file items to the component.
    /// </summary>
    [Parameter]
    public IReadOnlyList<ChatFileEventArgs> Items { get; set; } = [];
}

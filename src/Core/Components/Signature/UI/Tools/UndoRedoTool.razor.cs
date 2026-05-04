using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a user interface component that provides undo and redo functionality, allowing users to reverse or
/// reapply actions within an application.
/// </summary>
/// <remarks>The UndoRedoTool component can be configured to act as either an undo or redo button, depending on
/// the value of the IsRedoButton property. It supports disabling user interaction and exposes a callback for handling
/// button clicks. This component is intended for use within applications that require command history management or
/// editing workflows.</remarks>
public partial class UndoRedoTool : FluentComponentBase
{
    /// <summary>
    /// Represents the icon used for the redo action with a 24-pixel size arrow graphic.
    /// </summary>
    private static readonly Icon s_redoIcon = new Size24.ArrowRedo();

    /// <summary>
    /// Represents the icon used for the undo action, using the 24-pixel arrow undo graphic.
    /// </summary>
    private static readonly Icon s_undoIcon = new Size24.ArrowUndo();

    /// <summary>
    /// Initializes a new instance of the <see cref="UndoRedoTool"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to use for this component. Cannot be null.</param>
    public UndoRedoTool(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the callback that is invoked when the clear tool button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is disabled and cannot be interacted with.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, the component will not respond to user input and may
    /// appear visually distinct to indicate its disabled state.</remarks>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this button functions as a Redo action button.
    /// </summary>
    [Parameter]
    public bool IsRedoButton { get; set; }

    /// <summary>
    /// Gets the icon to display for the button, depending on whether the button represents a redo or undo action.
    /// </summary>
    private Icon IconButton => IsRedoButton ? s_redoIcon : s_undoIcon;
}

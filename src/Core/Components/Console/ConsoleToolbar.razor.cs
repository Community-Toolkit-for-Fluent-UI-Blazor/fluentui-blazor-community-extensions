using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a toolbar component for a console interface, providing controls for managing the console's behavior, such as pausing and resuming message updates. This component is designed to be used within a console or logging interface, allowing users to control the flow of information displayed in the console.
/// </summary>
public partial class ConsoleToolbar
    : FluentComponentBase
{
    /// <summary>
    /// Represents whether the console is currently paused. When true, the console will not update with new messages until it is unpaused.
    /// </summary>
    private bool _isPaused;

    /// <summary>
    /// Represents the icon used to indicate the pause state in the user interface.
    /// </summary>
    private static readonly Icon s_pauseIcon = new Size24.Pause();

    /// <summary>
    /// Represents the icon used to indicate the play (resume) state in the user interface.
    /// </summary>
    private static readonly Icon s_playIcon = new Size24.Play();

    /// <summary>
    /// Initializes a new instance of the ConsoleToolbar class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that determine the behavior and available options for the toolbar. Cannot be null.</param>
    public ConsoleToolbar(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the callback that is invoked when the paused state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> IsPausedChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the export action is triggered.
    /// </summary>
    [Parameter]
    public EventCallback OnExport { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the clear action is triggered.
    /// </summary>
    [Parameter]
    public EventCallback OnClear { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a filter action is triggered by the user.
    /// </summary>
    [Parameter]
    public EventCallback OnFilter { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the settings action is triggered by the user.
    /// </summary>
    [Parameter]
    public EventCallback OnSettings { get; set; }

    /// <summary>
    /// Gets or sets the parent console instance associated with this component.
    /// </summary>
    /// <remarks>This property is marked as a cascading parameter, allowing it to receive its value from a
    /// parent component in the hierarchy. It is essential for components that require access to the parent console's
    /// functionality or state.</remarks>
    [CascadingParameter]
    private FluentCxConsole? Parent { get; set; } = default;

    /// <summary>
    /// Asynchronously notifies subscribers when the paused state is toggled.
    /// </summary>
    private async Task OnTogglePauseAsync()
    {
        _isPaused = !_isPaused;

        if (IsPausedChanged.HasDelegate)
        {
            await IsPausedChanged.InvokeAsync(_isPaused);
        }
    }

    /// <summary>
    /// Gets the label that reflects the current state of the operation, indicating whether it can be resumed or paused.
    /// </summary>
    /// <remarks>The returned label depends on whether the operation is paused. If the operation is paused,
    /// the method returns the 'Resume' label; otherwise, it returns the 'Pause' label. If the parent object is null or
    /// does not define labels, an empty string is returned.</remarks>
    /// <returns>A string containing the appropriate label for the current state. Returns an empty string if no label is
    /// available.</returns>
    private string GetLabel()
    {
        return _isPaused ? Parent!.Labels.Resume : Parent!.Labels.Pause;
    }

    /// <summary>
    /// Gets the icon that represents the current play or pause state.
    /// </summary>
    private Icon GetIcon()
    {
        return _isPaused ? s_playIcon : s_pauseIcon;
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException("ConsoleToolbar must be used within a FluentCxConsole component.");
        }
    }
}

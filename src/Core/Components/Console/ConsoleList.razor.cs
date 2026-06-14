using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a collection of console components that can be manipulated in a fluent manner.
/// </summary>
/// <remarks>This class extends the functionality of FluentComponentBase, allowing for enhanced interaction with
/// console elements. It is designed to facilitate the creation and management of console-based user
/// interfaces.</remarks>
public partial class ConsoleList : FluentComponentBase
{
    /// <summary>
    /// Represents the icon associated with the Size24 information.
    /// </summary>
    /// <remarks>This icon is used to visually indicate Size24-related information in the user interface. Use
    /// this field when a consistent visual representation of Size24 is required.</remarks>
    private static readonly Icon s_infoIcon = new Size24.Info();

    /// <summary>
    /// Represents the data grid used to display console messages in a fluent format.
    /// </summary>
    /// <remarks>This field is intended for internal use to manage the presentation and interaction of console
    /// message data within the user interface.</remarks>
    private FluentDataGrid<ConsoleMessage>? _consoleMessageDataGrid;

    /// <summary>
    /// Indicates whether the loading state has changed since the last check.
    /// </summary>
    private bool _hasLoadingChanged;

    /// <summary>
    /// Represents the reference to the container element used for rendering the data grid.
    /// </summary>
    private ElementReference _datagridContainer;

    /// <summary>
    /// Gets or sets a value indicating whether the user is currently at the bottom of the content.
    /// </summary>
    /// <remarks>This property is used to determine the user's scroll position within the content. It can be
    /// useful for implementing features that depend on the user's view, such as lazy loading of additional
    /// content.</remarks>
    private bool _userIsAtBottom = true;

    /// <summary>
    /// Represents the JavaScript console list used for client-side interactions.
    /// </summary>
    /// <remarks>This field may be null if no console list has been initialized. Access its members only after
    /// verifying it is not null to avoid runtime exceptions.</remarks>
    private ConsoleListJS? _consoleListJS;

    /// <summary>
    /// Initializes a new instance of the ConsoleList class using the specified library configuration.
    /// </summary>
    /// <param name="configuration">The configuration settings that determine how the library operates. Cannot be null.</param>
    public ConsoleList(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the current state of the console, providing access to console-related operations and properties.
    /// </summary>
    /// <remarks>This property is injected and should not be set directly. It is essential for managing
    /// console interactions within the application.</remarks>
    [Inject]
    private IConsoleState ConsoleState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the options that configure the behavior of the console component.
    /// </summary>
    /// <remarks>This property is injected and provides access to settings that control aspects such as input
    /// handling and output formatting for the console. Modifying these options affects how the console operates within
    /// the application.</remarks>
    [Inject]
    private ConsoleOptions Options { get; set; } = null!;

    /// <summary>
    /// Gets or sets the content to display when the list contains no items.
    /// </summary>
    /// <remarks>Use this property to provide a custom message or UI fragment that is rendered when the
    /// primary content is empty. This can improve user experience by informing users that no data is available or by
    /// offering alternative actions.</remarks>
    [Parameter]
    public RenderFragment? EmptyContent { get; set; }

    /// <summary>
    /// Gets or sets the height of the element, specified in CSS units such as pixels or percentages.
    /// </summary>
    /// <remarks>The default value is "400px". This property can be used to control the vertical size of the
    /// element in the layout.</remarks>
    [Parameter]
    public string Height { get; set; } = "400px";

    /// <summary>
    /// Gets or sets a value indicating whether the component is currently loading data.
    /// </summary>
    /// <remarks>This property can be used to control the display of loading indicators in the UI. When set to
    /// <see langword="true"/>, it typically signifies that an asynchronous operation is in progress.</remarks>
    [Parameter]
    public bool Loading { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a console message is clicked.
    /// </summary>
    /// <remarks>Use this property to handle click events on console messages. The callback receives the
    /// clicked message as a parameter, allowing custom logic to be executed in response. Ensure that the handler is
    /// assigned to respond appropriately to user interactions.</remarks>
    [Parameter]
    public EventCallback<ConsoleMessage> OnMessageClick { get; set; }

    /// <summary>
    /// Retrieves the CSS color variable associated with the specified console log level.
    /// </summary>
    /// <remarks>If the provided log level does not match any predefined values, a default color variable is
    /// returned.</remarks>
    /// <param name="level">The log level for which to obtain the corresponding color. Determines the severity category used to select the
    /// color variable.</param>
    /// <returns>A string containing the CSS variable that represents the color for the specified log level.</returns>
    private static string GetLevelColor(ConsoleLevel level)
    {
        return level switch
        {
            ConsoleLevel.Trace => "var(--colorPaletteLightTealForeground2)",
            ConsoleLevel.Debug => "var(--colorBrandForeground1)",
            ConsoleLevel.Information => "var(--colorNeutralForeground3)",
            ConsoleLevel.Warning => "var(--colorStatusWarningForeground1)",
            ConsoleLevel.Error => "var(--colorStatusDangerForeground1)",
            ConsoleLevel.Critical => "var(--colorPaletteBerryForeground1)",
            _ => "var(--colorNeutralForegroundInverted)"
        };
    }

    /// <summary>
    /// Invokes the OnMessageClick event asynchronously when a console message is selected.
    /// </summary>
    /// <remarks>If there are no subscribers to the OnMessageClick event, this method does nothing.</remarks>
    /// <param name="message">The console message that was selected and will be passed to the event handler.</param>
    /// <returns>A task that represents the asynchronous operation of invoking the event handler.</returns>
    private async Task OnMessageSelectedAsync(ConsoleMessage message)
    {
        if (OnMessageClick.HasDelegate)
        {
            await OnMessageClick.InvokeAsync(message);
        }
    }

    /// <summary>
    /// Scrolls the console view to the bottom asynchronously, ensuring that the latest output is visible to the user.
    /// </summary>
    /// <remarks>This method requires that the console list JavaScript object (_consoleListJS) is initialized.
    /// If it is null, the method does nothing.</remarks>
    /// <returns>A task that represents the asynchronous scroll operation.</returns>
    private async Task ScrollToBottomAsync()
    {
        if (_consoleListJS is not null)
        {
            await _consoleListJS.ScrollToBottomAsync();
        }
    }

    /// <summary>
    /// Handles the event that occurs when the console state changes.
    /// </summary>
    /// <param name="sender">The source of the event, typically the console instance that raised the event.</param>
    /// <param name="e">An object that contains the event data related to the console state change.</param>
    /// <exception cref="NotImplementedException">Thrown when the method is called, as it is not yet implemented.</exception>
    private void ConsoleState_Changed(object? sender, ConsoleStateChangedEventArgs e)
    {
        if (_consoleListJS is not null &&
            (e.Kind == ConsoleChangeKind.BatchCompleted ||
            e.Kind == ConsoleChangeKind.MessageAdded))
        {
            InvokeAsync(() => _consoleListJS.AutoScrollIfNeededAsync(Options.AutoScroll));
            InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Updates the internal state to reflect whether the user is at the bottom of the scrollable content.
    /// </summary>
    /// <param name="atBottom">A boolean value indicating whether the user is currently at the bottom of the scrollable area. A value of <see
    /// langword="true"/> means the user is at the bottom; otherwise, <see langword="false"/>.</param>
    internal void OnScrollChanged(bool atBottom)
    {
        _userIsAtBottom = atBottom;
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _hasLoadingChanged = true;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (_hasLoadingChanged)
        {
            _hasLoadingChanged = false;
            _consoleMessageDataGrid?.SetLoadingState(Loading);
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _consoleListJS = new ConsoleListJS(_datagridContainer, this, JSModule);
            await _consoleListJS.InitializeAsync();
            await ScrollToBottomAsync();
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        ConsoleState.Changed += ConsoleState_Changed;
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        ConsoleState.Changed -= ConsoleState_Changed;

        return base.DisposeAsync();
    }
}

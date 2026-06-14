using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a fluent interface for performing console operations and formatting output in applications that utilize a
/// fluent programming style.
/// </summary>
/// <remarks>Inherits from FluentComponentBase, enabling integration with other fluent components and extending
/// functionality for streamlined console interactions.</remarks>
public partial class FluentCxConsole : FluentComponentBase
{
    /// <summary>
    /// Provides a set of options for JSON serialization with indented formatting enabled.
    /// </summary>
    /// <remarks>This instance of <see cref="JsonSerializerOptions"/> is configured to produce human-readable,
    /// indented JSON output. Use these options when readability is preferred over compactness, such as for logging or
    /// configuration files.</remarks>
    private static readonly JsonSerializerOptions s_jsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// Represents a value indicating whether the console is currently paused, which can be used to control the flow of console.
    /// </summary>
    private bool _isPaused;

    /// <summary>
    /// Gets a value indicating whether the loading process is currently in progress.
    /// </summary>
    /// <remarks>This property is typically used to determine the loading state of a resource, allowing the
    /// application to manage UI updates or user interactions accordingly.</remarks>
    private bool _isLoading;

    /// <summary>
    /// Represents the options used for exporting console data, allowing customization of the export behavior and content.
    /// </summary>
    private ConsoleExportOptions _exportOptions = new();

    /// <summary>
    /// Represents the options used for exporting console data to a file.
    /// </summary>
    /// <remarks>This field stores configuration settings such as file format and destination path for export
    /// operations. Modify these options before initiating an export to ensure the desired output.</remarks>
    private ConsoleExportFileOptions _exportFileOptions = new();

    /// <summary>
    /// Initializes a new instance of the FluentCxConsole class using the specified library configuration.
    /// </summary>
    /// <remarks>A unique identifier is generated for each console instance upon initialization.</remarks>
    /// <param name="configuration">The configuration settings that determine the behavior and available options for the console.</param>
    public FluentCxConsole(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the file downloader used for downloading files.
    /// </summary>
    /// <remarks>This property is injected and should be initialized before use. Ensure that the
    /// implementation of IFileDownloader is properly configured to handle file download operations.</remarks>
    [Inject]
    private IFileDownloader FileDownloader { get; set; } = default!;

    /// <summary>
    /// Gets or sets the console labels used for displaying information in the console.
    /// </summary>
    /// <remarks>The default value is set to ConsoleLabels.Default, which provides a standard set of labels.
    /// Custom labels can be defined by setting this property to a different ConsoleLabels value.</remarks>
    [Parameter]
    public ConsoleLabels Labels { get; set; } = ConsoleLabels.Default;

    /// <summary>
    /// Gets or sets the current state of the console, providing access to console-related operations and properties.
    /// </summary>
    /// <remarks>This property is injected and should be used to interact with the console state throughout
    /// the application. Ensure that the console state is properly initialized before use.</remarks>
    [Inject]
    private IConsoleState ConsoleState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the service responsible for exporting console data.
    /// </summary>
    /// <remarks>This property is injected and provides functionality to export data from the console. Ensure
    /// that the service is properly configured before use.</remarks>
    [Inject]
    private IConsoleExportService ConsoleExportService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the dialog service used to display modal dialogs and prompts to the user.
    /// </summary>
    /// <remarks>This property is typically injected by the dependency injection container. It provides
    /// methods for showing dialogs and handling user interactions within the application.</remarks>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the options used to configure the behavior and appearance of the console component.
    /// </summary>
    /// <remarks>This property is typically provided via dependency injection. Ensure that the options are
    /// properly configured before accessing console features that depend on them.</remarks>
    [Inject]
    private ConsoleOptions ConsoleOptions { get; set; } = default!;

    /// <summary>
    /// Handles changes to the console state and updates the component's state as needed.
    /// </summary>
    /// <remarks>If the console is paused and a new message is added, this method ignores the event to prevent
    /// unnecessary state updates.</remarks>
    /// <param name="sender">The source of the event, typically the console instance that triggered the state change.</param>
    /// <param name="e">An object that contains information about the console state change event.</param>
    private void ConsoleState_Changed(object? sender, ConsoleStateChangedEventArgs e)
    {
        if (_isPaused && e.Kind == ConsoleChangeKind.MessageAdded)
        {
            return;
        }

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sets the pause state of the operation.
    /// </summary>
    /// <param name="isPaused"><see langword="true"/> to pause the operation; <see langword="false"/> to resume it.</param>
    private void TogglePause(bool isPaused)
    {
        _isPaused = isPaused;
    }

    /// <summary>
    /// Displays a dialog that allows the user to configure export options and updates the current settings based on
    /// user input.
    /// </summary>
    /// <remarks>If the user confirms the dialog, the export options are updated to reflect the changes made.
    /// The dialog is presented as a modal drawer with a medium size and a custom header.</remarks>
    /// <returns>A task that represents the asynchronous operation of displaying the settings dialog and updating the export
    /// options.</returns>
    private async Task OnSettingsAsync()
    {
        var optionsJson = JsonSerializer.Serialize(ConsoleOptions, s_jsonOptions);
        var optionsInternal = JsonSerializer.Deserialize<ConsoleOptions>(optionsJson)!;

        var result = await DialogService.ShowDrawerAsync<ConsoleOptionsPanel>(options =>
        {
            options.Modal = true;
            options.Size = DialogSize.Small;
            options.Header.Title = "Options settings";

            options.Parameters.Add(nameof(ConsoleOptionsPanel.Options), optionsInternal);

            options.Footer.PrimaryAction.Label = "Ok";
            options.Footer.SecondaryAction.Label = "Cancel";
        });

        if (!result.Cancelled)
        {
            ConsoleOptions = optionsInternal;
        }
    }

    /// <summary>
    /// Displays a filter settings dialog and processes the user's selections for exporting data.
    /// </summary>
    /// <remarks>This method serializes the current filter state, presents a modal dialog for user input, and
    /// handles the export of data based on the user's selections. It is important to ensure that the export options are
    /// correctly set before invoking this method.</remarks>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnFilterAsync()
    {
        var filterJson = JsonSerializer.Serialize(ConsoleState.Filter, s_jsonOptions);
        var filterInternal = JsonSerializer.Deserialize<ConsoleFilter>(filterJson)!;

        var result = await DialogService.ShowDrawerAsync<ConsoleFilterPanel>(options =>
        {
            options.Modal = true;
            options.Size = DialogSize.Small;
            options.Header.Title = "Filter settings";

            options.Parameters.Add(nameof(ConsoleFilterPanel.Filter), filterInternal);

            options.Footer.PrimaryAction.Label = "Ok";
            options.Footer.SecondaryAction.Label = "Cancel";
        });

        if (!result.Cancelled)
        {
            await ConsoleState.SetFilterAsync(filterInternal);
        }
    }

    /// <summary>
    /// Handles the selection of a console message asynchronously, enabling further processing or actions based on the
    /// selected message.
    /// </summary>
    /// <remarks>This method should be called when a message is selected in the console interface. It may
    /// trigger UI updates or initiate additional actions depending on the selected message.</remarks>
    /// <param name="message">The console message that has been selected. Contains the details necessary for processing the selection. Cannot
    /// be null.</param>
    /// <returns>A task that represents the asynchronous operation. The task does not return a value.</returns>
    private async Task OnMessageSelectedAsync(ConsoleMessage message)
    {
        await DialogService.ShowDrawerAsync<ConsoleDetailsPanel>(options =>
        {
            options.Modal = true;
            options.Size = DialogSize.Medium;
            options.Header.Title = "Details";

            options.Parameters.Add(nameof(ConsoleDetailsPanel.Message), message);

            options.Footer.PrimaryAction.Label = "Close";
        });
    }

    /// <summary>
    /// Displays a dialog for configuring export options and initiates the export process based on user input.
    /// </summary>
    /// <remarks>This method serializes the current export options and file options to JSON, then deserializes
    /// them back to their respective types for use in the dialog. If the user confirms the dialog, the export options
    /// are updated and the export process is initiated.</remarks>
    /// <returns>A task representing the asynchronous operation of exporting data. This method does not return a value.</returns>
    private async Task OnExportAsync()
    {
        var optionsJson = JsonSerializer.Serialize(_exportOptions, s_jsonOptions);
        var exportOptions = JsonSerializer.Deserialize<ConsoleExportOptions>(optionsJson)!;

        var fileOptionsJson = JsonSerializer.Serialize(_exportFileOptions, s_jsonOptions);
        var exportFileOptions = JsonSerializer.Deserialize<ConsoleExportFileOptions>(fileOptionsJson)!;

        var result = await DialogService.ShowDrawerAsync<ConsoleExportOptionsPanel>(options =>
        {
            options.Modal = true;
            options.Size = DialogSize.Small;
            options.Header.Title = "Export Options";

            options.Parameters.Add(nameof(ConsoleExportOptionsPanel.Options), exportOptions);
            options.Parameters.Add(nameof(ConsoleExportOptionsPanel.FileOptions), exportFileOptions);

            options.Footer.PrimaryAction.Label = "Export";
            options.Footer.SecondaryAction.Label = "Cancel";
        });

        if (!result.Cancelled)
        {
            await SetLoadingAsync(true);

            if (!exportFileOptions.IsMultiExportSelected &&
                !string.IsNullOrEmpty(exportFileOptions.SelectedFormat))
            {
                var exportResult = await ConsoleExportService.ExportAsync(exportFileOptions.SelectedFormat, exportOptions);
                await FileDownloader.DownloadFileAsync(exportResult.FileName, exportResult.ContentType, exportResult.Content);
            }
            else if (_exportFileOptions.SelectedFormats.Count > 0)
            {
                var exportResult = await ConsoleExportService.ExportAsync(_exportFileOptions.SelectedFormats, exportOptions);
                await FileDownloader.DownloadFileAsync(exportResult.FileName, exportResult.ContentType, exportResult.Content);
            }

            _exportOptions = exportOptions;
            _exportFileOptions = exportFileOptions;

            await SetLoadingAsync(false);
        }

        Task SetLoadingAsync(bool isLoading)
        {
            _isLoading = isLoading;
            return InvokeAsync(StateHasChanged);
        }
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        ConsoleState.Changed += ConsoleState_Changed;
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        ConsoleState.Changed -= ConsoleState_Changed;
        await base.DisposeAsync();

        GC.SuppressFinalize(this);
    }
}

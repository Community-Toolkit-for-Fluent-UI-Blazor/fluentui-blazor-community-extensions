namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for exporting console messages, allowing customization of the export content and
/// filtering behavior.
/// </summary>
/// <remarks>Use this class to specify which console messages to include in an export operation, as well as which
/// message details to output. Options include filtering messages by date range, including or excluding specific message
/// fields such as timestamps, log levels, categories, properties, and exceptions, and setting a file name prefix for
/// the exported data. All properties are immutable and must be set at setialization.</remarks>
public sealed class ConsoleExportOptions
{
    /// <summary>
    /// Gets a value indicating whether only messages that meet the specified filter criteria are included in the
    /// export.
    /// </summary>
    /// <remarks>When this property is set to <see langword="true"/>, only messages matching the current
    /// filter settings are exported. This can help reduce the size of the exported data and focus on relevant
    /// information. If set to <see langword="false"/>, all messages are exported regardless of filtering.</remarks>
    public bool ExportFilteredMessagesOnly { get; set; } = true;

    /// <summary>
    /// Gets the optional starting date and time for the associated event.
    /// </summary>
    /// <remarks>This property can be null, indicating that no starting date and time has been specified. If a
    /// value is provided, it represents the earliest date and time from which the event is considered valid.</remarks>
    public DateTimeOffset? From { get; set; }

    /// <summary>
    /// Gets or sets the starting date and time for the range of data to be processed.
    /// </summary>
    /// <remarks>If this property is null, no lower bound is applied to the date and time filter. When set,
    /// only data occurring on or after the specified date and time is included.</remarks>
    internal DateTime? FromDateTime
    {
        get => From?.DateTime;
        set => From = value.HasValue ? new DateTimeOffset(value.Value) : null;
    }

    /// <summary>
    /// Gets or sets the date and time represented by the 'To' property as a nullable DateTime.
    /// </summary>
    /// <remarks>If the value is set to null, the 'To' property will also be set to null. When setting a
    /// value, it is converted to a DateTimeOffset.</remarks>
    internal DateTime? ToDateTime
    {
        get => To?.DateTime;
        set => To = value.HasValue ? new DateTimeOffset(value.Value) : null;
    }

    /// <summary>
    /// Gets the optional end date and time for the event.
    /// </summary>
    /// <remarks>This property can be null, indicating that the event does not have a defined end
    /// time.</remarks>
    public DateTimeOffset? To { get; set; }

    /// <summary>
    /// Gets the prefix to use for generated file names.
    /// </summary>
    /// <remarks>The prefix is applied to all generated file names, allowing customization of the naming
    /// convention. If not set, a default prefix is used.</remarks>
    public string? FileNamePrefix { get; set; }

    /// <summary>
    /// Gets a value indicating whether to include timestamps in the exported data.
    /// </summary>
    public bool IncludeTimestamp { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the level of detail is included in the output.
    /// </summary>
    /// <remarks>By default, this property is set to <see langword="true"/>, which means the level of detail
    /// will be included. Set this property to <see langword="false"/> to exclude the level of detail from the
    /// output.</remarks>
    public bool IncludeLevel { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether the category is included in the output.
    /// </summary>
    /// <remarks>By default, this property is set to <see langword="true"/>, which means the category will be
    /// included in the output. Set to <see langword="false"/> to exclude the category.</remarks>
    public bool IncludeCategory { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether additional properties are included in the output.
    /// </summary>
    /// <remarks>This property defaults to <see langword="true"/>, so additional properties are included
    /// unless explicitly set to <see langword="false"/>.</remarks>
    public bool IncludeProperties { get; set; } = true;

    /// <summary>
    /// Gets a value indicating whether exceptions are included in the output.
    /// </summary>
    /// <remarks>By default, this property is set to <see langword="true"/>, meaning exceptions will be
    /// included in the output. Set this property to <see langword="false"/> to exclude exceptions, which may be useful
    /// for generating cleaner logs or reports.</remarks>
    public bool IncludeExceptions { get; set; } = true;
}

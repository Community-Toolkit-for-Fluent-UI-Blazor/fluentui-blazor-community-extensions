namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a filter that determines which console logging levels are enabled for logging output.
/// </summary>
public sealed class ConsoleFilter
{
    /// <summary>
    /// Gets or sets the set of console logging levels that are enabled for this instance.
    /// </summary>
    public HashSet<ConsoleLevel> Levels { get; init; } = [.. Enum.GetValues<ConsoleLevel>()];

    /// <summary>
    /// Gets or sets the text used to filter results based on a substring match.
    /// </summary>
    public string? TextContains { get; set; }

    /// <summary>
    /// Gets or sets the category of the item, which can be used to classify or group related items.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the optional starting date and time for the range.
    /// </summary>
    public DateTimeOffset? From { get; set; }

    /// <summary>
    /// Gets or sets the optional end date and time for the event.
    /// </summary>
    public DateTimeOffset? To { get; set; }

    /// <summary>
    /// Gets or sets the date and time value representing the start date for the associated entity.
    /// </summary>
    /// <remarks>If the value is set to null, the associated entity will not have a start date. When setting a
    /// value, it is converted to a DateTimeOffset.</remarks>
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
    /// Gets or sets a value indicating whether only exception-related entries are included in the output.
    /// </summary>
    public bool IncludeExceptionsOnly { get; set; }

    /// <summary>
    /// Determines whether the specified console message satisfies all filter criteria defined by the current instance.
    /// </summary>
    /// <remarks>The filter criteria may include message level, text content, category, timestamp range, and
    /// whether the message contains an exception. Each criterion is applied in sequence, and the method returns false
    /// as soon as a mismatch is found.</remarks>
    /// <param name="message">The console message to evaluate against the filter conditions.</param>
    /// <returns>true if the message matches all specified criteria; otherwise, false.</returns>
    public bool Match(ConsoleMessage message)
    {
        if (Levels is not null && !Levels.Contains(message.Level))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(TextContains) &&
            (string.IsNullOrWhiteSpace(message.Message) ||
            !message.Message.Contains(TextContains, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(Category) &&
            (string.IsNullOrWhiteSpace(message.Category) ||
            !message.Category.Equals(Category, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        if (From.HasValue && message.Timestamp < From)
        {
            return false;
        }

        if (To.HasValue && message.Timestamp > To)
        {
            return false;
        }

        if (IncludeExceptionsOnly && message.Exception is null)
        {
            return false;
        }

        return true;
    }
}

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the data associated with a confirm email event, including the user identifier and confirmation code.
/// </summary>
/// <param name="UserId">The unique identifier of the user whose email is being confirmed. Cannot be null or empty.</param>
/// <param name="Code">The confirmation code sent to the user for email verification. Cannot be null or empty.</param>
public record ConfirmEmailEventArgs(string UserId, string Code)
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool IsSuccessful { get; set; }
}

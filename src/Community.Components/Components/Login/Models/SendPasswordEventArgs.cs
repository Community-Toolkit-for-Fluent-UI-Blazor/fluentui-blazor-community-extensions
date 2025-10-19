namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that requests a password to be sent to a specified email address.
/// </summary>
/// <param name="Email">The email address to which the password should be sent. Cannot be null or empty.</param>
public record SendPasswordEventArgs(string Email)
{
    /// <summary>
    /// Gets a value indicating whether the password send operation completed successfully.
    /// </summary>
    public bool Successful => FailReason == SendPasswordFailReason.None;

    /// <summary>
    /// Gets the reason for a failed password send operation.
    /// </summary>
    public SendPasswordFailReason FailReason { get; set; } = SendPasswordFailReason.None;
}

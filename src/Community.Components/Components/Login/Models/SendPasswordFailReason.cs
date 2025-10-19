namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the reason for a failure when attempting to send a password.
/// </summary>
public enum SendPasswordFailReason
{
    /// <summary>
    /// Represents no failure; the operation was successful.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that the specified email address was not found.
    /// </summary>
    EmailNotFound,

    /// <summary>
    /// Indicates that no response was received from the server.
    /// </summary>
    NoServerResponse,
}

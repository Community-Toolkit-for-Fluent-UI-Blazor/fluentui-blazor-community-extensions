namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for login events, including user credentials and login options.
/// </summary>
/// <param name="Email">The username entered by the user.</param>
/// <param name="Password">The password entered by the user.</param>
/// <param name="RememberMe">A value indicating whether the user has requested to be remembered on future logins.</param>
/// <param name="Lockout">A value indicating whether the account should be locked out after failed attempts.</param>
public record LoginEventArgs(string Email, string Password, bool RememberMe, bool Lockout = false)
{
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool IsSuccessful => FailReason == LoginFailReason.None;

    /// <summary>
    /// Gets or sets the reason for a failed login attempt.
    /// </summary>
    public LoginFailReason FailReason { get; set; }
}

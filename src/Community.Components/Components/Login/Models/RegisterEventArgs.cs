namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for a user registration event, including user credentials and the result of the registration attempt.
/// </summary>
/// <param name="DisplayName">The display name provided during registration.</param>
/// <param name="Email">The email address provided during registration.</param>
/// <param name="Password">The password entered by the user during registration.</param>
/// <param name="ConfirmPassword">The confirmation of the password entered by the user.</param>
public record RegisterEventArgs(string DisplayName, string Email, string Password, string ConfirmPassword)
{
    /// <summary>
    /// Gets a value indicating whether the registration operation was successful.
    /// </summary>
    public bool IsSuccessful => FailReason == RegisterFailReason.None;

    /// <summary>
    /// Gets or sets the reason for a registration failure.
    /// </summary>
    public RegisterFailReason FailReason { get; set; }
}

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for a reset password event, including the user's email address, reset code, and new password
/// information.
/// </summary>
/// <param name="Email">The email address of the user requesting the password reset. Cannot be null or empty.</param>
/// <param name="Code">The reset code sent to the user's email address. Cannot be null or empty.</param>
/// <param name="Password">The new password specified by the user. Cannot be null or empty.</param>
/// <param name="ConfirmPassword">The confirmation of the new password. Must match the value of the Password parameter.</param>
public record ResetPasswordEventArgs(string Email, string Code, string Password, string ConfirmPassword)
{
    /// <summary>
    /// Gets a value indicating whether the password reset operation completed successfully.
    /// </summary>
    public bool IsSuccessful => Errors is null || Errors.Count == 0;

    /// <summary>
    /// Gets a list of error messages associated with the password reset operation.
    /// </summary>
    public List<string> Errors { get; } = [];
}

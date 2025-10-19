namespace FluentUI.Blazor.Community.Security;

/// <summary>
/// Represents a password validation error, including a localized error message and the specific requirement that was
/// not met.
/// </summary>
/// <param name="localizedError">The localized error message describing the password validation failure. Cannot be null.</param>
/// <param name="requirement">An integer identifying the specific password requirement that was not satisfied.</param>
public readonly struct PasswordError(string localizedError, int requirement)
{
    /// <summary>
    /// Gets the localized error message associated with the current operation.
    /// </summary>
    public string LocalizedError { get; } = localizedError;

    /// <summary>
    /// Gets the numeric requirement value associated with this instance.
    /// </summary>
    public int Requirement { get; } = requirement;
}

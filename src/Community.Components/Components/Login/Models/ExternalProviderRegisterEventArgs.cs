namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for an event that occurs when a user registers using an external authentication provider.
/// </summary>
/// <param name="Email">The email address associated with the user registering through the external provider.</param>
public record ExternalProviderRegisterEventArgs(string Email)
{
    /// <summary>
    /// Gets a value indicating whether the registration operation was successful.
    /// </summary>
    public bool IsSuccessful => !RequireConfirmedAccount && Errors.Count == 0;

    /// <summary>
    /// Gets or sets a value indicating whether the user must confirm their account (e.g., via email verification)
    /// </summary>
    public bool RequireConfirmedAccount { get; set; }

    /// <summary>
    /// Gets the list of error messages associated with the current operation or object.
    /// </summary>
    /// <remarks>The list is read-only and will be empty if no errors have occurred. Each entry represents a
    /// distinct error message relevant to the context in which this property is used.</remarks>
    public List<string> Errors { get; } = [];
}

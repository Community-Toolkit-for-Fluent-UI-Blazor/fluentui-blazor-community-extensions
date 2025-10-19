namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for events related to authentication or interaction with an external identity provider.
/// </summary>
/// <remarks>This event argument type contains information about the outcome of an external provider operation,
/// such as sign-in or account linking. It includes the user's email address, the result status, and a reason for
/// failure if applicable.</remarks>
public record ExternalProviderProcessingEventArgs
{
    /// <summary>
    /// Gets or sets the email address associated with the external provider operation.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Gets a value indicating whether the operation completed successfully without any failure reason.
    /// </summary>
    public bool IsSuccessful => FailReason == ExternalProviderProcessingFailReason.None && string.IsNullOrEmpty(Email);

    /// <summary>
    /// Gets or sets the reason for failure reported by the external provider.
    /// </summary>
    public ExternalProviderProcessingFailReason FailReason { get; set; }
}

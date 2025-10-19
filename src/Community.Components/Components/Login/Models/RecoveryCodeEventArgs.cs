namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides data for events related to recovery operations, including the recovery code and the reason for any failure.
/// </summary>
/// <param name="RecoveryCode">The recovery code associated with the recovery operation. This value uniquely identifies the recovery attempt and
/// may be used for validation or auditing purposes.</param>
public record RecoveryCodeEventArgs(string RecoveryCode)
{
    /// <summary>
    /// Gets or sets the reason for a recovery operation failure.
    /// </summary>
    public RecoveryFailReason FailReason { get; set; } = RecoveryFailReason.None;

    /// <summary>
    /// Gets a value indicating whether the recovery operation completed successfully.
    /// </summary>
    public bool IsSuccessful => FailReason == RecoveryFailReason.None;
}

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the reason for a recovery operation failure.
/// </summary>
/// <remarks>Use this enumeration to determine the specific cause when a recovery process does not succeed. The
/// values indicate whether the failure was due to a lockout, an invalid recovery code, or if no failure
/// occurred.</remarks>
public enum RecoveryFailReason
{
    /// <summary>
    /// Indicates that there was no failure in the recovery operation.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that the recovery operation failed due to the user being locked out.
    /// </summary>
    LockedOut,

    /// <summary>
    /// Indicates that the recovery operation failed due to an invalid recovery code being provided.
    /// </summary>
    InvalidCode,
}

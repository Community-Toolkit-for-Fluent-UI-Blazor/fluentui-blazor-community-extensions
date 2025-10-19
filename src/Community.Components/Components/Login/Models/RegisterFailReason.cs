namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Specifies the possible reasons for a registration operation failure.
/// </summary>
public enum RegisterFailReason
{
    /// <summary>
    /// Indicates that the registration operation was successful and no failure occurred.
    /// </summary>
    None,

    /// <summary>
    /// Indicates that the provided email address is already associated with an existing account.
    /// </summary>
    EmailAlreadyInUse,

    /// <summary>
    /// Indicates that the provided displayname is already taken by another user.
    /// </summary>
    DisplayNameAlreadyInUse
}

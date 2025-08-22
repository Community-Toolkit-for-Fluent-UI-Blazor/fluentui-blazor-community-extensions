namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the available choices for the cookie.
/// </summary>
public enum CookieChoices
{
    /// <summary>
    /// The user can only accept the cookies.
    /// </summary>
    AcceptOnly,

    /// <summary>
    /// The user can accept or deny the cookies.
    /// </summary>
    AcceptDeny,

    /// <summary>
    /// The user can accept, deny or manage the cookies.
    /// </summary>
    AcceptDenyManage
}

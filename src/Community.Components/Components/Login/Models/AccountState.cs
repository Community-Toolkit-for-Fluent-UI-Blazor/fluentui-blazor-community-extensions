namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the current state of a user login operation within the authentication workflow.
/// </summary>
/// <remarks>This type is intended for internal use and is not accessible outside of the containing assembly. It
/// encapsulates information related to the progress or result of a login process.</remarks>
internal sealed class AccountState
{
    /// <summary>
    /// Gets or sets the URL to which the user is redirected after completing an operation.
    /// </summary>
    public string ReturnUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the user's login credentials should be remembered for future sessions.
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// Gets or sets the name of the provider associated with this instance.
    /// </summary>
    public string? Provider { get; set; }
}

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an external authentication provider with its unique name and display name.
/// </summary>
/// <param name="Name">The unique identifier for the external authentication provider. Cannot be null or empty.</param>
/// <param name="DisplayName">The user-friendly name to display for the external authentication provider. Cannot be null or empty.</param>
public record ExternalAuthenticationProvider(string Name, string DisplayName)
{
}

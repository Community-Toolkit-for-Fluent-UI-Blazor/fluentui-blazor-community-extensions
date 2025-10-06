using Microsoft.AspNetCore.Authentication;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides authentication-related services using the specified authentication scheme provider.
/// </summary>
/// <param name="schemeProvider">The authentication scheme provider used to manage and retrieve authentication schemes.</param>
internal sealed class ExternalProviderService(IAuthenticationSchemeProvider schemeProvider)
{
    /// <summary>
    /// Retrieves a collection of external authentication schemes available for user sign-in.
    /// </summary>
    /// <remarks>External providers are identified by having a non-empty display name. This method excludes
    /// schemes without a display name, which are typically used for internal authentication.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
    /// cref="AuthenticationScheme"/> objects representing external providers. The collection will be empty if no
    /// external providers are configured.</returns>
    public async Task<IEnumerable<AuthenticationScheme>> GetExternalProvidersAsync()
    {
        var allSchemes = await schemeProvider.GetAllSchemesAsync();

        return allSchemes.Where(s => !string.IsNullOrEmpty(s.DisplayName));
    }
}

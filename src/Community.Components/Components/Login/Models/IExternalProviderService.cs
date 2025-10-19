namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides authentication-related services using the specified authentication scheme provider.
/// </summary>
public interface IExternalProviderService
{
    /// <summary>
    /// Asynchronously retrieves a collection of available external authentication providers.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see
    /// cref="ExternalProvider"/> objects representing the available external authentication providers. The collection
    /// will be empty if no providers are available.</returns>
    ValueTask<IEnumerable<ExternalAuthenticationProvider>> GetExternalProvidersAsync();
}

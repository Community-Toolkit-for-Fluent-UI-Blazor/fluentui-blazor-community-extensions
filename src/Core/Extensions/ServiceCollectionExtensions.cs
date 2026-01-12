using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the extensions for the <see cref="IServiceCollection"/> instance.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add common services required by the FluentCx UI Web Components for Blazor library.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Library configuration</param>
    /// <returns>Returns the collection of services.</returns>
    public static IServiceCollection AddFluentCxUIComponents(this IServiceCollection services, LibraryConfiguration? configuration = null)
    {
        return services
            .AddScoped<DeviceInfoState>();
    }
    /// <summary>
    /// Add common services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Library configuration</param>
    public static IServiceCollection AddFluentCxUIComponents(this IServiceCollection services, Action<LibraryConfiguration> configuration)
    {
        LibraryConfiguration options = new();
        configuration.Invoke(options);

        return AddFluentCxUIComponents(services, options);
    }
}

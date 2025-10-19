using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Internal;
using FluentUI.Blazor.Community.Security;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Blazor.Community.Extensions;

/// <summary>
/// Represents the extensions for the <see cref="IServiceCollection"/> instance.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add common services required by the FluentCx UI Web Components for Blazor library.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <returns>Returns the collection of services.</returns>
    public static IServiceCollection AddFluentCxUIComponents(this IServiceCollection services)
    {
        return services.AddScoped(typeof(DropZoneState<>))
                       .AddScoped<FileManagerState>()
                       .AddScoped<DeviceInfoState>()
                       .AddScoped<LottieState>()
                       .AddSingleton<ObserverService>()
                       .AddScoped<AccountState>()
                       .AddSingleton<IPasswordRuleOptions, PasswordRuleOptions>();
    }
}

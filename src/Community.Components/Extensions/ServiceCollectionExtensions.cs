using FluentUI.Blazor.Community.Cache;
using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Components.Internal;
using FluentUI.Blazor.Community.Components.Services;
using FluentUI.Blazor.Community.Providers;
using FluentUI.Blazor.Community.Services;
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
                       .AddScoped<ChatState>();
    }

    /// <summary>
    /// Adds all component for using a chat.
    /// </summary>
    /// <param name="services">Collection of services.</param>
    /// <returns>Returns the collection of services.</returns>
    /// <remarks>
    /// This method registers the services required for the chat functionality,
    ///  including user cache for SignalR, SignalR Messages, cookie provider, and translation client.
    /// </remarks>
    public static IServiceCollection AddChat(this IServiceCollection services)
    {
        return services.AddSingleton<ISignalRUserCache, SignalRUserCache>()
                       .AddScoped<IMessageServiceFactory, SignalRMessageFactory>()
                       .AddScoped<ICookieProvider, CookieProvider>()
                       .AddTranslationClient();
    }

    public static IServiceCollection AddTranslationClient(this IServiceCollection services)
    {
        return services.AddScoped<ITranslationClient, TranslationClient>();
    }
}

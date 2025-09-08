using FluentUI.Blazor.Community.Components;
using FluentUI.Demo.Shared.Infrastructure;
using FluentUI.Demo.Shared.Layout;
using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Demo.Shared;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add common client services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    public static IServiceCollection AddFluentUIDemoClientServices(this IServiceCollection services)
    {
        services.AddSingleton<IAppVersionService, AppVersionService>();
        services.AddScoped<CacheStorageAccessor>();
        services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();
        services.AddSingleton<IFileManagerItemsProvider<NoFileEntryData>, FileManagerItemsProvider>();
        services.AddSingleton<DemoNavProvider>();

        return services;
    }

    /// <summary>
    /// Add common server services required by the Fluent UI Web Components for Blazor library
    /// </summary>
    /// <param name="services">Service collection</param>
    public static IServiceCollection AddFluentUIDemoServerServices(this IServiceCollection services)
    {
        services.AddScoped<IAppVersionService, AppVersionService>();
        services.AddScoped<CacheStorageAccessor>();
        services.AddHttpClient<IStaticAssetService, ServerStaticAssetService>();
        services.AddSingleton<IFileManagerItemsProvider<NoFileEntryData>, FileManagerItemsProvider>();
        services.AddSingleton<DemoNavProvider>();
        return services;
    }
}

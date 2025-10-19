using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Security;
using FluentUI.Demo.Shared.Infrastructure;
using FluentUI.Demo.Shared.Layout;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
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
        services.AddScoped<DownloadFile>();
        services.AddScoped<IExternalProviderService, ExternalProviderService>();
        services.AddScoped<LocalStorage>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IRuleLocalization, DynamicRuleLocalization>();

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
        services.AddScoped<DownloadFile>();
        services.AddScoped<LocalStorage>();
        services.AddScoped<IExternalProviderService, ExternalProviderService>();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IRuleLocalization, DynamicRuleLocalization>();

        return services;
    }
}

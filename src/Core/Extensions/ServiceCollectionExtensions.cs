using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Clipboard;
using FluentUI.Blazor.Community.Components.Infrastructure;
using FluentUI.Blazor.Community.Components.Components.FileManager.Services;
using FluentUI.Blazor.Community.Components.States;
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
        var options = configuration ?? new();

        var serviceLifetime = options?.ServiceLifetime ?? ServiceLifetime.Scoped;
        if (serviceLifetime == ServiceLifetime.Transient)
        {
            throw new NotSupportedException("Transient lifetime is not supported for Fluent UI Community services.");
        }

        return services.AddScoped<DeviceInfoState>()
                       .AddScoped<ChatState>()
                       .AddScoped<IClipboardInitializer, ClipboardInitializer>()
                       .AddScoped<IClipboard, Clipboard.Clipboard>()
                       .AddScoped<FileManagerState>()
                       .AddScoped<IFileDownloader, FileDownloader>()
                       .AddScoped(typeof(IFileEntryZipService<>), typeof(DefaultFileEntryZipService<>))
                       .AddScoped<SlideshowState>();
    }

    /// <summary>
    /// Adds the Azure translation client to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the translation client to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAzureTranslationClient(this IServiceCollection services)
    {
        return services.AddScoped<ITranslationClient, AzureTranslationClient>();
    }
}

using System.Diagnostics.CodeAnalysis;
using FluentUI.Blazor.Community.Components.Chat.Messages;
using FluentUI.Blazor.Community.Components.Clipboard;
using FluentUI.Blazor.Community.Components.Infrastructure;
using FluentUI.Blazor.Community.Components.Components.FileManager.Services;
using FluentUI.Blazor.Community.Components.States;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using FluentUI.Blazor.Community.Components.Chat.Room;
using FluentUI.Blazor.Community.Components.Chat.Engine;
using FluentUI.Blazor.Community.Components.Chat.Transport;

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
                       .AddScoped<IClipboardInitializer, ClipboardInitializer>()
                       .AddScoped<IClipboard, Clipboard.Clipboard>()
                       .AddScoped<FileManagerState>()
                       .AddScoped<IFileDownloader, FileDownloader>()
                       .AddScoped<IFileUploader, FileUploader>()
                       .AddScoped(typeof(IFileEntryZipService<>), typeof(DefaultFileEntryZipService<>))
                       .AddScoped<SlideshowState>()
                       .AddScoped<VideoState>()
                       .AddConsole();
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

    /// <summary>
    /// Adds the chat services to the service collection, which includes the chat room view state, chat message state, chat message dynamic state, chat engine, and chat state.
    /// </summary>
    /// <param name="services">The service collection to add the chat services to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddChat(this IServiceCollection services)
    {
        return services.AddScoped<ChatRoomViewState>()
                       .AddScoped<ChatMessageState>()
                       .AddScoped<ChatMessageDynamicState>()
                       .AddScoped<ChatEngine>()
                       .AddScoped<ChatState>();
    }

    /// <summary>
    /// Adds the SignalR message transport to the service collection, which enables real-time communication for chat applications.
    /// </summary>
    /// <param name="services">The service collection to add the SignalR message transport to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSignalRChat(this IServiceCollection services)
    {
        return services.AddScoped<IMessageTransport, SignalRMessageTransport>();
    }

    /// <summary>
    /// Registers console enrichers that provide additional contextual information to console logging output.
    /// </summary>
    /// <remarks>This method adds three enrichers: EnvironmentEnricher, ThreadEnricher, and ActivityEnricher.
    /// These enrichers enhance console log entries with information about the environment, the current thread, and the
    /// current activity, respectively.</remarks>
    /// <param name="services">The service collection to which the console enrichers will be added. Cannot be null.</param>
    /// <returns>The updated IServiceCollection instance, enabling further configuration of services.</returns>
    private static IServiceCollection AddConsole(this IServiceCollection services)
    {
        return services.AddSingleton<IConsoleEnricher, EnvironmentEnricher>()
                       .AddSingleton<IConsoleEnricher, ThreadEnricher>()
                       .AddSingleton<IConsoleEnricher, ActivityEnricher>()
                       .AddSingleton<ConsoleOptions>()
                       .AddSingleton<IConsoleWriter, ConsoleWriter>()
                       .AddSingleton<IConsoleState, ConsoleState>()
                       .AddSingleton<ILoggerProvider, ConsoleLoggerProvider>()
                       .AddSingleton<IConsoleSearchService, ConsoleSearchService>()
                       .AddSingleton<IConsoleExportService, ConsoleExportService>()
                       .AddSingleton<IConsoleExporter, JsonConsoleExporter>()
                       .AddSingleton<IConsoleExporter, CsvConsoleExporter>()
                       .AddSingleton<IConsoleExporter, XmlConsoleExporter>()
                       .AddSingleton<IConsoleExporter, TextConsoleExporter>()
                       .AddSingleton<IConsoleExporter, MarkdownConsoleExporter>();
    }
}

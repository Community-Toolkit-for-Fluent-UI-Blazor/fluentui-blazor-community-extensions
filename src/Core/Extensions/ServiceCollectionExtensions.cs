using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
                       .AddConsole()
                       .AddScoped<IFileDownloader, FileDownloader>();
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

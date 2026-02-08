using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to enrich console output with additional environment information.
/// </summary>
/// <remarks>This class implements the IConsoleEnricher interface, allowing for the enhancement of console output
/// by adding relevant environmental data. The actual enrichment logic is not yet implemented.</remarks>
internal sealed class EnvironmentEnricher : IConsoleEnricher
{
    /// <summary>
    /// Gets the name of the application.
    /// </summary>
    private readonly string _appName;

    /// <summary>
    /// Stores the application version as a string.
    /// </summary>
    private readonly string _appVersion;

    /// <summary>
    /// Stores the process identifier associated with the current instance.
    /// </summary>
    private readonly int _procId;

    /// <summary>
    /// Strores the name of the process, which can be configured via application settings or defaults to "process".
    /// </summary>
    private readonly string _procName;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentEnricher"/> class.
    /// </summary>
    public EnvironmentEnricher(IConfiguration configuration)
    {
        _appName = Assembly.GetEntryAssembly()?.GetName()?.Name ?? AppDomain.CurrentDomain.FriendlyName;
        _appVersion = Assembly.GetEntryAssembly()?.GetName()?.Version?.ToString() ?? "Unknown";
        _procId = Environment.ProcessId;
        _procName = configuration["Application:Name"] ?? "process";
    }

    /// <inheritdoc/>
    public void Enrich(ConsoleEnrichmentBag bag)
    {
        bag.Properties["Application.Name"] = _appName;
        bag.Properties["Application.Version"] = _appVersion;
        bag.Properties["Process.Id"] = _procId;
        bag.Properties["Process.Name"] = _procName;
        bag.Properties["Environment.MachineName"] = Environment.MachineName;
        bag.Properties["Environment.OSVersion"] = Environment.OSVersion.ToString();
        bag.Properties["Environment.ProcessorCount"] = Environment.ProcessorCount;
        bag.Properties["Environment.Framework"] = RuntimeInformation.FrameworkDescription;
        bag.Properties["Environment.OSDescription"] = RuntimeInformation.OSDescription;
    }
}

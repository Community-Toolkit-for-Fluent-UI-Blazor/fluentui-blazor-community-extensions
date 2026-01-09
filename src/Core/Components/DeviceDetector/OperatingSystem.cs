using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the operating system where the app is running.
/// </summary>
[JsonConverter(typeof(OperatingSystemConverter))]
public enum OperatingSystem
{
    /// <summary>
    /// Unknown operating system.
    /// </summary>
    Undefined,
    /// <summary>
    /// Windows operating system.
    /// </summary>
    Windows,
    /// <summary>
    /// Mac operating system.
    /// </summary>
    Mac,
    /// <summary>
    /// Linux operating system.
    /// </summary>
    Linux
}

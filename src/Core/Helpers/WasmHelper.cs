using System.Runtime.InteropServices;

namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Helper class to determine if the application is running in a WebAssembly environment.
/// </summary>
internal static class WasmHelper
{
    /// <summary>
    /// Gets a value indicating whether the application is running in a WebAssembly environment.
    /// </summary>
    internal static bool IsRunning => RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));
}

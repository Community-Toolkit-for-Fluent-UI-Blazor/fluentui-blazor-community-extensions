using System.Runtime.InteropServices;

namespace FluentUI.Blazor.Community.Helpers;

/// <summary>
/// Helper class to determine if the application is running in a WebAssembly environment.
/// </summary>
internal static class RunningWasmHelper
{
    /// <summary>
    /// Gets a value indicating whether the application is running in a WebAssembly environment.
    /// </summary>
    internal static bool IsWasm => RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));
}

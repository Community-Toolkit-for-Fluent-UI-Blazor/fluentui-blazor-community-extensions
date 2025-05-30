// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

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
    /// Windows NT operating system.
    /// </summary>
    WindowsNT,

    /// <summary>
    /// Windows XP operating system.
    /// </summary>
    WindowsXp,

    /// <summary>
    /// Windows 7 operating system.
    /// </summary>
    Windows7,

    /// <summary>
    /// Windows 8 operating system.
    /// </summary>
    Windows8,

    /// <summary>
    /// Windows 10 operating system.
    /// </summary>
    Windows10,

    /// <summary>
    /// Windows 11 operating system.
    /// </summary>
    Windows11,

    /// <summary>
    /// Windows Vista operating system.
    /// </summary>
    WindowsVista,

    /// <summary>
    /// Mac operating system.
    /// </summary>
    Mac,

    /// <summary>
    /// Linux operating system.
    /// </summary>
    Linux,

    /// <summary>
    /// Nix operating system.
    /// </summary>
    Nix
}

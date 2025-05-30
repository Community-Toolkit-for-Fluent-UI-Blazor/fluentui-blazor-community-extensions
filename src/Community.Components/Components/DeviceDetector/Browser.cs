// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the browser used to running the app.
/// </summary>
[JsonConverter(typeof(BrowserConverter))]
public enum Browser
{
    /// <summary>
    /// Undefined browser.
    /// </summary>
    Undefined,

    /// <summary>
    /// Represents Microsoft Edge browser.
    /// </summary>
    Edge,

    /// <summary>
    /// Represents Internet Explorer 9 browser.
    /// </summary>
    InternetExplorer9,

    /// <summary>
    /// Represents Internet Explorer 10 browser.
    /// </summary>
    InternetExplorer10,

    /// <summary>
    /// Represents Internet Explorer 11 browser.
    /// </summary>
    InternetExplorer11,

    /// <summary>
    /// Represents an unknown Internet Explorer browser.
    /// </summary>
    InternetExplorerUnknown,

    /// <summary>
    /// Represents Firefox browser.
    /// </summary>
    Firefox,

    /// <summary>
    /// Represents Chrome browser.
    /// </summary>
    Chrome,

    /// <summary>
    /// Represents Safari browser.
    /// </summary>
    Safari,

    /// <summary>
    /// Represents Opera browser.
    /// </summary>
    Opera
}

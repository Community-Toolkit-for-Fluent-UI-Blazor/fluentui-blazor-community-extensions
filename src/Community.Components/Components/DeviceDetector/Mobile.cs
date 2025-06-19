using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the mobile where the app is running.
/// </summary>
[JsonConverter(typeof(MobileConverter))]
public enum Mobile
{
    /// <summary>
    /// The app is running on a mobile but the app doesn't know the kind of the mobile.
    /// </summary>
    UnknownMobileDevice,

    /// <summary>
    /// The app is running on a desktop.
    /// </summary>
    NotMobileDevice,

    /// <summary>
    /// The app is running on a Windows Phone.
    /// </summary>
    WindowsPhone,

    /// <summary>
    /// The app is running on an Iphone.
    /// </summary>
    IPhone,

    /// <summary>
    /// The app is running on an Ipad.
    /// </summary>
    IPad,

    /// <summary>
    /// The app is running on an Ipod.
    /// </summary>
    IPod,

    /// <summary>
    /// The app is running on an Android.
    /// </summary>
    Android,

    /// <summary>
    /// The app is running on a BlackBerry.
    /// </summary>
    BlackBerry,

    /// <summary>
    /// The app is running on a Safari.
    /// </summary>
    Safari
}

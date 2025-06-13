// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the information of the device where the app is running.
/// </summary>
public record DeviceInfo
{
    private DeviceOrientation _deviceOrientation;

    /// <summary>
    /// Gets the user agent.
    /// </summary>
    public string? UserAgent { get; init; }

    /// <summary>
    /// Gets the used browser.
    /// </summary>
    public Browser Browser { get; init; }

    /// <summary>
    /// Gets the operating system.
    /// </summary>
    public OperatingSystem OperatingSystem { get; init; }

    /// <summary>
    /// Gets if touch is enabled.
    /// </summary>
    public bool Touch { get; init; }

    /// <summary>
    /// Gets the used mobile.
    /// </summary>
    /// <remarks>If the app is running on desktop, this value is set to <see cref="Mobile.NotMobileDevice"/></remarks>
    public Mobile Mobile { get; init; } 

    /// <summary>
    /// Gets a value indicating if the app is running on a tablet.
    /// </summary>
    public bool IsTablet { get; init; }

    /// <summary>
    /// Gets a value if the app is running on a mobile.
    /// </summary>
    public bool IsMobile => Mobile != Mobile.NotMobileDevice;

    /// <summary>
    /// 
    /// </summary>
    public DeviceOrientation Orientation
    {
        get => _deviceOrientation;
        internal set
        {
            _deviceOrientation = value;
            OrientationChanged?.Invoke(this, value);
        }
    }

    public event EventHandler<DeviceOrientation>? OrientationChanged;

    /// <inheritdoc />
    public override string ToString()
    {
        var handler = new DefaultInterpolatedStringHandler();

        handler.AppendLiteral("Operating System : ");
        handler.AppendFormatted(OperatingSystem);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("Browser : ");
        handler.AppendFormatted(Browser);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("Mobile : ");
        handler.AppendFormatted(Mobile);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("IsTablet : ");
        handler.AppendFormatted(IsTablet);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("Orientation : ");
        handler.AppendFormatted(Orientation);
        handler.AppendLiteral(Environment.NewLine);

        return handler.ToString();
    }

    /// <summary>
    /// Returns a <see cref="MarkupString"/> that represents the current object.
    /// </summary>
    /// <returns>A <see cref="MarkupString"/> that represents the current object.</returns>
    public MarkupString ToMarkup()
    {
        var handler = new DefaultInterpolatedStringHandler();

        handler.AppendLiteral("<strong>Operating System : </strong>");
        handler.AppendFormatted(OperatingSystem);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>Browser : </strong>");
        handler.AppendFormatted(Browser);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>Mobile : </strong>");
        handler.AppendFormatted(Mobile);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>IsTablet : </strong>");
        handler.AppendFormatted(IsTablet);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>Orientation : </strong>");
        handler.AppendFormatted(Orientation);
        handler.AppendLiteral("<br />");

        return new(handler.ToString());
    }
}

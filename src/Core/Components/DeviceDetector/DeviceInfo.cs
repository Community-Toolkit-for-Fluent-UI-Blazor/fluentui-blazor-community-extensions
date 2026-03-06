using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the information of the device where the app is running.
/// </summary>
public sealed record DeviceInfo
{
    /// <summary>
    /// Represents the orientation of the device.
    /// </summary>
    private DeviceOrientation _orientation;

    /// <summary>
    /// Represents the device breakpoint that determines the current responsive layout state.
    /// </summary>
    private DeviceBreakpoint _breakpoint;

    /// <summary>
    /// Occurs when the orientation of the component changes.
    /// </summary>
    public event EventHandler<DeviceOrientation>? OnOrientationChanged;

    /// <summary>
    /// Occurs when the breakpoint state changes.
    /// </summary>
    public event EventHandler<DeviceBreakpoint>? OnBreakpointChanged;

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
    /// Gets the device breakpoint that determines the current responsive layout state.
    /// </summary>
    public DeviceBreakpoint Breakpoint
    {
        get => _breakpoint;
        set
        {
            _breakpoint = value;
            OnBreakpointChanged?.Invoke(this, value);
        }
    }

    /// <summary>
    /// Gets if touch is enabled.
    /// </summary>
    public bool HasTouch { get; init; }

    /// <summary>
    /// Gets if the device is a mobile device or tablet.
    /// </summary>
    public bool IsMobile { get; init; }

    /// <summary>
    /// Gets the orientation of the device.
    /// </summary>
    public DeviceOrientation Orientation
    {
        get => _orientation;
        set
        {
            _orientation = value;
            OnOrientationChanged?.Invoke(this, _orientation);
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var handler = new DefaultInterpolatedStringHandler();

        handler.AppendLiteral("Operating System: ");
        handler.AppendFormatted(OperatingSystem);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("Browser: ");
        handler.AppendFormatted(Browser);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("IsMobile: ");
        handler.AppendFormatted(IsMobile);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("Orientation: ");
        handler.AppendFormatted(Orientation);
        handler.AppendLiteral(Environment.NewLine);

        handler.AppendLiteral("Breakpoint: ");
        handler.AppendFormatted(Breakpoint);
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

        handler.AppendLiteral("<strong>Operating System: </strong>");
        handler.AppendFormatted(OperatingSystem);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>Browser: </strong>");
        handler.AppendFormatted(Browser);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>IsMobile: </strong>");
        handler.AppendFormatted(IsMobile);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>Orientation: </strong>");
        handler.AppendFormatted(Orientation);
        handler.AppendLiteral("<br />");

        handler.AppendLiteral("<strong>Breakpoint: </strong>");
        handler.AppendFormatted(Breakpoint);
        handler.AppendLiteral("<br />");

        return new(handler.ToString());
    }
}

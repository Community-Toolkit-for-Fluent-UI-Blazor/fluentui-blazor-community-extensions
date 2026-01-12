using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentUI.Blazor.Community.Components;
using Xunit;
using OperatingSystem = FluentUI.Blazor.Community.Components.OperatingSystem;

namespace Components.Tests.Components.DeviceDetector;

public class DeviceInfoTests
{
    [Fact]
    public void DeviceInfo_IsMobile_ReturnsCorrectValue()
    {
        // Arrange & Act
        var mobileDevice = new DeviceInfo { IsMobile = true };
        var desktopDevice = new DeviceInfo { IsMobile = false };

        // Assert
        Assert.True(mobileDevice.IsMobile);
        Assert.False(desktopDevice.IsMobile);
    }

    [Fact]
    public void DeviceInfo_ToString_ReturnsFormattedString()
    {
        // Arrange
        var deviceInfo = new DeviceInfo
        {
            OperatingSystem = OperatingSystem.Windows,
            Browser = Browser.Chrome,
            IsMobile = false,
            Orientation = DeviceOrientation.Portrait
        };

        // Act
        var result = deviceInfo.ToString();

        // Assert
        Assert.Contains("Operating System: Windows", result);
        Assert.Contains("Browser: Chrome", result);
        Assert.Contains("IsMobile: False", result);
        Assert.Contains("Orientation: Portrait", result);
    }

    [Fact]
    public void DeviceInfo_ToMarkup_ReturnsFormattedMarkup()
    {
        // Arrange
        var deviceInfo = new DeviceInfo
        {
            OperatingSystem = OperatingSystem.iOS,
            Browser = Browser.Safari,
            IsMobile = true,
            Orientation = DeviceOrientation.Landscape
        };

        // Act
        var result = deviceInfo.ToMarkup();

        // Assert
        var markupString = result.ToString();
        Assert.Contains("<strong>Operating System: </strong>iOS", markupString);
        Assert.Contains("<strong>Browser: </strong>Safari", markupString);
        Assert.Contains("<strong>IsMobile: </strong>True", markupString);
        Assert.Contains("<strong>Orientation: </strong>Landscape", markupString);
        Assert.Contains("<br />", markupString);
    }

    [Fact]
    public void DeviceInfo_DefaultValues()
    {
        // Arrange & Act
        var deviceInfo = new DeviceInfo();

        // Assert
        Assert.Equal(Browser.Undefined, deviceInfo.Browser);
        Assert.Equal(OperatingSystem.Undefined, deviceInfo.OperatingSystem);
        Assert.False(deviceInfo.IsMobile);
        Assert.False(deviceInfo.HasTouch);
        Assert.Equal(DeviceOrientation.Unknown, deviceInfo.Orientation);
        Assert.Null(deviceInfo.UserAgent);
    }

    [Fact]
    public void DeviceInfo_UserAgent_Property()
    {
        // Arrange & Act
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var deviceInfo = new DeviceInfo { UserAgent = userAgent };

        // Assert
        Assert.Equal(userAgent, deviceInfo.UserAgent);
    }

    [Fact]
    public void DeviceInfo_Touch_Property()
    {
        // Arrange & Act
        var deviceInfo = new DeviceInfo { HasTouch = true };

        // Assert
        Assert.True(deviceInfo.HasTouch);
    }
}

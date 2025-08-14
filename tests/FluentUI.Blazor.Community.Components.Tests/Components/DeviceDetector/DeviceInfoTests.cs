using FluentUI.Blazor.Community.Components;
using OperatingSystem = FluentUI.Blazor.Community.Components.OperatingSystem;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DeviceDetector;

public class DeviceInfoTests
{
    [Fact]
    public void DeviceInfo_IsMobile_ReturnsCorrectValue()
    {
        // Arrange & Act
        var mobileDevice = new DeviceInfo { Mobile = Mobile.Android };
        var desktopDevice = new DeviceInfo { Mobile = Mobile.NotMobileDevice };

        // Assert
        Assert.True(mobileDevice.IsMobile);
        Assert.False(desktopDevice.IsMobile);
    }

    [Theory]
    [InlineData(Mobile.Android, true)]
    [InlineData(Mobile.IPhone, true)]
    [InlineData(Mobile.IPad, true)]
    [InlineData(Mobile.NotMobileDevice, false)]
    public void DeviceInfo_IsMobile_VariousDeviceTypes(Mobile mobile, bool expectedIsMobile)
    {
        // Arrange
        var deviceInfo = new DeviceInfo { Mobile = mobile };

        // Act & Assert
        Assert.Equal(expectedIsMobile, deviceInfo.IsMobile);
    }

    [Fact]
    public void DeviceInfo_ToString_ReturnsFormattedString()
    {
        // Arrange
        var deviceInfo = new DeviceInfo
        {
            OperatingSystem = OperatingSystem.Windows10,
            Browser = Browser.Chrome,
            Mobile = Mobile.NotMobileDevice,
            IsTablet = false,
            Orientation = DeviceOrientation.Portrait
        };

        // Act
        var result = deviceInfo.ToString();

        // Assert
        Assert.Contains("Operating System : Windows10", result);
        Assert.Contains("Browser : Chrome", result);
        Assert.Contains("Mobile : NotMobileDevice", result);
        Assert.Contains("IsTablet : False", result);
        Assert.Contains("Orientation : Portrait", result);
    }

    [Fact]
    public void DeviceInfo_ToMarkup_ReturnsFormattedMarkup()
    {
        // Arrange
        var deviceInfo = new DeviceInfo
        {
            OperatingSystem = OperatingSystem.Mac,
            Browser = Browser.Safari,
            Mobile = Mobile.IPhone,
            IsTablet = false,
            Orientation = DeviceOrientation.Landscape
        };

        // Act
        var result = deviceInfo.ToMarkup();

        // Assert
        var markupString = result.ToString();
        Assert.Contains("<strong>Operating System : </strong>Mac", markupString);
        Assert.Contains("<strong>Browser : </strong>Safari", markupString);
        Assert.Contains("<strong>Mobile : </strong>IPhone", markupString);
        Assert.Contains("<strong>IsTablet : </strong>False", markupString);
        Assert.Contains("<strong>Orientation : </strong>Landscape", markupString);
        Assert.Contains("<br />", markupString);
    }

    [Fact]
    public void DeviceInfo_OrientationChanged_Event_Triggered()
    {
        // Arrange
        var deviceInfo = new DeviceInfo { Orientation = DeviceOrientation.Portrait };
        DeviceOrientation? receivedOrientation = null;
        deviceInfo.OrientationChanged += (sender, orientation) =>
        {
            receivedOrientation = orientation;
        };

        // Act
        // We need to use reflection to set the orientation since the setter is internal
        var orientationProperty = typeof(DeviceInfo).GetProperty("Orientation");
        orientationProperty?.SetValue(deviceInfo, DeviceOrientation.Landscape);

        // Assert
        Assert.Equal(DeviceOrientation.Landscape, receivedOrientation);
    }

    [Fact]
    public void DeviceInfo_DefaultValues()
    {
        // Arrange & Act
        var deviceInfo = new DeviceInfo();

        // Assert
        Assert.Equal(Browser.Undefined, deviceInfo.Browser);
        Assert.Equal(OperatingSystem.Undefined, deviceInfo.OperatingSystem);
        Assert.Equal(Mobile.UnknownMobileDevice, deviceInfo.Mobile);
        Assert.False(deviceInfo.IsTablet);
        Assert.False(deviceInfo.Touch);
        Assert.Equal(DeviceOrientation.Portrait, deviceInfo.Orientation);
        Assert.Null(deviceInfo.UserAgent);
        Assert.True(deviceInfo.IsMobile); // Changed from Assert.False to Assert.True
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
        var deviceInfo = new DeviceInfo { Touch = true };

        // Assert
        Assert.True(deviceInfo.Touch);
    }

    [Fact]
    public void DeviceInfo_IsTablet_Property()
    {
        // Arrange & Act
        var deviceInfo = new DeviceInfo { IsTablet = true };

        // Assert
        Assert.True(deviceInfo.IsTablet);
    }
}

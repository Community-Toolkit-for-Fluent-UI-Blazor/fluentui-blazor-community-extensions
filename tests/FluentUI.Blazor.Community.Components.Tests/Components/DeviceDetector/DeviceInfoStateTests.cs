using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using OperatingSystem = FluentUI.Blazor.Community.Components.OperatingSystem;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.DeviceDetector;

public class DeviceInfoStateTests
    : TestBase
{
    [Fact]
    public void DeviceInfoState_Default()
    {
        var cut = new DeviceInfoState();

        Assert.NotNull(cut);
        Assert.Null(cut.DeviceInfo);
    }

    [Theory]
    [InlineData(["userAgent", Browser.Edge, OperatingSystem.Windows11, true, Mobile.WindowsPhone, true, DeviceOrientation.Landscape])]
    [InlineData(["unknownText", Browser.Safari, OperatingSystem.Undefined, false, Mobile.IPod, false, DeviceOrientation.Portrait])]
    [InlineData(["text_of_an_user_agent", Browser.Chrome, OperatingSystem.Windows8, true, Mobile.BlackBerry, false, DeviceOrientation.Landscape])]
    [InlineData(["an another unuseful text", Browser.Firefox, OperatingSystem.Windows7, true, Mobile.Android, true, DeviceOrientation.Portrait])]
    public void DeviceInfoState_With_Info(
        string userAgent,
        Browser browser,
        OperatingSystem operatingSystem,
        bool touch,
        Mobile mobile,
        bool isTablet,
        DeviceOrientation deviceOrientation)
    {
        var state = new DeviceInfoState()
        {
            DeviceInfo = new DeviceInfo()
            {
                Browser = browser,
                OperatingSystem = operatingSystem,
                Touch = touch,
                Mobile = mobile,
                IsTablet = isTablet,
                Orientation = deviceOrientation,
                UserAgent = userAgent
            }
        };

        Assert.NotNull(state);
        Assert.NotNull(state.DeviceInfo);
        Assert.Equal(userAgent, state.DeviceInfo.UserAgent);
        Assert.Equal(browser, state.DeviceInfo.Browser);
        Assert.Equal(operatingSystem, state.DeviceInfo.OperatingSystem);
        Assert.Equal(touch, state.DeviceInfo.Touch);
        Assert.Equal(mobile, state.DeviceInfo.Mobile);
        Assert.Equal(isTablet, state.DeviceInfo.IsTablet);
        Assert.Equal(deviceOrientation, state.DeviceInfo.Orientation);
    }

    [Fact]
    public void DeviceInfoState_DeviceInfo_Set_Null_Raises_Updated_Event()
    {
        var state = new DeviceInfoState
        {
            DeviceInfo = new DeviceInfo()
        };

        var eventCount = 0;
        DeviceInfo? eventArg = null;

        state.Updated += (sender, info) =>
        {
            eventCount++;
            eventArg = info;
        };

        state.DeviceInfo = null;

        Assert.Equal(1, eventCount);
        Assert.Null(eventArg);
    }

    [Fact]
    public void DeviceInfoState_ForceUpdate_Raises_Updated_Event()
    {
        var state = new DeviceInfoState();
        var eventCount = 0;

        state.Updated += (sender, info) => eventCount++;

        state.DeviceInfo = new DeviceInfo();
        eventCount = 0; // reset after initial set

        state.ForceUpdate();

        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void DeviceInfoState_Updated_Event_Is_Raised_On_DeviceInfo_Change()
    {
        var state = new DeviceInfoState();
        DeviceInfo? eventArg = null;
        var eventCount = 0;

        state.Updated += (sender, info) =>
        {
            eventCount++;
            eventArg = info;
        };

        var deviceInfo = new DeviceInfo
        {
            Browser = Browser.Chrome,
            OperatingSystem = OperatingSystem.Windows10,
            Touch = true,
            Mobile = Mobile.Android,
            IsTablet = false,
            Orientation = DeviceOrientation.Landscape,
            UserAgent = "test-agent"
        };

        state.DeviceInfo = deviceInfo;

        Assert.Equal(1, eventCount);
        Assert.Equal(deviceInfo, eventArg);
    }
}

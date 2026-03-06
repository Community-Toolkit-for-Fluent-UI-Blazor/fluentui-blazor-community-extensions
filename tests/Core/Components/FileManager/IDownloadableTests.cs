using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class IDownloadableTests
{
    private sealed class DownloadableItem : IDownloadable
    {
        public bool IsDownloadAllowed { get; set; }
    }

    [Fact]
    public void Interface_AllowsSettingProperty()
    {
        var item = new DownloadableItem { IsDownloadAllowed = true };

        Assert.True(item.IsDownloadAllowed);
    }
}

using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class IRenamableTests
{
    private sealed class RenamableItem : IRenamable
    {
        public bool IsRenameAllowed { get; set; }
    }

    [Fact]
    public void Interface_AllowsSettingProperty()
    {
        var item = new RenamableItem { IsRenameAllowed = true };

        Assert.True(item.IsRenameAllowed);
    }
}

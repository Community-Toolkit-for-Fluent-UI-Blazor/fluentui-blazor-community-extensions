using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class IDeletableTests
{
    private sealed class DeletableItem : IDeletable
    {
        public bool IsDeleteAllowed { get; set; }
    }

    [Fact]
    public void Interface_AllowsSettingProperty()
    {
        var item = new DeletableItem { IsDeleteAllowed = true };

        Assert.True(item.IsDeleteAllowed);
    }
}

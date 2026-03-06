using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class IMovableTests
{
    private sealed class MovableItem : IMovable
    {
        public bool IsMoveAllowed { get; set; }
    }

    [Fact]
    public void Interface_AllowsSettingProperty()
    {
        var item = new MovableItem { IsMoveAllowed = true };

        Assert.True(item.IsMoveAllowed);
    }
}

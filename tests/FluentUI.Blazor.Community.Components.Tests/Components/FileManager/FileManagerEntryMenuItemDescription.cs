using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Components.FileManager;

public class FileManagerEntryMenuItemDescriptionTests
{
    private class Dummy { }

    [Fact]
    public void Can_Construct_And_Set_Properties()
    {
        var icon = new Icon("icon", IconVariant.Regular, IconSize.Size10, "svg");
        var desc = new FileManagerEntryMenuItemDescription<Dummy>
        {
            Icon = icon,
            Label = "Test"
        };

        Assert.Equal(icon, desc.Icon);
        Assert.Equal("Test", desc.Label);
        Assert.Null(desc.OnClick);
    }

    [Fact]
    public async Task OnClick_Delegate_Is_Invoked()
    {
        var called = false;
        var desc = new FileManagerEntryMenuItemDescription<Dummy>
        {
            OnClick = async entry =>
            {
                called = true;
                await Task.CompletedTask;
            }
        };

        var entry = FileManagerEntry<Dummy>.Home;
        await desc.OnClick!(entry);
        Assert.True(called);
    }

    [Fact]
    public void Default_Properties_Are_Null()
    {
        var desc = new FileManagerEntryMenuItemDescription<Dummy>();
        Assert.Null(desc.Icon);
        Assert.Null(desc.Label);
        Assert.Null(desc.OnClick);
    }
}

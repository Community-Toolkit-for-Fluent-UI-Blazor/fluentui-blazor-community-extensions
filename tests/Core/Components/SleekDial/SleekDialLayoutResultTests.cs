using System;
using System.Collections.Generic;
using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.SleekDial;

public class SleekDialLayoutResultTests
{
    [Fact]
    public void Defaults_AreExpected()
    {
        var assembly = typeof(SleekDialItem).Assembly;
        var type = assembly.GetType("FluentUI.Blazor.Community.Components.SleekDialLayoutResult", throwOnError: true)!;
        var instance = Activator.CreateInstance(type)!;

        var layouts = type.GetProperty("Layouts")?.GetValue(instance) as List<SleekDialItemLayout>;
        var popupWidth = (int)(type.GetProperty("PopupWidth")?.GetValue(instance) ?? 0);
        var popupHeight = (int)(type.GetProperty("PopupHeight")?.GetValue(instance) ?? 0);

        Assert.NotNull(layouts);
        Assert.Empty(layouts!);
        Assert.Equal(0, popupWidth);
        Assert.Equal(0, popupHeight);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Extensions;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components.Tests.Extensions;

public class ParameterViewExtensionsTests
{
    private static ParameterView CreateParameterView<T>(string name, T value)
    {
        var dict = new Dictionary<string, object?> { [name] = value };
        return ParameterView.FromDictionary(dict);
    }

    [Fact]
    public void HasValueChangedT_ReturnsTrue_WhenValueChanged()
    {
        var parameterView = CreateParameterView("Test", 42);
        Assert.True(parameterView.HasValueChanged("Test", 21));
    }

    [Fact]
    public void HasValueChangedT_ReturnsFalse_WhenValueUnchanged()
    {
        var parameterView = CreateParameterView("Test", 42);
        Assert.False(parameterView.HasValueChanged("Test", 42));
    }

    [Fact]
    public void HasValueChangedT_ReturnsFalse_WhenParameterMissing()
    {
        var parameterView = ParameterView.FromDictionary(new Dictionary<string, object?>());
        Assert.False(parameterView.HasValueChanged("Test", 42));
    }

    [Fact]
    public void HasValueChangedT_ReturnsFalse_WhenNewValueIsNull()
    {
        var parameterView = CreateParameterView<string>("Test", null);
        Assert.False(parameterView.HasValueChanged("Test", "abc"));
    }

    [Fact]
    public void HasValueChanged_String_ReturnsTrue_WhenValueChanged()
    {
        var parameterView = CreateParameterView("Test", "abc");
        Assert.True(parameterView.HasValueChanged("Test", "def"));
    }

    [Fact]
    public void HasValueChanged_String_ReturnsFalse_WhenValueUnchanged()
    {
        var parameterView = CreateParameterView("Test", "abc");
        Assert.False(parameterView.HasValueChanged("Test", "abc"));
    }

    [Fact]
    public void HasValueChanged_String_ReturnsFalse_WhenParameterMissing()
    {
        var parameterView = ParameterView.FromDictionary(new Dictionary<string, object?>());
        Assert.False(parameterView.HasValueChanged("Test", "abc"));
    }

    [Fact]
    public void HasValueChanged_String_ReturnsTrue_WithDifferentStringComparison()
    {
        var parameterView = CreateParameterView("Test", "abc");
        Assert.True(parameterView.HasValueChanged("Test", "ABC", StringComparison.Ordinal));
        Assert.False(parameterView.HasValueChanged("Test", "ABC", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void HasValueChanged_String_ReturnsFalse_WhenNewValueIsNull()
    {
        var parameterView = CreateParameterView<string>("Test", null);
        Assert.False(parameterView.HasValueChanged("Test", "abc"));
    }
}

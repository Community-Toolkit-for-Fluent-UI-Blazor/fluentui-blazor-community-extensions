using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.FileManager;

public class AcceptFileRulesTests
{
    [Fact]
    public void AddCustom_NormalizesAndAvoidsDuplicates()
    {
        var rules = new AcceptFileRules();

        rules.AddCustom("TXT");
        rules.AddCustom(".txt");
        rules.AddCustom("  ");

        Assert.Single(rules.CustomExtensions);
        Assert.Equal(".txt", rules.CustomExtensions[0]);
    }

    [Fact]
    public void ToAcceptString_BuildsDistinctValues()
    {
        var rules = new AcceptFileRules
        {
            Categories = AcceptFileCategory.Image,
            Extensions = AcceptFileExtension.Pdf
        };

        rules.AddCustom("txt");

        var result = rules.ToAcceptString();

        Assert.Equal("image/*,.pdf,.txt", result);
    }
}

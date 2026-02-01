using FluentUI.Blazor.Community.Components;
using Xunit;

namespace Components.Tests.Components.CookieConsent;

public class CookieLabelsTests
{
    [Fact]
    public void CookieLabels_DefaultValues()
    {
        var labels = new CookieLabels();

        Assert.Equal("This site use cookies", labels.Title);
        Assert.Equal("Our site uses cookies to enhance your browsing experience, analyze traffic, and personalize the content offered. These small files stored on your device allow, for example, to remember your preferences, facilitate access to your account, or provide you with advertisements tailored to your interests.<br/><br/>By continuing to browse this site, you accept the use of these cookies. You can change your settings at any time or consult our privacy policy to learn more about their management.", labels.Description);
        Assert.Equal("Accept", labels.Accept);
        Assert.Equal("Decline", labels.Decline);
        Assert.Equal("Manage cookies", labels.ManageCookies);
        Assert.Equal("Privacy Statement", labels.PrivacyStatement);
        Assert.Equal("Third-Party Cookies", labels.ThirdPartyCookies);
        Assert.Equal("Manage cookie preference", labels.ManageCookiesTitle);
        Assert.Equal("Save changes", labels.SaveChanges);
        Assert.Equal("Cancel", labels.Cancel);
        Assert.Equal("This site uses Google Analytics, an analytics tool provided by Google, which helps us understand how visitors interact with our content.<br/><br/>Through these cookies, we collect anonymous data such as the number of visits, the pages viewed, or the traffic sources.<br/><br/>This information allows us to improve the user experience and optimize our services.", labels.GoogleAnalyticsDescription);
        Assert.Equal("Show cookie dialog", labels.ShowCookieDialogTitle);
        Assert.Equal("Hide cookie dialog", labels.HideCookieDialogTitle);
    }

    [Fact]
    public void CookieLabels_DefaultSingletonReturnsNewInstance()
    {
        var defaultLabels = CookieLabels.Default;
        var newLabels = new CookieLabels();

        Assert.NotSame(defaultLabels, newLabels);
    }

    [Fact]
    public void CookieLabels_AssignsProperties()
    {
        var labels = new CookieLabels
        {
            Title = "Title",
            Description = "Description",
            Accept = "Accept",
            Decline = "Decline",
            ManageCookies = "Manage",
            PrivacyStatement = "Privacy",
            ThirdPartyCookies = "Third",
            ManageCookiesTitle = "Manage Title",
            SaveChanges = "Save",
            Cancel = "Cancel",
            GoogleAnalyticsDescription = "GA",
            ShowCookieDialogTitle = "Show",
            HideCookieDialogTitle = "Hide"
        };

        Assert.Equal("Title", labels.Title);
        Assert.Equal("Description", labels.Description);
        Assert.Equal("Accept", labels.Accept);
        Assert.Equal("Decline", labels.Decline);
        Assert.Equal("Manage", labels.ManageCookies);
        Assert.Equal("Privacy", labels.PrivacyStatement);
        Assert.Equal("Third", labels.ThirdPartyCookies);
        Assert.Equal("Manage Title", labels.ManageCookiesTitle);
        Assert.Equal("Save", labels.SaveChanges);
        Assert.Equal("Cancel", labels.Cancel);
        Assert.Equal("GA", labels.GoogleAnalyticsDescription);
        Assert.Equal("Show", labels.ShowCookieDialogTitle);
        Assert.Equal("Hide", labels.HideCookieDialogTitle);
    }
}

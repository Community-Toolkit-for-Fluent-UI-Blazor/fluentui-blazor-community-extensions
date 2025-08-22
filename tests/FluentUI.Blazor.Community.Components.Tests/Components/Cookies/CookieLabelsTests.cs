using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Cookies;

public class CookieLabelsTests
{
    [Fact]
    public void Default_CookieLabels_Should_Have_Expected_Values()
    {
        var cookieLabels = CookieLabels.Default;

        Assert.Equal("This site use cookies", cookieLabels.Title);
        Assert.Equal("Our site uses cookies to enhance your browsing experience, analyze traffic, and personalize the content offered. These small files stored on your device allow, for example, to remember your preferences, facilitate access to your account, or provide you with advertisements tailored to your interests.", cookieLabels.PrimaryDescription);
        Assert.Equal("By continuing to browse this site, you accept the use of these cookies. You can change your settings at any time or consult our privacy policy to learn more about their management.", cookieLabels.SecondaryDescription);
        Assert.Equal("Accept", cookieLabels.Accept);
        Assert.Equal("Decline", cookieLabels.Decline);
        Assert.Equal("Manage cookies", cookieLabels.ManageCookies);
        Assert.Equal("Privacy Statement", cookieLabels.PrivacyStatement);
        Assert.Equal("Third-Party Cookies", cookieLabels.ThirdPartyCookies);
        Assert.Equal("Manage cookie preference", cookieLabels.ManageCookiesTitle);
        Assert.Equal("Save changes", cookieLabels.SaveChanges);
        Assert.Equal("Cancel", cookieLabels.Cancel);
        Assert.Equal("This site uses Google Analytics, an analytics tool provided by Google, which helps us understand how visitors interact with our content.<br/><br/>Through these cookies, we collect anonymous data such as the number of visits, the pages viewed, or the traffic sources.<br/><br/>This information allows us to improve the user experience and optimize our services.", cookieLabels.GoogleAnalyticsDescription);
    }

    [Fact]
    public void French_CookieLabels_Should_Have_Expected_Values()
    {
        var cookieLabels = CookieLabels.French;

        Assert.Equal("Ce site utilise des cookies", cookieLabels.Title);
        Assert.Equal("Notre site utilise des cookies afin d’améliorer votre expérience de navigation, analyser le trafic et personnaliser le contenu proposé. Ces petits fichiers stockés sur votre appareil permettent, par exemple, de mémoriser vos préférences, de faciliter l’accès à votre compte ou de vous proposer des publicités adaptées à vos centres d’intérêt.", cookieLabels.PrimaryDescription);
        Assert.Equal("En poursuivant votre navigation sur ce site, vous acceptez l’utilisation de ces cookies. Vous pouvez à tout moment modifier vos paramètres ou consulter notre politique de confidentialité pour en savoir plus sur leur gestion.", cookieLabels.SecondaryDescription);
        Assert.Equal("Accepter", cookieLabels.Accept);
        Assert.Equal("Refuser", cookieLabels.Decline);
        Assert.Equal("Gérer les cookies", cookieLabels.ManageCookies);
        Assert.Equal("Politique de confidentialité", cookieLabels.PrivacyStatement);
        Assert.Equal("Cookies tiers", cookieLabels.ThirdPartyCookies);
        Assert.Equal("Gérer vos préférence de cookie", cookieLabels.ManageCookiesTitle);
        Assert.Equal("Enregistrer", cookieLabels.SaveChanges);
        Assert.Equal("Annuler", cookieLabels.Cancel);
        Assert.Equal("Ce site utilise Google Analytics, un outil d’analyse fourni par Google, qui nous aide à comprendre comment les visiteurs interagissent avec notre contenu.<br/><br/>Grâce à ces cookies, nous recueillons des données anonymes telles que le nombre de visites, les pages consultées ou les sources de trafic.<br/><br/>Ces informations nous permettent d’améliorer l’expérience utilisateur et d’optimiser nos services.", cookieLabels.GoogleAnalyticsDescription);
    }
}

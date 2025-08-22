namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// 
/// </summary>
public record CookieLabels
{
    public static CookieLabels Default { get; } = new();

    public static CookieLabels French { get; } = new()
    {
        Accept = "Accepter",
        Decline = "Refuser",
        ManageCookies = "Gérer les cookies",
        PrimaryDescription = "Notre site utilise des cookies afin d’améliorer votre expérience de navigation, analyser le trafic et personnaliser le contenu proposé. Ces petits fichiers stockés sur votre appareil permettent, par exemple, de mémoriser vos préférences, de faciliter l’accès à votre compte ou de vous proposer des publicités adaptées à vos centres d’intérêt.",
        SecondaryDescription = "En poursuivant votre navigation sur ce site, vous acceptez l’utilisation de ces cookies. Vous pouvez à tout moment modifier vos paramètres ou consulter notre politique de confidentialité pour en savoir plus sur leur gestion.",
        PrivacyStatement = "Politique de confidentialité",
        ThirdPartyCookies = "Cookies tiers",
        Title = "Ce site utilise des cookies",
        Cancel = "Annuler",
        ManageCookiesTitle = "Gérer vos préférence de cookie",
        SaveChanges = "Enregistrer",
        GoogleAnalyticsDescription = "Ce site utilise Google Analytics, un outil d’analyse fourni par Google, qui nous aide à comprendre comment les visiteurs interagissent avec notre contenu.<br/><br/>Grâce à ces cookies, nous recueillons des données anonymes telles que le nombre de visites, les pages consultées ou les sources de trafic.<br/><br/>Ces informations nous permettent d’améliorer l’expérience utilisateur et d’optimiser nos services."
    };

    public string Title { get; set; } = "This site use cookies";

    public string PrimaryDescription { get; set; } = "Our site uses cookies to enhance your browsing experience, analyze traffic, and personalize the content offered. These small files stored on your device allow, for example, to remember your preferences, facilitate access to your account, or provide you with advertisements tailored to your interests.";

    public string SecondaryDescription { get; set; } = "By continuing to browse this site, you accept the use of these cookies. You can change your settings at any time or consult our privacy policy to learn more about their management.";

    public string Accept { get; set; } = "Accept";

    public string Decline { get; set; } = "Decline";

    public string ManageCookies { get; set; } = "Manage cookies";

    public string PrivacyStatement { get; set; } = "Privacy Statement";

    public string ThirdPartyCookies { get; set; } = "Third-Party Cookies";

    public string ManageCookiesTitle { get; set; } = "Manage cookie preference";

    public string SaveChanges { get; set; } = "Save changes";

    public string Cancel { get; set; } = "Cancel";

    public string GoogleAnalyticsDescription { get; set; } = "This site uses Google Analytics, an analytics tool provided by Google, which helps us understand how visitors interact with our content.<br/><br/>Through these cookies, we collect anonymous data such as the number of visits, the pages viewed, or the traffic sources.<br/><br/>This information allows us to improve the user experience and optimize our services.";
}

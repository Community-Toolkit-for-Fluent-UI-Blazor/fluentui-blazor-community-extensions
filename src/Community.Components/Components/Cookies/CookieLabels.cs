namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the labels used in a cookie consent component.
/// </summary>
public record CookieLabels
{
    /// <summary>
    /// Gets the default instance of the <see cref="CookieLabels"/> class.
    /// </summary>
    public static CookieLabels Default { get; } = new();

    /// <summary>
    /// Gets the french instance of the <see cref="CookieLabels"/> class.
    /// </summary>
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
        GoogleAnalyticsDescription = "Ce site utilise Google Analytics, un outil d’analyse fourni par Google, qui nous aide à comprendre comment les visiteurs interagissent avec notre contenu.<br/><br/>Grâce à ces cookies, nous recueillons des données anonymes telles que le nombre de visites, les pages consultées ou les sources de trafic.<br/><br/>Ces informations nous permettent d’améliorer l’expérience utilisateur et d’optimiser nos services.",
        ShowCookieDialogTitle = "Afficher la boîte de dialogue des cookies",
        HideCookieDialogTitle = "Masquer la boîte de dialogue des cookies"
    };

    /// <summary>
    /// Gets or sets the title of the cookie consent dialog.
    /// </summary>
    public string Title { get; set; } = "This site use cookies";

    /// <summary>
    /// Gets or sets the primary description of the cookie consent dialog.
    /// </summary>
    public string PrimaryDescription { get; set; } = "Our site uses cookies to enhance your browsing experience, analyze traffic, and personalize the content offered. These small files stored on your device allow, for example, to remember your preferences, facilitate access to your account, or provide you with advertisements tailored to your interests.";

    /// <summary>
    /// Gets or sets the secondary description of the cookie consent dialog.
    /// </summary>
    public string SecondaryDescription { get; set; } = "By continuing to browse this site, you accept the use of these cookies. You can change your settings at any time or consult our privacy policy to learn more about their management.";

    /// <summary>
    /// Gets or sets the label for the accept button.
    /// </summary>
    public string Accept { get; set; } = "Accept";

    /// <summary>
    /// Gets or sets the label for the decline button.
    /// </summary>
    public string Decline { get; set; } = "Decline";

    /// <summary>
    /// Gets or sets the label for the manage cookies button.
    /// </summary>
    public string ManageCookies { get; set; } = "Manage cookies";

    /// <summary>
    /// Gets or sets the label for the privacy statement link.
    /// </summary>
    public string PrivacyStatement { get; set; } = "Privacy Statement";

    /// <summary>
    /// Gets or sets the label for third-party cookies section.
    /// </summary>
    public string ThirdPartyCookies { get; set; } = "Third-Party Cookies";

    /// <summary>
    /// Gets or sets the title for the manage cookies dialog.
    /// </summary>
    public string ManageCookiesTitle { get; set; } = "Manage cookie preference";

    /// <summary>
    /// Gets or sets the label for the save changes button.
    /// </summary>
    public string SaveChanges { get; set; } = "Save changes";

    /// <summary>
    /// Gets or sets the label for the cancel button.
    /// </summary>
    public string Cancel { get; set; } = "Cancel";

    /// <summary>
    /// Gets or sets the description for Google Analytics cookies.
    /// </summary>
    public string GoogleAnalyticsDescription { get; set; } = "This site uses Google Analytics, an analytics tool provided by Google, which helps us understand how visitors interact with our content.<br/><br/>Through these cookies, we collect anonymous data such as the number of visits, the pages viewed, or the traffic sources.<br/><br/>This information allows us to improve the user experience and optimize our services.";

    /// <summary>
    /// Gets or sets the title for the button that shows the cookie dialog.
    /// </summary>
    public string ShowCookieDialogTitle { get; set; } = "Show cookie dialog";

    /// <summary>
    /// Gets or sets the title for the button that hides the cookie dialog.
    /// </summary>
    public string HideCookieDialogTitle { get; set; } = "Hide cookie dialog";
}

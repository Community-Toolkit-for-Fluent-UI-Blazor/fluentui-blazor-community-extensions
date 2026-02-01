using FluentUI.Blazor.Community.Components.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the cookie dialog.
/// </summary>
public partial class FluentCxCookie
    : FluentComponentBase
{
    /// <summary>
    /// Represents a value that show the dialog or not.
    /// </summary>
    private bool _showCookieDialog;

    /// <summary>
    /// Gets or sets a value indicating whether the open button is disabled when the manage cookies dialog is visible.
    /// </summary>
    private bool _manageCookieVisible;

    /// <summary>
    /// Represents the javascript file to load.
    /// </summary>
    private const string JavascriptFilename = FluentCxConstants.JAVASCRIPT_ROOT + "CookieConsent/FluentCxCookie.razor.js";

    /// <summary>
    /// Represents the javascript module.
    /// </summary>
    private IJSObjectReference? _jSObjectReference;

    /// <summary>
    /// Represents the google analytics item.
    /// </summary>
    internal const string GoogleAnalytics = "Google Analytics";

    /// <summary>
    /// Represents the cookies to accept or deny.
    /// </summary>
    private IEnumerable<CookiePolicyEntry>? _cookiePolicyEntries;

    /// <summary>
    /// Represents the actions buttons to render.
    /// </summary>
    private readonly RenderFragment _renderActionButtons;

    /// <summary>
    /// Gets or sets the dialog service.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the device info state.
    /// </summary>
    [Inject]
    private DeviceInfoState DeviceInfoState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the view of the cookie dialog.
    /// </summary>
    [Parameter]
    public CookieView View { get; set; }

    /// <summary>
    /// Gets or sets the consent mode to close the dialog.
    /// </summary>
    [Parameter]
    public CookieConsentMode ConsentMode { get; set; }

    /// <summary>
    /// Gets or sets the default view content.
    /// </summary>
    [Parameter]
    public RenderFragment? DefaultViewContent { get; set; }

    /// <summary>
    /// Gets or sets the small view content.
    /// </summary>
    [Parameter]
    public RenderFragment? SmallViewContent { get; set; }

    /// <summary>
    /// Gets or sets the template to render the item in the dialog.
    /// </summary>
    [Parameter]
    public RenderFragment<CookiePolicyEntry>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the cookies to accept or deny.
    /// </summary>
    [Parameter]
    public IEnumerable<CookiePolicyEntry> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the labels of the dialog.
    /// </summary>
    [Parameter]
    public CookieLabels Labels { get; set; } = CookieLabels.Default;

    /// <summary>
    /// Gets or sets the privacy statement url.
    /// </summary>
    [Parameter]
    public string? PrivacyStatementUrl { get; set; }

    /// <summary>
    /// Gets or sets the third party cookie url.
    /// </summary>
    [Parameter]
    public string? ThirdPartyCookiesUrl { get; set; }

    /// <summary>
    /// Gets or sets the width of the dialog.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the dialog.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the event callback to raise when an active cookie is selected.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnInitActiveCookie { get; set; }

    /// <summary>
    /// Gets or sets the emoji to represent the cookie.
    /// </summary>
    [Parameter]
    public Emoji CookieEmoji { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Emojis.FoodDrink.Color.Default.Cookie();

    /// <summary>
    /// Gets or sets the Google analytics id.
    /// </summary>
    [Parameter]
    public string? GoogleAnalyticsId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the dialog is modal or not.
    /// </summary>
    [Parameter]
    public bool Modal { get; set; } = true;

    /// <summary>
    /// Gets or sets the position of the open button.
    /// </summary>
    [Parameter]
    public FloatingPosition OpenButtonPosition { get; set; } = FloatingPosition.BottomLeft;

    /// <summary>
    /// Gets or sets a value indicating whether the open button is visible or not.
    /// </summary>
    /// <remarks>
    /// When this property is set to <see cref="OpenCookieVisibility.Always" />, the user can open the cookie dialog again to change his cookie preferences.
    /// When this property is set to <see cref="OpenCookieVisibility.Never" />, the user cannot open the cookie dialog again after he accepted or declined the cookies.
    /// When this property is set to <see cref="OpenCookieVisibility.WhenFirstHidden" />, the user can open the cookie dialog again only if he accepted or declined the cookies the first time.
    /// </remarks>
    [Parameter]
    public OpenCookieVisibility OpenConsentVisibility { get; set; } = OpenCookieVisibility.WhenFirstHidden;

    /// <summary>
    /// Gets or sets the icon of the close button.
    /// </summary>
    [Parameter]
    public Icon CloseButtonIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Dismiss();

    /// <summary>
    /// Gets or sets the icon of the open button.
    /// </summary>
    [Parameter]
    public Icon OpenButtonIcon { get; set; } = new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Cookies();

    /// <summary>
    /// Gets the css to use for the dialog.
    /// </summary>
    private string? InternalCss => new CssBuilder(Class)
        .AddClass("cookie-dialog", View == CookieView.Default)
        .AddClass("cookie-small", View == CookieView.Small)
        .Build();

    /// <summary>
    /// Gets the style to use for the dialog.
    /// </summary>
    private string? InternalStyle => new StyleBuilder(Style)
        .AddStyle("--cookie-width", Width, !string.IsNullOrEmpty(Width))
        .AddStyle("--cookie-height", Height, !string.IsNullOrEmpty(Height))
        .AddStyle("--cookie-width", "97%", string.IsNullOrEmpty(Width) && View == CookieView.Default && (DeviceInfoState.DeviceInfo?.IsMobile ?? false))
        .Build();

    /// <summary>
    /// Initializes all other active cookies in an asynchronous way.
    /// </summary>
    /// <param name="items">Items to initialize.</param>
    /// <returns>Returns a task which initialize all active cookies when completed.</returns>
    private async Task InitOtherCookiesAsync(IEnumerable<CookiePolicyEntry>? items)
    {
        if (items is not null &&
            items.Any() &&
            OnInitActiveCookie.HasDelegate)
        {
            foreach (var item in items)
            {
                await OnInitActiveCookie.InvokeAsync(item.Name);
            }
        }
    }

    /// <summary>
    /// Stores the cookies in the local storage in an asynchronous way.
    /// </summary>
    /// <param name="activated">Indicates if the cookie is active or not.</param>
    /// <returns>Returns a task wich stores the cookies in the local storage in an asynchronous way.</returns>
    private async Task StoreCookieAsync(bool activated)
    {
        var result = Items.ToList();

        foreach (var item in result)
        {
            item.IsActive = activated;
        }

        if (!string.IsNullOrEmpty(GoogleAnalyticsId))
        {
            result.Add(new()
            {
                IsActive = true,
                Name = GoogleAnalytics,
            });
        }

        await StoreCookieInternalAsync(result);
    }

    /// <summary>
    /// Stores the cookies in the local storage in an asynchronous way.
    /// </summary>
    /// <param name="value">Cookies to store.</param>
    /// <returns>Returns a task wich stores the cookies in the local storage in an asynchronous way.</returns>
    private async Task StoreCookieInternalAsync(IEnumerable<CookiePolicyEntry> value)
    {
        if (_jSObjectReference is not null)
        {
            await _jSObjectReference.InvokeVoidAsync("FluentUI.Blazor.Community.CookieConsent.setCookiePolicy", value);
            _cookiePolicyEntries = value;
        }
    }

    /// <summary>
    /// Initializes the Google Analytics cookie if activated.
    /// </summary>
    /// <returns>Returns a task which actives the Google Analytics cookie when completed.</returns>
    private async Task InitGoogleAnalyticsAsync()
    {
        if (_jSObjectReference is not null &&
            !string.IsNullOrEmpty(GoogleAnalyticsId) &&
            (_cookiePolicyEntries?.Any(x => x.Name == GoogleAnalytics && x.IsActive == true) ?? false))
        {
            await _jSObjectReference.InvokeVoidAsync("FluentUI.Blazor.Community.CookieConsent.initializeGoogleAnalytics", GoogleAnalyticsId);
        }
    }

    /// <summary>
    /// Deletes the cookie from the local storage in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which deletes the cookie from the local storage when completed.</returns>
    public async Task DeleteCookieAsync()
    {
        if (_jSObjectReference is not null)
        {
            await _jSObjectReference.InvokeVoidAsync("FluentUI.Blazor.Community.CookieConsent.deleteCookiePolicy");
            _showCookieDialog = true;
            _cookiePolicyEntries = null;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Occurs when the Accept button is clicked.
    /// </summary>
    /// <returns>Returns a task which active all cookies when completed.</returns>
    public async Task OnAcceptAsync()
    {
        _showCookieDialog = false;

        await StoreCookieAsync(true);
        await InitGoogleAnalyticsAsync();
        await InitOtherCookiesAsync(_cookiePolicyEntries?.Where(x => x.Name != GoogleAnalytics));
    }

    /// <summary>
    /// Occurs when the Decline button is clicked.
    /// </summary>
    /// <returns>Returns a task which deactive all cookies when completed.</returns>
    public async Task OnDeclineAsync()
    {
        _showCookieDialog = false;
        await StoreCookieAsync(false);
    }

    /// <summary>
    /// Occurs when the Manage Cookies button is clicked.
    /// </summary>
    /// <returns>Returns a task which manage the cookie dialog when completed.</returns>
    public async Task OnManageCookiesAsync()
    {
        List<CookiePolicyEntry> cookies = [];
        _manageCookieVisible = true;
        var showDialog = _showCookieDialog;
        _showCookieDialog = false;

        if (_jSObjectReference is not null)
        {
            var items = await _jSObjectReference.InvokeAsync<IEnumerable<CookiePolicyEntry>>("FluentUI.Blazor.Community.CookieConsent.getCookiePolicy");

            if (items != null)
            {
                // We store the name of the cookies and their active state,
                // so we need to find the items in the Items collection to get all information.
                foreach (var item in items)
                {
                    var existingItem = Items.FirstOrDefault(x => x.Name == item.Name);

                    if (existingItem != null)
                    {
                        existingItem.IsActive = item.IsActive;
                        cookies.Add(existingItem);
                    }
                }
            }
        }

        if (cookies.Count == 0)
        {
            if (!string.IsNullOrEmpty(GoogleAnalyticsId))
            {
                cookies.Add(CookiePolicyEntry.CreateGoogleAnalyticsCookiePolicyEntry(Labels.GoogleAnalyticsDescription));
            }

            cookies.AddRange(Items);
        }

        var dialog = await DialogService.ShowDialogAsync<ManageCookie>(
            new DialogOptions(options =>
            {
                options.Header.Title = Labels.ManageCookiesTitle;
                options.Footer.PrimaryAction.Label = Labels.SaveChanges;
                options.Footer.SecondaryAction.Label = Labels.Cancel;
                options.Parameters.Add(nameof(ManageCookie.Content), new CookieDialogContext(Items, Labels, ItemTemplate));
            }));

        if (!dialog.Cancelled &&
            dialog.Value is IEnumerable<CookiePolicyEntry> cookieItems)
        {
            _cookiePolicyEntries = cookieItems;
            _showCookieDialog = false;
            await StoreCookieInternalAsync(_cookiePolicyEntries);
            await InitGoogleAnalyticsAsync();
            await InitOtherCookiesAsync(_cookiePolicyEntries!.Where(x => x.Name != GoogleAnalytics && x.IsActive == true));
        }
        else
        {
            _showCookieDialog = showDialog;
        }

        _manageCookieVisible = false;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jSObjectReference = await JSModule.ImportJavaScriptModuleAsync(JavascriptFilename);
            _cookiePolicyEntries = await _jSObjectReference.InvokeAsync<IEnumerable<CookiePolicyEntry>>("FluentUI.Blazor.Community.CookieConsent.getCookiePolicy");
            _showCookieDialog = _cookiePolicyEntries is null;

            if (!_showCookieDialog)
            {
                if (_cookiePolicyEntries!.Any(x => x.Name == GoogleAnalytics && x.IsActive == true))
                {
                    await InitGoogleAnalyticsAsync();
                }

                await InitOtherCookiesAsync(_cookiePolicyEntries!.Where(x => x.Name != GoogleAnalytics && x.IsActive == true));
            }

            await InvokeAsync(StateHasChanged);
        }
    }
}

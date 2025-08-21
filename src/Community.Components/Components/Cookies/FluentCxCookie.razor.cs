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
    /// Represents the javascript file to load.
    /// </summary>
    private const string JavascriptFilename = "./_content/FluentUI.Blazor.Community.Components/Components/Cookies/FluentCxCookie.razor.js";

    /// <summary>
    /// Represents the javascript module.
    /// </summary>
    private IJSObjectReference? _module;

    /// <summary>
    /// Represents the google analytics item.
    /// </summary>
    internal const string GoogleAnalytics = "Google Analytics";

    /// <summary>
    /// Represents the cookies to accept or deny.
    /// </summary>
    private IEnumerable<CookieItem> _cookieState = [];

    /// <summary>
    /// Represents the actions buttons to render.
    /// </summary>
    private readonly RenderFragment _renderActionButtons;

    /// <summary>
    /// Gets or sets the javascript runtime.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the dialog service.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the view of the cookie dialog.
    /// </summary>
    [Parameter]
    public CookieView View { get; set; }

    /// <summary>
    /// Gets or sets the choices to close the dialog.
    /// </summary>
    [Parameter]
    public CookieChoices Choices { get; set; }

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
    public RenderFragment<CookieItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets the cookies to accept or deny.
    /// </summary>
    [Parameter]
    public IEnumerable<CookieItem> Items { get; set; } = [];

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
        .Build();

    /// <summary>
    /// Initializes all other active cookies in an asynchronous way.
    /// </summary>
    /// <param name="items">Items to initialize.</param>
    /// <returns>Returns a task which initialize all active cookies when completed.</returns>
    private async Task InitOtherCookiesAsync(IEnumerable<CookieItem>? items)
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
                IsActive = activated,
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
    private async Task StoreCookieInternalAsync(IEnumerable<CookieItem> value)
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("setCookiePolicy", value);
            _cookieState = value;
        }
    }

    /// <summary>
    /// Initializes the Google Analytics cookie if activated.
    /// </summary>
    /// <returns>Returns a task which actives the Google Analytics cookie when completed.</returns>
    private async Task InitGoogleAnalyticsAsync()
    {
        if (_module is not null &&
            !string.IsNullOrEmpty(GoogleAnalyticsId) &&
            (_cookieState?.Any(x => x.Name == GoogleAnalytics && x.IsActive == true) ?? false))
        {
            await _module.InvokeVoidAsync("initializeGoogleAnalytics", GoogleAnalyticsId);
        }
    }

    /// <summary>
    /// Deletes the cookie from the local storage in an asynchronous way.
    /// </summary>
    /// <returns>Returns a task which deletes the cookie from the local storage when completed.</returns>
    public async Task DeleteCookieAsync()
    {
        if (_module is not null)
        {
            await _module.InvokeVoidAsync("deleteCookiePolicy");
            _showCookieDialog = true;
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
        await InitOtherCookiesAsync(_cookieState?.Where(x => x.Name != GoogleAnalytics));
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
        List<CookieItem> cookies = [];

        if (_module is not null)
        {
            var items = await _module.InvokeAsync<IEnumerable<CookieItem>>("getCookiePolicy");

            if (items != null)
            {
                cookies.AddRange(items);
            }
        }

        if (cookies.Count == 0)
        {
            if (!string.IsNullOrEmpty(GoogleAnalyticsId))
            {
                cookies.Add(CookieItem.CreateGoogleAnalyticsCookie(Labels.GoogleAnalyticsDescription));
            }

            cookies.AddRange(Items);
        }

        var dialog = await DialogService.ShowDialogAsync<ManageCookie>(
            new CookieData(cookies, Labels, ItemTemplate),
            new DialogParameters()
            {
                Title = Labels.ManageCookiesTitle,
                PrimaryAction = Labels.SaveChanges,
                SecondaryAction = Labels.Cancel,
                ShowDismiss = true,
                PreventDismissOnOverlayClick = true,
                PreventScroll = true,
            });

        var result = await dialog.Result;

        if (!result.Cancelled &&
            result.Data is IEnumerable<CookieItem> cookieItems)
        {
            _cookieState = cookieItems;
            _showCookieDialog = false;
            await StoreCookieInternalAsync(_cookieState);
            await InitGoogleAnalyticsAsync();
            await InitOtherCookiesAsync(_cookieState!.Where(x => x.Name != GoogleAnalytics && x.IsActive == true));
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _module = await Runtime.InvokeAsync<IJSObjectReference>("import", JavascriptFilename);
            _cookieState = await _module.InvokeAsync<IEnumerable<CookieItem>>("getCookiePolicy");
            _showCookieDialog = _cookieState is null;

            if (!_showCookieDialog)
            {
                if (_cookieState!.Any(x => x.Name == GoogleAnalytics && x.IsActive == true))
                {
                    await InitGoogleAnalyticsAsync();
                }

                await InitOtherCookiesAsync(_cookieState!.Where(x => x.Name != GoogleAnalytics && x.IsActive == true));
            }

            await InvokeAsync(StateHasChanged);
        }
    }
}

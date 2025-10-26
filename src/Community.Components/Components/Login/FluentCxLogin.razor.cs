using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that provides user login functionality with customizable labels and responsive layout
/// options.
/// </summary>
public partial class FluentCxLogin
{
    /// <summary>
    /// Represents the render fragment for the image panel.
    /// </summary>
    private readonly RenderFragment _renderImagePanelFragment;

    /// <summary>
    /// Represents the animation settings for the component.
    /// </summary>
    private FluentCxAnimation? _animation;

    /// <summary>
    /// Value indicating whether the component is being viewed on a mobile device.
    /// </summary>
    private bool _isMobile;

    /// <summary>
    /// Represents the current source of the image being displayed.
    /// </summary>
    private string? _source;

    /// <summary>
    /// Represents a collection of ImageView instances, each associated with a specific login view.
    /// </summary>
    private readonly Dictionary<AccountManagerView, ImageView> _imageViews = new(EqualityComparer<AccountManagerView>.Default);

    /// <summary>
    /// Reference to the internal login component instance.
    /// </summary>
    private LoginBaseComponent? _loginComponent;

    /// <summary>
    /// Gets or sets the labels used in the login component for various UI elements.
    /// </summary>
    [Parameter]
    public AccountLabels Labels { get; set; } = AccountLabels.Default;

    /// <summary>
    /// Gets or sets the position of the login panel within the component.
    /// </summary>
    /// <remarks>The default value is <see cref="LoginPanelPosition.Right"/>. Set this property to control
    /// where the login panel appears, such as on the left or right side of the layout.</remarks>
    [Parameter]
    public LoginPanelPosition PanelPosition { get; set; } = LoginPanelPosition.Right;

    /// <summary>
    /// Gets or sets the width, in pixels, of the component.
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 1200;

    /// <summary>
    /// Gets or sets the height, in pixels, of the component.
    /// </summary>
    [Parameter]
    public int Height { get; set; } = 800;

    /// <summary>
    /// Gets or sets the CSS media query width value that determines when the component should be considered in a mobile
    /// layout.
    /// </summary>
    /// <remarks>Set this property to specify the maximum viewport width (such as "600px" or "40em") at which
    /// the component switches to its mobile presentation. The value should be a valid CSS width expression.
    /// you must write the media query like "(max-width: 600px)".
    /// </remarks>
    [Parameter]
    public string? QueryMobileWidth { get; set; }

    /// <summary>
    /// Gets or sets the layout options for the login component, allowing customization of its appearance and behavior.
    /// </summary>
    [Parameter]
    public LoginLayoutOptions LayoutOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the content to be rendered within the login component, allowing for customization of the login form
    /// </summary>
    [Parameter]
    public RenderFragment? LoginContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered within the registration view of the component, allowing for customization.
    /// </summary>
    [Parameter]
    public RenderFragment? RegisterContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the register confirmation area.
    /// </summary>
    [Parameter]
    public RenderFragment? RegisterConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display for email confirmation within the component.
    /// </summary>
    [Parameter]
    public RenderFragment? EmailConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether external authentication providers are enabled.
    /// </summary>
    [Parameter]
    public bool UseExternalProviders { get; set; } = true;

    /// <summary>
    /// Gets or sets the source identifier or URI for the image.
    /// </summary>
    /// <remarks>
    /// This property is only used when the component is in desktop mode (i.e., not on a mobile device).
    /// </remarks>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the alternative text description for the image, enhancing accessibility.
    /// </summary>
    /// <remarks>
    /// This property is only used when the component is in desktop mode (i.e., not on a mobile device).
    /// </remarks>
    [Parameter]
    public string? AltText { get; set; }

    /// <summary>
    /// Gets or sets the title for the image.
    /// </summary>
    /// <remarks>
    /// This property is only used when the component is in desktop mode (i.e., not on a mobile device).
    /// </remarks>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered in the overlay section of the component, allowing for additional
    /// </summary>
    [Parameter]
    public RenderFragment? OverlayContent { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the login view changes.
    /// </summary>
    [Parameter]
    public EventCallback<AccountManagerView> OnViewChanged { get; set; }

    /// <summary>
    /// Gets or sets the custom content to display as the image within the component.
    /// </summary>
    /// <remarks>Use this property to provide a custom image or image-related markup using a <see
    /// cref="RenderFragment"/>. If not set, no image content will be rendered.</remarks>
    [Parameter]
    public RenderFragment? ImageViews { get; set; }

    /// <summary>
    /// Gets or sets the content to display when the account is disabled.
    /// </summary>
    [Parameter]
    public RenderFragment? AccountDisabledContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when the account is locked.
    /// </summary>
    [Parameter]
    public RenderFragment? AccountLockedContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display for external authentication providers.
    /// </summary>
    [Parameter]
    public RenderFragment? ExternalProviderContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the forgot password confirmation area.
    /// </summary>
    [Parameter]
    public RenderFragment? ForgotPasswordConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the forgot password view.
    /// </summary>
    [Parameter]
    public RenderFragment? ForgotPasswordContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when invalid credentials are provided.
    /// </summary>
    [Parameter]
    public RenderFragment? InvalidCredentialsContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the reset password section.
    /// </summary>
    [Parameter]
    public RenderFragment? ResetPasswordContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the reset password confirmation view.
    /// </summary>
    [Parameter]
    public RenderFragment? ResetPasswordConfirmationContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when an invalid password reset token is encountered.
    /// </summary>
    [Parameter]
    public RenderFragment? InvalidPasswordResetContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the recovery code section.
    /// </summary>
    [Parameter]
    public RenderFragment? RecoveryCodeContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display in the two-factor authentication required section.
    /// </summary>
    [Parameter]
    public RenderFragment? RequiredTwoFactorContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when an unknown error occurs.
    /// </summary>
    [Parameter]
    public RenderFragment? UnknownErrorContent { get; set; }

    /// <summary>
    /// Gets or sets the content to display when a user's account is not confirmed.
    /// </summary>
    [Parameter]
    public RenderFragment? UserNotConfirmedContent { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a login event occurs.
    /// </summary>
    /// <remarks>Use this property to handle login events, such as when a user successfully signs in. The
    /// callback receives a <see cref="LoginEventArgs"/> instance containing details about the login event.</remarks>
    [Parameter]
    public EventCallback<LoginEventArgs> OnLogin { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a user attempts to register using an external authentication
    /// provider.
    /// </summary>
    [Parameter]
    public EventCallback<ExternalProviderRegisterEventArgs> OnExternalProviderRegister { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when external provider processing occurs.
    /// </summary>
    [Parameter]
    public EventCallback<ExternalProviderProcessingEventArgs> OnExternalProviderProcessing { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a recovery code event occurs.
    /// </summary>
    [Parameter]
    public EventCallback<RecoveryCodeEventArgs> OnRecoveryCode { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when instructions to send a password are triggered.
    /// </summary>
    [Parameter]
    public EventCallback<SendPasswordEventArgs> OnSendInstructions { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the user requests to resend a verification email.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnResendEmail { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a registration event occurs.
    /// </summary>
    [Parameter]
    public EventCallback<RegisterEventArgs> OnSignUp { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a reset password event occurs.
    /// </summary>
    [Parameter]
    public EventCallback<ResetPasswordEventArgs> OnResetPassword { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display additional text for external authentication providers.
    /// </summary>
    [Parameter]
    public bool ShowExternalProviderText { get; set; }

    /// <summary>
    /// Gets or sets a delegate that provides a custom icon for a given icon name.
    /// </summary>
    [Parameter]
    public Func<string, Icon>? ExternalIconProvider { get; set; }

    /// <summary>
    /// Gets or sets the view model containing data and settings for the account manager UI.
    /// </summary>
    [SupplyParameterFromQuery(Name = "view")]
    public string? ViewRaw { get; set; }

    /// <summary>
    /// Gets or sets the return URL to redirect to after a successful login.
    /// </summary>
    [SupplyParameterFromQuery]
    private string? ReturnUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user's login session should be persisted across browser restarts.
    /// </summary>
    [SupplyParameterFromQuery]
    private bool RememberMe { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the lockout is enabled for the component.
    /// </summary>
    [Parameter]
    public bool Lockout { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is being viewed on a mobile device.
    /// </summary>
    [Parameter]
    public bool IsMobile { get; set; }

    /// <summary>
    /// Gets a value indicating whether the component is being viewed on a mobile device,
    ///  considering both internal and external indicators.
    /// </summary>
    private bool IsInternalMobile => _isMobile || IsMobile;

    /// <summary>
    /// Gets or sets the current login state for the component.
    /// </summary>
    [Inject]
    private AccountState State { get; set; } = default!;

    /// <summary>
    /// Gets the current view state of the account manager, reflecting the login component's view if available.
    /// </summary>
    /// <remarks>If the login component is not initialized, the property returns the default login view. This
    /// property is intended for internal use and may change as the account manager's state evolves.</remarks>
    internal AccountManagerView View => _loginComponent?.View ?? AccountManagerView.Login;

    /// <summary>
    /// Sets the current login view to the specified view.
    /// </summary>
    /// <param name="view">The login view to display. Cannot be null.</param>
    public async Task SetViewAsync(AccountManagerView view)
    {
        if (_loginComponent is not null)
        {
            await _loginComponent.SetViewAsync(view);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        State.RememberMe = RememberMe;
        State.ReturnUrl = ReturnUrl ?? string.Empty;
    }

    /// <summary>
    /// Adds the specified ImageView to the collection, associating it with its corresponding view.
    /// </summary>
    /// <param name="imageView">The ImageView instance to add. Must not reference a view that is already associated with another ImageView in
    /// the collection.</param>
    /// <exception cref="InvalidOperationException">Thrown if an ImageView for the specified view has already been added.</exception>
    internal void Add(ImageView imageView)
    {
        if (_imageViews.ContainsKey(imageView.View))
        {
            throw new InvalidOperationException($"An ImageView for the view '{imageView.View}' has already been added. Only one ImageView per LoginView is allowed.");
        }

        _imageViews[imageView.View] = imageView;
    }

    /// <summary>
    /// Removes the specified image view from the collection.
    /// </summary>
    /// <param name="imageView">The image view to remove from the collection. Cannot be null.</param>
    internal void Remove(ImageView imageView)
    {
        _imageViews.Remove(imageView.View);
    }

    /// <summary>
    /// Updates the image source with the current state of the specified login view.
    /// </summary>
    /// <param name="view">The login view whose state is used to update the data source. Cannot be null.</param>
    private async Task UpdateSourceAsync(AccountManagerView view)
    {
        if (_imageViews.TryGetValue(view, out var imageView))
        {
            _source = imageView.Source;
        }
        else
        {
            _source = Source;
        }

        if (OnViewChanged.HasDelegate)
        {
            await OnViewChanged.InvokeAsync(view);
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender && _loginComponent is not null)
        {
            var view = string.IsNullOrWhiteSpace(ViewRaw)
                ? AccountManagerView.Login
                : Enum.Parse<AccountManagerView>(ViewRaw, true);

            await SetViewAsync(view);
        }
    }

    /// <summary>
    /// Asynchronously restarts the animation if one is currently associated with the instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous restart operation. The task completes when the animation has been
    /// restarted.</returns>
    public async Task RestartAsync()
    {
        if (_animation is not null)
        {
            await _animation.RestartAsync();
        }
    }
}

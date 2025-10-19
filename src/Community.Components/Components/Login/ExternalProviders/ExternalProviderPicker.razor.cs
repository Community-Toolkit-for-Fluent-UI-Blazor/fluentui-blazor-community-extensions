using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a Blazor component that displays and manages external authentication provider options for user sign-in or
/// sign-up.
/// </summary>
/// <remarks>Use this component to present external login buttons (such as Google or Facebook) within a login or
/// registration form. The component supports customization of provider icons, button text, and UI labels, and
/// integrates with navigation and authentication services to initiate external authentication flows. It is typically
/// used in scenarios where users can authenticate using third-party providers.</remarks>
public partial class ExternalProviderPicker
{
    /// <summary>
    /// Provides a culture-independent, invariant culture for formatting and parsing operations.
    /// </summary>
    /// <remarks>Use this field when culture-specific formatting is not required, ensuring consistent results
    /// regardless of the user's locale.</remarks>
    private static readonly CultureInfo s_culture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Represents the collection of external authentication providers available for user sign-in.
    /// </summary>
    private IEnumerable<ExternalAuthenticationProvider>? _providers;

    /// <summary>
    /// Represents a render fragment that generates the text content for each external authentication provider button.
    /// </summary>
    private readonly RenderFragment<string> _renderTextContent;

    /// <summary>
    /// Gets or sets a delegate that retrieves an <see cref="Icon"/> instance for a given external icon name.
    /// </summary>
    /// <remarks>The delegate should accept a string representing the external icon name and return the
    /// corresponding <see cref="Icon"/>. If no icon is found, the delegate may return <c>null</c>. This property allows
    /// customization of icon sourcing from external providers or resources.</remarks>
    [Parameter]
    public Func<string, Icon>? IconFromExternal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to display the text associated with each external authentication provider.
    /// </summary>
    [Parameter]
    public bool ShowText { get; set; }

    /// <summary>
    /// Gets or sets the set of labels used for the login form UI elements.
    /// </summary>
    /// <remarks>Use this property to customize the text displayed for fields, buttons, and messages in the
    /// login form. If not set, default labels are used. This property is typically set via component parameters in
    /// Blazor applications.</remarks>
    [Parameter]
    public AccountLabels Labels { get; set; } = AccountLabels.Default;

    /// <summary>
    /// Gets or sets a value indicating whether the component is being used for sign-in (false) or sign-up (true).
    /// </summary>
    [Parameter]
    public bool SignUp { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when an external authentication provider is selected.
    /// </summary>
    [Parameter]
    public EventCallback OnExternalProviderSelected { get; set; }

    /// <summary>
    /// Gets or sets the service used to interact with external providers.
    /// </summary>
    [Inject]
    private IExternalProviderService ExternalProviderService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the current login state for the user session.
    /// </summary>
    [Inject]
    private AccountState State { get; set; } = default!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _providers = await ExternalProviderService.GetExternalProvidersAsync();
    }

    /// <summary>
    /// Navigates to the external login page for the specified authentication provider.
    /// </summary>
    /// <remarks>This method initiates a navigation to the external login flow, typically used to authenticate
    /// users via third-party providers such as Google or Facebook. The navigation is performed with a full page reload
    /// to ensure the external authentication process starts correctly.</remarks>
    /// <param name="provider">The name of the external authentication provider to use for login. Cannot be null or empty.</param>
    private async Task OnProviderSelectedAsync(string provider)
    {
        State.Provider = provider;

        if (OnExternalProviderSelected.HasDelegate)
        {
            await OnExternalProviderSelected.InvokeAsync();
        }
    }

    /// <summary>
    /// Retrieves an icon associated with the specified provider name using an external icon provider, if available.
    /// </summary>
    /// <remarks>If no external icon provider is set or the provider does not supply an icon for the given
    /// name, the method returns <see langword="null"/>.</remarks>
    /// <param name="providerName">The name of the provider for which to retrieve the icon. Cannot be null.</param>
    /// <returns>An <see cref="Icon"/> instance representing the icon for the specified provider if found; otherwise, <see
    /// langword="null"/>.</returns>
    private Icon? GetIconFrom(string providerName)
    {
        if (IconFromExternal is not null)
        {
            var icon = IconFromExternal(providerName);

            if (icon is not null)
            {
                return icon;
            }
        }

        return null;
    }

    /// <summary>
    /// Generates the localized text content for a provider button based on the current sign-up or connect state.
    /// </summary>
    /// <remarks>The returned text varies depending on whether the control is in sign-up mode or connect mode.
    /// The format and language are determined by the current culture and resource labels.</remarks>
    /// <param name="providerName">The display name of the authentication provider to include in the generated text. Cannot be null.</param>
    /// <returns>A localized string containing the appropriate button text for the specified provider. Returns null if the
    /// provider name is null.</returns>
    private string? GetTextContent(string providerName)
    {
        return SignUp ? string.Format(s_culture, Labels.SignUpWithProvider, providerName) :
                        string.Format(s_culture, Labels.ConnectWithProvider, providerName);
    }
}

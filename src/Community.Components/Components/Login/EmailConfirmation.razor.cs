using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the confirmation component displayed after a user confirms their email address.
/// </summary>
public partial class EmailConfirmation
{
    /// <summary>
    /// Value indicating whether the component is currently processing the email confirmation.
    /// </summary>
    private bool _isProcessing = true;

    /// <summary>
    /// Value indicating whether the email confirmation was successful.
    /// </summary>
    private bool _isSuccessful;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component.
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the user confirms their email address.
    /// </summary>
    /// <remarks>Use this event to handle actions that should occur after a user successfully confirms their
    /// email, such as updating user status or displaying a confirmation message. The event provides details about the
    /// confirmation through the <see cref="ConfirmEmailEventArgs"/> parameter.</remarks>
    [Parameter]
    public EventCallback<ConfirmEmailEventArgs> OnConfirmEmail { get; set; }

    /// <summary>
    /// Gets or sets the user identifier provided in the query string.
    /// </summary>
    [SupplyParameterFromQuery]
    private string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the code value supplied from the query string.
    /// </summary>
    [SupplyParameterFromQuery]
    private string? Code { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (string.IsNullOrWhiteSpace(UserId) || string.IsNullOrWhiteSpace(Code))
        {
            throw new InvalidOperationException("UserId and Code must be provided in the query string.");
        }

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(EmailConfirmation)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            var args = new ConfirmEmailEventArgs(UserId!, Code!);

            if (OnConfirmEmail.HasDelegate)
            {
                await OnConfirmEmail.InvokeAsync(args);
            }

            _isSuccessful = args.IsSuccessful;
            _isProcessing = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Handles the login event asynchronously by updating the parent view to the login screen, if a parent is
    /// available.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoginAsync()
    {
        if (Parent is not null)
        {
            await Parent.SetViewAsync(AccountManagerView.Login);
        }
    }
}

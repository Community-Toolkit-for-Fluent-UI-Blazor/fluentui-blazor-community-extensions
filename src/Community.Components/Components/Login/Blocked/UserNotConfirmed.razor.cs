using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a component that displays an user not confirmed message and provides options for user login.
/// </summary>
/// <remarks>Use this component to inform users that their account is locked and to offer a way to initiate the
/// login process. The component can be customized with specific labels and a callback for login actions.</remarks>
public partial class UserNotConfirmed
{
    /// <summary>
    /// Gets or sets the parent <see cref="FluentCxLogin"/> component.  
    /// </summary>
    [CascadingParameter]
    private FluentCxLogin? Parent { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (Parent is null)
        {
            throw new InvalidOperationException($"{nameof(UserNotConfirmed)} must be used within a {nameof(FluentCxLogin)} component.");
        }
    }

    /// <summary>
    /// Asynchronously transitions the parent view to the login screen.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnLoginAsync()
    {
        await Parent!.SetViewAsync(AccountManagerView.Login);
    }
}

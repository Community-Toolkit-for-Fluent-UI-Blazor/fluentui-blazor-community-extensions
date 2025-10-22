using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentUI.Blazor.Community.Components;
using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.Login;

public partial class LoginContainer : ObserverItem
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

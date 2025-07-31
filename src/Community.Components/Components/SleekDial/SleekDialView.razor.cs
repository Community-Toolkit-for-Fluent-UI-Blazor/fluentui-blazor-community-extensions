using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class SleekDialView
    : FluentComponentBase
{
    [CascadingParameter]
    private FluentCxSleekDial Parent { get; set; } = default!;
}

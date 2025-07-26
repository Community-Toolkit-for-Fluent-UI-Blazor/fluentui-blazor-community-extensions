using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public class SleekDialRadialSettings
{
    public int StartAngle { get; set; } = -1;

    public int EndAngle { get; set; } = -1;

    public string Offset { get; set; } = "100px";

    public SleekDialRadialDirection Direction { get; set; } = SleekDialRadialDirection.Default;
}

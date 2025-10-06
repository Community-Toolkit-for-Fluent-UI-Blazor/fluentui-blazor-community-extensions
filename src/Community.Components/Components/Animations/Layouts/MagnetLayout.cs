using Microsoft.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a layout strategy that positions elements towards a specified point with some randomness, creating a "magnet" effect.
/// </summary>
public sealed class MagnetLayout
    : AnimatedLayoutBase
{
    /// <summary>
    /// Gets or sets the target X coordinate towards which elements will be attracted.
    /// </summary>
    [Parameter]
    public double MagnetX { get; set; }

    /// <summary>
    /// Gets or sets the target Y coordinate towards which elements will be attracted.
    /// </summary>
    [Parameter]
    public double MagnetY { get; set; }

    /// <inheritdoc />
    protected override void Update(int index, int count, AnimatedElement animatedElement)
    {
        animatedElement.OffsetXState = CreateState(MagnetX + Random.Shared.NextDouble() * 50 - 25);
        animatedElement.OffsetYState = CreateState(MagnetY + Random.Shared.NextDouble() * 50 - 25);
    }
}

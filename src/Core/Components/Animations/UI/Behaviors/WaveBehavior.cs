using FluentUI.Blazor.Community.Components.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a motion behavior that animates a property of a motion item using a sinusoidal wave pattern along a
/// specified axis.
/// </summary>
/// <remarks>Use this behavior to create oscillating or wave-like animations for UI elements. The amplitude,
/// frequency, and phase offset parameters allow customization of the wave's shape and timing. The axis parameter
/// determines which property of the motion item is affected (X, Y, or rotation).</remarks>
public sealed class WaveBehavior : MotionBehaviorBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WaveBehavior"/> class with the specified library configuration.
    /// </summary>
    /// <param name="configuration">The library configuration to be used by this component.</param>
    public WaveBehavior(LibraryConfiguration configuration)
        : base(configuration)
    { }

    /// <summary>
    /// Gets or sets the axis along which the wave animation is applied.
    /// </summary>
    [Parameter]
    public WaveAxis Axis { get; set; } = WaveAxis.Y;

    /// <summary>
    /// Gets or sets the amplitude value.
    /// </summary>
    [Parameter]
    public double Amplitude { get; set; } = 20;

    /// <summary>
    /// Gets or sets the frequency value, which controls how many oscillations occur per second.
    /// </summary>
    [Parameter]
    public double Frequency { get; set; } = 2;

    /// <summary>
    /// Gets or sets the phase offset applied to the animation.
    /// </summary>
    [Parameter]
    public double PhaseOffset { get; set; } = 0.3;

    /// <inheritdoc />
    protected internal override void Apply(MotionItem item, int index, TimeSpan elapsed)
    {
        var t = elapsed.TotalSeconds * Frequency + index * PhaseOffset;
        var wave = Math.Sin(t) * Amplitude;

        switch (Axis)
        {
            case WaveAxis.X:
                item.State.X = wave;
                break;

            case WaveAxis.Y:
                item.State.Y = wave;
                break;

            case WaveAxis.Rotation:
                item.State.Rotation = wave;
                break;
        }
    }
}

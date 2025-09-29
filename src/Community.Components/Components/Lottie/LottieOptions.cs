namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents configuration options for a Lottie animation.
/// </summary>
/// <remarks>This record is used to configure the behavior and rendering of a Lottie animation.  It includes
/// options for the animation's file path, playback settings, speed, and rendering type.</remarks>
/// <param name="Path">The file path to the Lottie animation. This must be a valid path to the animation file.</param>
/// <param name="Loop">A value indicating whether the animation should loop continuously.  The default value is <see langword="true"/>.</param>
/// <param name="Autoplay">A value indicating whether the animation should start playing automatically.  The default value is <see
/// langword="true"/>.</param>
/// <param name="Speed">The playback speed of the animation. A value of 1 represents normal speed.  Values greater than 1 increase the
/// speed, while values between 0 and 1 decrease it.  The default value is 1.</param>
/// <param name="Renderer">The rendering type for the animation. This determines how the animation is rendered  (e.g., as SVG or Canvas). The
/// default value is <see cref="LottieRenderer.Svg"/>.</param>
public record LottieOptions(
    string Path,
    bool Loop = true,
    bool Autoplay = true,
    double Speed = 1,
    LottieRenderer Renderer = LottieRenderer.Svg)
{
}

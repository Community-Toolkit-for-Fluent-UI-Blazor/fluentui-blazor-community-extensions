namespace FluentUI.Blazor.Community.Components.ColorSpace.Vision;

/// <summary>
/// Represents the type of color vision deficiency (color blindness) that a person may have.
/// </summary>
public enum ColorVisionType : byte
{
    /// <summary>
    /// Specifies the normal display mode or default behavior.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Represents the Protanopia color vision deficiency, which affects the perception of red hues.
    /// </summary>
    /// <remarks>Protanopia is a type of red-green color blindness where individuals have difficulty
    /// distinguishing between red and green colors due to the absence of functioning red cone cells in the
    /// eye.</remarks>
    Protanopia = 2,

    /// <summary>
    /// Represents the Protanomaly type of color vision deficiency, characterized by reduced sensitivity to red light.
    /// </summary>
    /// <remarks>Protanomaly is a form of red-green color blindness in which the red cones in the eye are
    /// present but do not function normally. Individuals with protanomaly perceive red colors as more green and less
    /// bright than individuals with normal color vision.</remarks>
    Protanomaly = 3,

    /// <summary>
    /// Represents the Deuteranopia color vision deficiency, also known as green-blindness.
    /// </summary>
    /// <remarks>Deuteranopia is a type of red-green color blindness where green cones in the eye are absent
    /// or non-functional, affecting the perception of green hues.</remarks>
    Deuteranopia = 4,

    /// <summary>
    /// Represents the deuteranomaly type of color vision deficiency, commonly known as green-weakness.
    /// </summary>
    /// <remarks>Deuteranomaly is a form of red-green color blindness where green cone cells in the eye do not
    /// detect enough green and are too sensitive to yellows, oranges, and reds. This value can be used to indicate or
    /// simulate this specific color vision deficiency in accessibility scenarios.</remarks>
    Deuteranomaly = 5,

    /// <summary>
    /// Represents the tritanopia color vision deficiency, which affects the perception of blue and yellow hues.
    /// </summary>
    /// <remarks>Tritanopia is a type of color blindness characterized by the absence of blue cone cells in
    /// the eye, resulting in difficulty distinguishing between blue and yellow colors. Use this value to indicate or
    /// simulate tritanopia in color-related functionality.</remarks>
    Tritanopia = 6,

    /// <summary>
    /// Represents tritanomaly, a type of color vision deficiency characterized by reduced sensitivity to blue light.
    /// </summary>
    /// <remarks>Tritanomaly is a form of blue-yellow color blindness where the blue cone photopigments in the
    /// eye are altered, resulting in difficulty distinguishing between blue and green hues. This value can be used to
    /// indicate or handle scenarios involving tritanomaly in color accessibility features.</remarks>
    Tritanomaly = 7,

    /// <summary>
    /// Represents the Achromatopsia color vision deficiency type.
    /// </summary>
    /// <remarks>Achromatopsia is a condition characterized by a complete inability to perceive color,
    /// resulting in vision that is limited to shades of gray. Use this value to indicate or detect scenarios where full
    /// color blindness must be considered.</remarks>
    Achromatopsia = 8,

    /// <summary>
    /// Represents the Achromatomaly color vision deficiency, a type of partial color blindness characterized by reduced
    /// sensitivity to color.
    /// </summary>
    /// <remarks>Achromatomaly is a rare form of color vision deficiency in which individuals perceive colors
    /// with less intensity, often resulting in a washed-out appearance. This value can be used to indicate or simulate
    /// this specific type of color vision deficiency in accessibility or visualization scenarios.</remarks>
    Achromatomaly = 9
}

using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an animated element with properties that can be serialized to JSON for animation purposes.
/// </summary>
public class JsonAnimatedElement
{
    /// <summary>
    /// Gets or sets the unique identifier for the animated element.
    /// </summary>
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the horizontal position of the element.
    /// </summary>
    [JsonPropertyName("x")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? X { get; set; }

    /// <summary>
    /// Gets or sets the vertical position of the element.
    /// </summary>
    [JsonPropertyName("y")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Y { get; set; }

    /// <summary>
    /// Gets or sets the horizontal scaling factor of the element. A value of 1.0 represents no scaling.
    /// </summary>
    [JsonPropertyName("sx")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? ScaleX { get; set; }

    /// <summary>
    /// Gets or sets the vertical scaling factor of the element. A value of 1.0 represents no scaling.
    /// </summary>
    [JsonPropertyName("sy")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? ScaleY { get; set; }

    /// <summary>
    /// Gets or sets the rotation angle of the element in degrees.
    /// </summary>
    [JsonPropertyName("r")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Rotation { get; set; }

    /// <summary>
    /// Gets or sets the color of the element in a string format (e.g., hex code, RGB, etc.).
    /// </summary>
    [JsonPropertyName("c")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the background color of the element in a string format (e.g., hex code, RGB, etc.).
    /// </summary>
    [JsonPropertyName("bc")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the element, where 1.0 is fully opaque and 0.0 is fully transparent.
    /// </summary>
    [JsonPropertyName("o")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Opacity { get; set; }

    /// <summary>
    /// Gets or sets a generic value associated with the element, which can be used for custom purposes.
    /// </summary>
    [JsonPropertyName("v")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? Value { get; set; }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a converter to a <see cref="OperatingSystem"/> enumeration from a <see cref="string"/> format.
/// </summary>
internal sealed class OperatingSystemConverter
    : JsonConverter<OperatingSystem>
{
    /// <inheritdoc />
    public override OperatingSystem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();

        if (Enum.TryParse(s, out OperatingSystem mobile))
        {
            return mobile;
        }

        return OperatingSystem.Undefined;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, OperatingSystem value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

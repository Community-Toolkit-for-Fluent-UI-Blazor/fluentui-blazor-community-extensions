using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a converter to a <see cref="Browser"/> enumeration from a <see cref="string"/> format.
/// </summary>
internal sealed class BrowserConverter
    : JsonConverter<Browser>
{
    /// <inheritdoc />
    public override Browser Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();

        if (Enum.TryParse(s, out Browser browser))
        {
            return browser;
        }

        return Browser.Undefined;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Browser value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

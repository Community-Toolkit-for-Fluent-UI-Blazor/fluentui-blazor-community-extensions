using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides a custom JSON converter for serializing and deserializing values of the DeviceBreakpoint enumeration.
/// </summary>
/// <remarks>This converter enables the conversion between JSON string representations and the DeviceBreakpoint
/// enum values when using System.Text.Json serialization. If the input string does not match a valid DeviceBreakpoint
/// value during deserialization, the default value DeviceBreakpoint.Md is returned.</remarks>
internal sealed class DeviceBreakpointConverter : JsonConverter<DeviceBreakpoint>
{
    /// <inheritdoc />
    public override DeviceBreakpoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();

        if (Enum.TryParse(s, true, out DeviceBreakpoint breakpoint))
        {
            return breakpoint;
        }

        return DeviceBreakpoint.Unknown;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DeviceBreakpoint value, JsonSerializerOptions options) => throw new NotImplementedException();
}

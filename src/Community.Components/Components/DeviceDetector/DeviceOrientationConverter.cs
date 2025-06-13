// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a converter to a <see cref="DeviceOrientation"/> enumeration from a <see cref="string"/> format.
/// </summary>
internal sealed class DeviceOrientationConverter
    : JsonConverter<DeviceOrientation>
{
    /// <inheritdoc />
    public override DeviceOrientation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();

        if (Enum.TryParse(s, out DeviceOrientation orientation))
        {
            return orientation;
        }

        return DeviceOrientation.Unknown;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, DeviceOrientation value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

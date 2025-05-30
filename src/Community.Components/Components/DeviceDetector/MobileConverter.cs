// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a converter to a <see cref="Mobile"/> enumeration from a <see cref="string"/> format.
/// </summary>
internal sealed class MobileConverter
    : JsonConverter<Mobile>
{
    /// <inheritdoc />
    public override Mobile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var s = reader.GetString();

        if (Enum.TryParse(s, out Mobile mobile))
        {
            return mobile;
        }

        return Mobile.NotMobileDevice;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Mobile value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

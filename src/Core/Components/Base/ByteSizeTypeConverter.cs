using System.ComponentModel;
using System.Globalization;

namespace FluentUI.Blazor.Community;

/// <summary>
/// Provides a type converter to convert between string representations and ByteSize values.
/// </summary>
/// <remarks>This converter enables parsing and formatting of ByteSize values from and to strings, supporting
/// scenarios such as property grid editing, serialization, and deserialization. It is typically used by the .NET type
/// conversion infrastructure when working with ByteSize properties in design-time environments or configuration
/// files.</remarks>
internal sealed class ByteSizeTypeConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType == typeof(string);

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) => destinationType == typeof(string);

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value is string s ? ByteSize.Parse(s, CultureInfo.CurrentCulture) : base.ConvertFrom(context, culture, value);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return destinationType == typeof(string) && value is ByteSize bs
            ? bs.ToBinaryString(CultureInfo.CurrentCulture)
            : base.ConvertTo(context, culture, value, destinationType);
    }
}

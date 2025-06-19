using System.ComponentModel;
using System.Globalization;

namespace FluentUI.Blazor.Community;

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

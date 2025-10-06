namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// 
/// </summary>
public sealed class StringColorInterpolator
    : IInterpolator<string>
{
    /// <inheritdoc />
    public string Lerp(string start, string end, double amount)
    {
        var colorA = System.Drawing.ColorTranslator.FromHtml(start);
        var colorB = System.Drawing.ColorTranslator.FromHtml(end);

        var r = (byte)Math.Round(colorA.R + (colorB.R - colorA.R) * amount);
        var g = (byte)Math.Round(colorA.G + (colorB.G - colorA.G) * amount);
        var b = (byte)Math.Round(colorA.B + (colorB.B - colorA.B) * amount);
        var a = (byte)Math.Round(colorA.A + (colorB.A - colorA.A) * amount);

        var color = System.Drawing.Color.FromArgb(a, r, g, b);

        return System.Drawing.ColorTranslator.ToHtml(color);
    }
}

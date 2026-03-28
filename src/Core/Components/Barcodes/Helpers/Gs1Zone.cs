using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components.Helpers;

internal static class Gs1Zone
{
    /// <summary>
    /// Adds a sequence of digit text elements to the specified list, positioning them evenly within a defined
    /// horizontal zone.
    /// </summary>
    /// <remarks>Each digit is centered within its allocated slot between zoneStart and zoneEnd. The method
    /// does not validate that the specified range in data contains only digit characters.</remarks>
    /// <param name="texts">The list to which the generated digit text elements will be added.</param>
    /// <param name="data">The string containing the digit characters to be displayed.</param>
    /// <param name="dataIndexStart">The zero-based index in the data string at which to start extracting digits.</param>
    /// <param name="count">The number of digit text elements to add. Must be greater than zero and not exceed the number of available
    /// characters in data starting from dataIndexStart.</param>
    /// <param name="zoneStart">The starting X-coordinate of the horizontal zone in which the digits will be placed.</param>
    /// <param name="zoneEnd">The ending X-coordinate of the horizontal zone in which the digits will be placed.</param>
    /// <param name="y">The Y-coordinate at which all digit text elements will be positioned.</param>
    public static void AddDigitZone(
        List<BarcodeText> texts,
        string data,
        int dataIndexStart,
        int count,
        double zoneStart,
        double zoneEnd,
        double y)
    {
        var zoneWidth = zoneEnd - zoneStart;
        var slotWidth = zoneWidth / count;

        for (var slot = 0; slot < count; slot++)
        {
            texts.Add(new BarcodeText
            {
                X = zoneStart + (slot + 0.5) * slotWidth,
                Y = y,
                Text = data[dataIndexStart + slot].ToString(),
                Anchor = SvgTextAnchor.Middle
            });
        }
    }

    /// <summary>
    /// Adds a leftmost digit label to the specified collection of barcode text elements, positioning it at the start of
    /// the barcode zone.
    /// </summary>
    /// <param name="texts">The collection of barcode text elements to which the left digit label will be added. Cannot be null.</param>
    /// <param name="digit">The character representing the digit to display as the leftmost label.</param>
    /// <param name="zoneStart">The X-coordinate marking the start of the barcode zone, used to position the digit label.</param>
    /// <param name="slotWidth">The width of a single barcode slot, used to offset the digit label from the zone start.</param>
    /// <param name="y">The Y-coordinate at which to place the digit label.</param>
    public static void AddOuterLeftDigit(
        List<BarcodeText> texts,
        char digit,
        double zoneStart,
        double slotWidth,
        double y)
    {
        texts.Add(new BarcodeText
        {
            X = zoneStart - slotWidth * 0.5,
            Y = y,
            Text = digit.ToString(),
            Anchor = SvgTextAnchor.End
        });
    }

    /// <summary>
    /// Adds a rightmost digit label to the specified collection of barcode text elements, positioning it at the end of
    /// the barcode zone.
    /// </summary>
    /// <param name="texts">The collection of barcode text elements to which the left digit label will be added. Cannot be null.</param>
    /// <param name="digit">The character representing the digit to display as the leftmost label.</param>
    /// <param name="zoneEnd">The X-coordinate marking the end of the barcode zone, used to position the digit label.</param>
    /// <param name="slotWidth">The width of a single barcode slot, used to offset the digit label from the zone start.</param>
    /// <param name="y">The Y-coordinate at which to place the digit label.</param>
    public static void AddOuterRightDigit(
        List<BarcodeText> texts,
        char digit,
        double zoneEnd,
        double slotWidth,
        double y)
    {
        texts.Add(new BarcodeText
        {
            X = zoneEnd + slotWidth * 0.5,
            Y = y,
            Text = digit.ToString(),
            Anchor = SvgTextAnchor.Start
        });
    }
}

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides lookup and metadata information for a set of common GS1 Application Identifiers (AIs).
/// </summary>
/// <remarks>This class exposes a predefined list of GS1 Application Identifiers and their associated metadata,
/// such as length constraints and content requirements. It is intended to assist in parsing, validating, or
/// interpreting GS1 barcodes and data structures. The set of AIs included is not exhaustive but covers frequently used
/// identifiers. This class is thread-safe for read operations.</remarks>
internal static class Gs1AiTable
{
    /// <summary>
    /// Provides a read-only list of GS1 Application Identifier (AI) definitions, including their format and
    /// constraints.
    /// </summary>
    /// <remarks>Each entry in the list describes a specific GS1 Application Identifier, specifying its code,
    /// whether it is of fixed length, its length constraints, and whether it contains only digits. This list can be
    /// used to validate or interpret GS1 barcodes and data structures according to the GS1 standard.</remarks>
    private static readonly List<Gs1AiInfo> Ais =
    [
        // --------------------------------------------------------------------
        // Identification of Trade Items
        // --------------------------------------------------------------------
        new() { Ai = "00", IsFixedLength = true,  Length = 18, MaxLength = 18, DigitsOnly = true }, // SSCC
        new() { Ai = "01", IsFixedLength = true,  Length = 14, MaxLength = 14, DigitsOnly = true }, // GTIN
        new() { Ai = "02", IsFixedLength = true,  Length = 14, MaxLength = 14, DigitsOnly = true }, // GTIN of contained items
        new() { Ai = "10", IsFixedLength = false, Length = 0,  MaxLength = 20, DigitsOnly = false },// Batch / Lot
        new() { Ai = "11", IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Production date YYMMDD
        new() { Ai = "12", IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Due date YYMMDD
        new() { Ai = "13", IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Packaging date YYMMDD
        new() { Ai = "15", IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Best before YYMMDD
        new() { Ai = "16", IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Sell by YYMMDD
        new() { Ai = "17", IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Expiry YYMMDD
        new() { Ai = "20", IsFixedLength = true,  Length = 2,  MaxLength = 2,  DigitsOnly = true }, // Variant
        new() { Ai = "21", IsFixedLength = false, Length = 0,  MaxLength = 20, DigitsOnly = false },// Serial
        new() { Ai = "22", IsFixedLength = false, Length = 0,  MaxLength = 29, DigitsOnly = false },// Consumer product variant

        // --------------------------------------------------------------------
        // Measurements
        // --------------------------------------------------------------------
        new() { Ai = "30", IsFixedLength = false, Length = 0,  MaxLength = 8,  DigitsOnly = true }, // Count
        new() { Ai = "31", IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Weight/length/area/volume (31xx)
        new() { Ai = "310",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Net weight kg
        new() { Ai = "311",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Length m
        new() { Ai = "312",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Width m
        new() { Ai = "313",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Height m
        new() { Ai = "314",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Area m²
        new() { Ai = "315",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Volume l
        new() { Ai = "316",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Volume m³

        // --------------------------------------------------------------------
        // Additional product identification
        // --------------------------------------------------------------------
        new() { Ai = "320",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Net weight lb
        new() { Ai = "330",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Logistic weight kg
        new() { Ai = "340",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Logistic length m
        new() { Ai = "350",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Logistic area m²
        new() { Ai = "360",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Logistic volume l

        // --------------------------------------------------------------------
        // Additional IDs
        // --------------------------------------------------------------------
        new() { Ai = "240",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// Additional ID
        new() { Ai = "241",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// Customer part number
        new() { Ai = "242",IsFixedLength = false, Length = 0,  MaxLength = 6,  DigitsOnly = true }, // Addl GTIN ID
        new() { Ai = "243",IsFixedLength = false, Length = 0,  MaxLength = 20, DigitsOnly = false },// Packaging component
        new() { Ai = "250",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// Secondary serial
        new() { Ai = "251",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// Reference to source entity

        // --------------------------------------------------------------------
        // Logistics
        // --------------------------------------------------------------------
        new() { Ai = "37", IsFixedLength = false, Length = 0,  MaxLength = 8,  DigitsOnly = true }, // Count of trade items
        new() { Ai = "400",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// Order number
        new() { Ai = "401",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// Consignment number
        new() { Ai = "402",IsFixedLength = true,  Length = 17, MaxLength = 17, DigitsOnly = true }, // Shipment ID
        new() { Ai = "403",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// Routing code

        // --------------------------------------------------------------------
        // Locations
        // --------------------------------------------------------------------
        new() { Ai = "410",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // Ship-to GLN
        new() { Ai = "411",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // Bill-to GLN
        new() { Ai = "412",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // Purchase-from GLN
        new() { Ai = "413",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // Ship-for GLN
        new() { Ai = "414",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // GLN of physical location
        new() { Ai = "415",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // GLN of invoicing party
        new() { Ai = "416",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // GLN of production/servicing

        // --------------------------------------------------------------------
        // Amounts
        // --------------------------------------------------------------------
        new() { Ai = "420",IsFixedLength = false, Length = 0,  MaxLength = 20, DigitsOnly = false },// Ship-to postal code
        new() { Ai = "421",IsFixedLength = false, Length = 0,  MaxLength = 15, DigitsOnly = false },// Ship-to postal code + country
        new() { Ai = "422",IsFixedLength = true,  Length = 3,  MaxLength = 3,  DigitsOnly = true }, // Country of origin
        new() { Ai = "423",IsFixedLength = false, Length = 0,  MaxLength = 15, DigitsOnly = false },// Country subdivision
        new() { Ai = "424",IsFixedLength = true,  Length = 3,  MaxLength = 3,  DigitsOnly = true }, // Country of processing
        new() { Ai = "425",IsFixedLength = false, Length = 0,  MaxLength = 15, DigitsOnly = false },// Country subdivision of processing
        new() { Ai = "426",IsFixedLength = true,  Length = 3,  MaxLength = 3,  DigitsOnly = true }, // Country of full process

        // --------------------------------------------------------------------
        // Price
        // --------------------------------------------------------------------
        new() { Ai = "390",IsFixedLength = false, Length = 0,  MaxLength = 15, DigitsOnly = true }, // Amount payable
        new() { Ai = "391",IsFixedLength = false, Length = 0,  MaxLength = 15, DigitsOnly = true }, // Amount payable with ISO currency
        new() { Ai = "392",IsFixedLength = false, Length = 0,  MaxLength = 15, DigitsOnly = true }, // Price
        new() { Ai = "393",IsFixedLength = false, Length = 0,  MaxLength = 15, DigitsOnly = true }, // Price with ISO currency

        // --------------------------------------------------------------------
        // Dangerous goods
        // --------------------------------------------------------------------
        new() { Ai = "7001",IsFixedLength = true,  Length = 13, MaxLength = 13, DigitsOnly = true }, // UN number
        new() { Ai = "7002",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// DG class
        new() { Ai = "7003",IsFixedLength = true,  Length = 10, MaxLength = 10, DigitsOnly = true }, // DG packaging

        // --------------------------------------------------------------------
        // Healthcare
        // --------------------------------------------------------------------
        new() { Ai = "710",IsFixedLength = true,  Length = 20, MaxLength = 20, DigitsOnly = true }, // NHRN
        new() { Ai = "711",IsFixedLength = true,  Length = 20, MaxLength = 20, DigitsOnly = true }, // NHRN
        new() { Ai = "712",IsFixedLength = true,  Length = 20, MaxLength = 20, DigitsOnly = true }, // NHRN
        new() { Ai = "713",IsFixedLength = true,  Length = 20, MaxLength = 20, DigitsOnly = true }, // NHRN

        // --------------------------------------------------------------------
        // Misc
        // --------------------------------------------------------------------
        new() { Ai = "8001",IsFixedLength = true,  Length = 14, MaxLength = 14, DigitsOnly = true }, // Roll products
        new() { Ai = "8002",IsFixedLength = false, Length = 0,  MaxLength = 20, DigitsOnly = false },// Serial within batch
        new() { Ai = "8003",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// GRAI
        new() { Ai = "8004",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// GIAI
        new() { Ai = "8005",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Price per unit
        new() { Ai = "8006",IsFixedLength = true,  Length = 18, MaxLength = 18, DigitsOnly = true }, // Identification of components
        new() { Ai = "8007",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// IBAN
        new() { Ai = "8008",IsFixedLength = true,  Length = 12, MaxLength = 12, DigitsOnly = true }, // Date/time
        new() { Ai = "8018",IsFixedLength = true,  Length = 18, MaxLength = 18, DigitsOnly = true }, // GSRN
        new() { Ai = "8019",IsFixedLength = false, Length = 0,  MaxLength = 30, DigitsOnly = false },// GSRNP
        new() { Ai = "8020",IsFixedLength = false, Length = 0,  MaxLength = 25, DigitsOnly = false },// Ref to internal data
        new() { Ai = "8100",IsFixedLength = true,  Length = 6,  MaxLength = 6,  DigitsOnly = true }, // Coupon code
        new() { Ai = "8200",IsFixedLength = false, Length = 0,  MaxLength = 70, DigitsOnly = false },// Extended packaging URL
    ];

    /// <summary>
    /// Provides a lookup dictionary that maps GS1 Application Identifier (AI) strings to their corresponding
    /// information objects, using case-sensitive ordinal comparison.
    /// </summary>
    private static readonly Dictionary<string, Gs1AiInfo> ByAi = Ais.ToDictionary(a => a.Ai, a => a, StringComparer.Ordinal);

    /// <summary>
    /// Attempts to retrieve the GS1 Application Identifier (AI) information associated with the specified AI code.
    /// </summary>
    /// <param name="ai">The AI code to locate. Cannot be null.</param>
    /// <param name="info">When this method returns, contains the GS1 AI information associated with the specified AI code, if found;
    /// otherwise, null.</param>
    /// <returns>Returns <see langword="true" /> if the AI code was found and the corresponding information was retrieved; otherwise, <see langword="false"/> otherwise .</returns>
    public static bool TryGet(string ai, out Gs1AiInfo? info) => ByAi.TryGetValue(ai, out info);
}

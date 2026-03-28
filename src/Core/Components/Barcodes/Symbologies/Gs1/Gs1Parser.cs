using FluentUI.Blazor.Community.Components.Enums;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides static methods for parsing GS1-formatted data strings into collections of GS1 elements using various
/// validation modes.
/// </summary>
/// <remarks>This class supports multiple parsing strategies, including strict, permissive, and hybrid modes, to
/// accommodate different levels of GS1 compliance. It is intended for internal use and is not thread-safe. Use the
/// appropriate validation mode to control how strictly the input data is interpreted according to GS1
/// standards.</remarks>
internal static class Gs1Parser
{
    /// <summary>
    /// Parses the specified GS1 data string using the given validation mode and returns a list of extracted GS1
    /// elements.
    /// </summary>
    /// <remarks>Use <see cref="Gs1ValidationMode.Strict"/> for strict compliance with GS1 standards, <see
    /// cref="Gs1ValidationMode.Permissive"/> for more lenient parsing, or <see cref="Gs1ValidationMode.Hybrid"/> for a
    /// balanced approach. If an unrecognized mode is provided, hybrid mode is used by default.</remarks>
    /// <param name="data">The GS1 data string to parse. Cannot be null.</param>
    /// <param name="mode">The validation mode that determines how strictly the data is parsed and validated.</param>
    /// <returns>A list of <see cref="Gs1Element"/> objects representing the parsed elements from the input data. The list is
    /// empty if no elements are found.</returns>
    public static List<Gs1Element> Parse(string data, Gs1ValidationMode mode)
        => mode switch
        {
            Gs1ValidationMode.Strict => ParseStrict(data),
            Gs1ValidationMode.Permissive => ParsePermissive(data),
            Gs1ValidationMode.Hybrid => ParseHybrid(data),
            _ => ParseHybrid(data)
        };

    /// <summary>
    /// Parses the specified data string into a list containing a single permissive GS1 element.
    /// </summary>
    /// <remarks>This method does not validate the input data or attempt to extract a GS1 Application
    /// Identifier. It is intended for permissive parsing scenarios where the input may not conform to GS1
    /// standards.</remarks>
    /// <param name="data">The input data to be wrapped as a GS1 element. Can be any string, including invalid or non-standard GS1 data.</param>
    /// <returns>A list containing one GS1 element with the input data as its value. The element will have an empty Application
    /// Identifier (AI) and be marked as variable length.</returns>
    private static List<Gs1Element> ParsePermissive(string data)
        =>
        [
            new Gs1Element
            {
                Ai = string.Empty,
                Value = data,
                IsVariableLength = true
            }
        ];

    /// <summary>
    /// Parses the specified GS1 data string using strict validation rules and returns a list of parsed GS1 elements.
    /// </summary>
    /// <remarks>Strict parsing enforces all GS1 syntax and validation rules. Use this method when input data
    /// must fully comply with GS1 standards.</remarks>
    /// <param name="data">The GS1 data string to parse. Cannot be null or empty.</param>
    /// <returns>A list of <see cref="Gs1Element"/> objects representing the parsed elements from the input data. The list is
    /// empty if no valid elements are found.</returns>
    private static List<Gs1Element> ParseStrict(string data) => ParseInternal(data, true);

    /// <summary>
    /// Parses the specified string as a hybrid GS1 data format and returns a list of GS1 elements.
    /// </summary>
    /// <param name="data">The input string containing the hybrid GS1 data to parse. Cannot be null.</param>
    /// <returns>A list of <see cref="Gs1Element"/> objects representing the parsed elements from the hybrid data. Returns an
    /// empty list if no elements are found.</returns>
    private static List<Gs1Element> ParseHybrid(string data) => ParseInternal(data, false);

    /// <summary>
    /// Parses a GS1-formatted data string into a list of GS1 elements, optionally enforcing strict validation of
    /// Application Identifiers (AIs) and value formats.
    /// </summary>
    /// <remarks>This method supports both fixed-length and variable-length GS1 Application Identifiers. In
    /// non-strict mode, unrecognized or incomplete segments are treated as free-form elements rather than causing an
    /// exception.</remarks>
    /// <param name="data">The GS1 data string to parse. Must not be null.</param>
    /// <param name="strict">If set to <see langword="true"/>, the method enforces strict validation and throws exceptions for unknown or
    /// malformed AIs; if <see langword="false"/>, parsing continues in a best-effort mode.</param>
    /// <returns>A list of <see cref="Gs1Element"/> objects representing the parsed GS1 elements from the input data. The list
    /// may contain a single element with an empty AI if parsing fails in non-strict mode.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="data"/> contains an unknown or malformed AI, or if a value does not meet the expected
    /// format, and <paramref name="strict"/> is <see langword="true"/>.</exception>
    private static List<Gs1Element> ParseInternal(string data, bool strict)
    {
        var result = new List<Gs1Element>();
        var i = 0;

        while (i < data.Length)
        {
            Gs1AiInfo? aiInfo = null;
            var ai = string.Empty;

            for (var len = 4; len >= 2; len--)
            {
                if (i + len > data.Length)
                {
                    continue;
                }

                var candidate = data.Substring(i, len);

                if (Gs1AiTable.TryGet(candidate, out var info))
                {
                    aiInfo = info;
                    ai = candidate;
                    break;
                }
            }

            if (aiInfo is null)
            {
                if (strict)
                {
                    throw new ArgumentException($"Unknown GS1 AI at position {i}.");
                }

                result.Add(new Gs1Element
                {
                    Ai = string.Empty,
                    Value = data[i..],
                    IsVariableLength = true
                });

                break;
            }

            i += ai.Length;

            if (aiInfo.IsFixedLength)
            {
                if (i + aiInfo.Length > data.Length)
                {
                    if (strict)
                    {
                        throw new ArgumentException($"Insufficient data for AI {ai}.");
                    }

                    var remaining = data[i..];

                    result.Add(new Gs1Element
                    {
                        Ai = ai,
                        Value = remaining,
                        IsVariableLength = false
                    });

                    break;
                }

                var value = data.Substring(i, aiInfo.Length);

                if (aiInfo.DigitsOnly && !value.All(char.IsDigit))
                {
                    if (strict)
                    {
                        throw new ArgumentException($"AI {ai} expects digits only.");
                    }
                }

                result.Add(new Gs1Element
                {
                    Ai = ai,
                    Value = value,
                    IsVariableLength = false
                });

                i += aiInfo.Length;
            }
            else
            {
                var start = i;
                var maxEnd = Math.Min(data.Length, i + aiInfo.MaxLength);
                var end = maxEnd;

                for (var pos = i + 1; pos < maxEnd; pos++)
                {
                    var remaining = data[pos..];

                    if (LooksLikeAiStart(remaining))
                    {
                        end = pos;
                        break;
                    }
                }

                var value = data[start..end];

                if (aiInfo.DigitsOnly && !value.All(char.IsDigit))
                {
                    if (strict)
                    {
                        throw new ArgumentException($"AI {ai} expects digits only.");
                    }
                }

                result.Add(new Gs1Element
                {
                    Ai = ai,
                    Value = value,
                    IsVariableLength = true
                });

                i = end;
            }
        }

        return result;
    }

    /// <summary>
    /// Determines whether the beginning of the specified string matches a known GS1 Application Identifier (AI) prefix.
    /// </summary>
    /// <remarks>This method checks for GS1 AI prefixes of length 2 to 4 at the start of the input string. Use
    /// this method to quickly identify if a string may begin with a GS1 Application Identifier.</remarks>
    /// <param name="s">The string to examine for a possible GS1 AI prefix. Cannot be null.</param>
    /// <returns>true if the start of the string matches a known GS1 AI prefix; otherwise, false.</returns>
    private static bool LooksLikeAiStart(string s)
    {
        for (var len = 4; len >= 2; len--)
        {
            if (s.Length < len)
            {
                continue;
            }

            var candidate = s[..len];

            if (Gs1AiTable.TryGet(candidate, out _))
            {
                return true;
            }
        }

        return false;
    }
}

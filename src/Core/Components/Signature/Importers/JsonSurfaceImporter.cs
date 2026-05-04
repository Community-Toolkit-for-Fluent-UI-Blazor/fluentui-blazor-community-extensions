using System.Text.Json;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Provides functionality to import surface data from JSON input, deserializing it into a strongly typed payload.
/// </summary>
/// <remarks>This importer expects the input string to be a valid JSON object or array. It uses System.Text.Json
/// for deserialization and returns an error message if the input cannot be parsed. This class is intended for scenarios
/// where surface data is represented in JSON format and needs to be converted into a typed payload for further
/// processing.</remarks>
/// <typeparam name="TPayload">The type of the payload to deserialize from the JSON input.</typeparam>
public sealed class JsonSurfaceImporter<TPayload>
    : SurfaceImporter<TPayload>
{
    /// <summary>
    /// Represents the character used to indicate the start of an object value.
    /// </summary>
    private const char StartObjectValue = '{';

    /// <summary>
    /// Represents the character used to indicate the end of an object value.
    /// </summary>
    private const char EndObjectValue = '}';

    /// <summary>
    /// Represents the character used to indicate the start of an array value.
    /// </summary>
    private const char StartArrayValue = '[';

    /// <summary>
    /// Represents the character used to indicate the end of an array value.
    /// </summary>
    private const char EndArrayValue = ']';

    /// <inheritdoc />
    public override bool IsValidInput(string input)
    {
        var trimmed = input.Trim();

        return (trimmed.StartsWith(StartObjectValue) || trimmed.StartsWith(StartArrayValue)) &&
               (trimmed.EndsWith(EndObjectValue) || trimmed.EndsWith(EndArrayValue));
    }

    /// <inheritdoc />
    protected override async ValueTask<ImportSurfaceResult<TPayload>> ParseAsync(string input)
    {
        try
        {
            using var ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(input));
            var payload = await JsonSerializer.DeserializeAsync<SurfacePayload<TPayload>>(ms);

            return new ImportSurfaceResult<TPayload>(payload);
        }
        catch (Exception ex)
        {
            return new ImportSurfaceResult<TPayload>(ErrorMessage: ex.Message);
        }
    }
}


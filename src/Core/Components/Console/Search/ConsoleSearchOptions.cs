namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents the options for configuring search behavior in the console application.
/// </summary>
/// <remarks>This class allows users to specify which aspects of the console messages should be indexed during a
/// search operation. The properties can be set to enable or disable indexing for various message attributes.</remarks>
public sealed class ConsoleSearchOptions
{
    /// <summary>
    /// Gets or sets the maximum number of items to return in a query result.
    /// </summary>
    /// <remarks>If set, this property limits the number of items returned by the query. A value of null
    /// indicates that there is no limit on the number of items returned.</remarks>
    public int? Top { get; init; }

    /// <summary>
    /// Gets a value indicating whether the index message is enabled.
    /// </summary>
    /// <remarks>This property is initialized to <see langword="true"/>, indicating that the index message is
    /// enabled by default. It can be used to control the behavior of indexing operations.</remarks>
    public bool IndexMessage { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the index category is enabled.
    /// </summary>
    /// <remarks>This property defaults to <see langword="true"/>, indicating that the index category is
    /// active. Use this property to determine if the index category should be included in operations or
    /// displays.</remarks>
    public bool IndexCategory { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether the source is indexed.
    /// </summary>
    /// <remarks>This property defaults to <see langword="true"/>, indicating that the source is indexed
    /// unless explicitly set to <see langword="false"/>. Use this property to determine if indexing operations should
    /// be performed on the source.</remarks>
    public bool IndexSource { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether an index exception is expected during index-related operations.
    /// </summary>
    /// <remarks>This property is initialized to <see langword="true"/>, which means that index exceptions are
    /// anticipated by default. Use this property to determine if error handling for index exceptions should be enabled
    /// in scenarios involving index access.</remarks>
    public bool IndexException { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether index properties are included in the output.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, index properties will be included in the output;
    /// otherwise, they will be excluded. This property is typically used to control whether indexed members, such as
    /// those with parameterized accessors, are processed or displayed.</remarks>
    public bool IndexProperties { get; init; } = true;

    /// <summary>
    /// Gets a value indicating whether index tags are enabled during processing.
    /// </summary>
    /// <remarks>When set to <see langword="true"/>, index tags are included in the processing. This property
    /// is initialized to <see langword="true"/> by default.</remarks>
    public bool IndexTags { get; init; } = true;
}

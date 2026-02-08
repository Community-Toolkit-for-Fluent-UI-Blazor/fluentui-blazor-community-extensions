namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Defines a contract for enriching console output with additional information.
/// </summary>
/// <remarks>Implementations of this interface should provide specific enrichment logic to enhance the console
/// output based on the provided enrichment bag.</remarks>
public interface IConsoleEnricher
{
    /// <summary>
    /// Adds contextual information to the specified console enrichment bag to enhance logging output.
    /// </summary>
    /// <remarks>Use this method to supplement log entries with relevant context, which can assist in
    /// debugging and monitoring console applications. The method modifies the provided bag in place.</remarks>
    /// <param name="bag">The enrichment bag to which additional context will be added. Must be initialized before calling this method.</param>
    void Enrich(ConsoleEnrichmentBag bag);
}

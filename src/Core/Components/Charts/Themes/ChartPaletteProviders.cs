namespace FluentUI.Blazor.Community.Components.Charts.Themes;

/// <summary>
/// Represents a collection of predefined chart palette providers that can be used to generate color palettes for chart components based on specific requests. Each provider corresponds to a different palette style, allowing for a variety of visual themes to be applied to charts.
/// </summary>
public sealed class ChartPaletteProviders
{
    /// <summary>
    /// Represents a dictionary of chart palette providers.
    /// </summary>
    private readonly Dictionary<string, IChartPaletteProvider> _providers = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartPaletteProviders"/>.
    /// </summary>
    public ChartPaletteProviders()
    {
        _providers["Default"] = new DefaultPaletteProvider();
        _providers["Neon"] = new NeonPaletteProvider();
        _providers["Pastel"] = new PastelPaletteProvider();
        _providers["PowerBi"] = new PowerBiPaletteProvider();
        _providers["Protanopia"] = new ProtanopiaPaletteProvider();
        _providers["Deuteranopia"] = new DeuteranopiaPaletteProvider();
        _providers["Tritanopia"] = new TritanopiaPaletteProvider();
        _providers["Achroma"] = new AchromaPaletteProvider();
        _providers["Monochrome"] = new MonochromePaletteProvider();
        _providers["HighContrast"] = new HighContrastPaletteProvider();
        _providers["Winter"] = new WinterPaletteProvider();
        _providers["Spring"] = new SpringPaletteProvider();
        _providers["Summer"] = new SummerPaletteProvider();
        _providers["Autumn"] = new AutumnPaletteProvider();
    }

    /// <summary>
    /// Gets a chart palette provider by name.
    /// </summary>
    /// <param name="name">The name of the chart palette provider.</param>
    /// <returns>The chart palette provider associated with the specified name, or the default provider if not found.</returns>
    public IChartPaletteProvider Get(string name) => _providers.TryGetValue(name, out var p) ? p : _providers["Default"];

    /// <summary>
    /// Gets a chart palette provider by name.
    /// </summary>
    /// <param name="style">The name of the chart palette provider.</param>
    /// <returns>The chart palette provider associated with the specified name, or the default provider if not found.</returns>
    public IChartPaletteProvider Get(ChartPaletteStyle style) => _providers.TryGetValue(style.ToString(), out var p) ? p : _providers["Default"];

    /// <summary>
    /// Registers a chart palette provider with the specified name for later retrieval.
    /// </summary>
    /// <remarks>If a provider is already registered with the given name, it will be replaced by the new
    /// provider.</remarks>
    /// <param name="name">The unique name used to identify the palette provider. Cannot be null.</param>
    /// <param name="provider">The chart palette provider to associate with the specified name. Cannot be null.</param>
    public void Register(string name, IChartPaletteProvider provider) => _providers[name] = provider;
}


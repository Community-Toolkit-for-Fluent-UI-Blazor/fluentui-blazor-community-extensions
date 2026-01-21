using FluentUI.Blazor.Community.Components.ColorPalette.Infrastructure;
using FluentUI.Blazor.Community.Components.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a customizable color palette component that supports various color generation modes, gradient strategies,
/// and selection options.
/// </summary>
/// <remarks>This component allows users to generate and interact with a color palette using predefined modes,
/// gradients, or custom plugins. It supports single and multi-selection of colors, as well as advanced customization
/// options such as grid layout, item size, and maximum color limits. The palette can also be populated from an image or
/// preset configurations.</remarks>
public partial class FluentCxColorPalette : FluentComponentBase
{
    //private const string JAVASCRIPT_FILE = FluentCxConstants.JAVASCRIPT_ROOT + "ColorPalette/FluentCxColorPalette.razor.js";

    /// <summary>
    /// Represents the default set of colors provided by the palette when no custom colors are specified.
    /// </summary>
    private static readonly List<string> DefaultProvided =
    [
        "#000000","#ffffff","#ef4444","#f97316","#f59e0b","#eab308","#84cc16","#22c55e",
        "#10b981","#06b6d4","#3b82f6","#6366f1","#8b5cf6","#a855f7","#ec4899","#f43f5e",
        "#6b7280","#94a3b8","#64748b","#374151","#111827"
    ];

    /// <summary>
    /// Represents a unique identifier for the color palette instance.
    /// </summary>
    private ElementReference[] _buttonsRef = [];

    /// <summary>
    /// Represents the list of colors currently generated and displayed in the palette.
    /// </summary>
    private List<string> _colors = [];

    /// <summary>
    /// Represents the index of the currently focused item.
    /// </summary>
    /// <remarks>This field is used to track the position of the focused item within a collection or
    /// list.</remarks>
    private int _focusIndex;

    /// <summary>
    /// Represents an error message that may be displayed to the user.
    /// </summary>
    private string? _errorMessage;

    /// <summary>
    /// Represents the number of colors to generate in the palette.
    /// </summary>
    private int _colorCount;

    /// <summary>
    /// Represents a flag indicating whether the number of colors has changed.
    /// </summary>
    private bool _numberOfColorsChanged;

    /// <summary>
    /// Represents a flag indicating whether any other property has changed.
    /// </summary>
    private bool _anyOtherPropertyChanged;

    /// <summary>
    /// Represents a set of generated colors for efficient lookup.
    /// </summary>
    private readonly HashSet<string> _generatedColorsSet = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Represents all built-in color plugins available for generating color schemes.
    /// </summary>
    private static readonly IColorPlugin[] _plugins = [
        new ComplementaryPlugin(),
        new AnalogousPlugin(),
        new TriadicPlugin(),
        new TetradicPlugin(),
        new SplitComplementaryPlugin(),
        new MonochromaticPlugin(),
        new WarmPlugin(),
        new CoolPlugin(),
        new PastelPlugin(),
        new NeonPlugin(),
        new GrayscalePlugin(),
        new AccessibilitySafePlugin(),
        new DesaturatePlugin()
    ];

    /// <summary />
    public FluentCxColorPalette(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets the internal class the component use.
    /// </summary>
    private string? InternalClass => DefaultClassBuilder
        .AddClass("fluentcx-color-palette")
        .Build();

    /// <summary>
    /// Gets the internal style the component use.
    /// </summary>
    private string? InternalStyle => DefaultStyleBuilder
        .AddStyle("grid-template-columns", $"repeat(auto-fill, {ItemSize}px)")
        .AddStyle("max-height", $"{MaxHeight}px", when: MaxHeight > 0)
        .AddStyle("gap", GridGap, when: !string.IsNullOrWhiteSpace(GridGap))
        .Build();

    /// <summary>
    /// Gets or sets the size of each color item in the grid, in pixels. Default is 28.
    /// </summary>
    [Parameter]
    public int ItemSize { get; set; } = 28;

    /// <summary>
    /// Gets or sets the gap between grid items, in pixels. Default is 6.
    /// </summary>
    [Parameter]
    public string GridGap { get; set; } = "6px";

    /// <summary>
    /// Gets or sets the maximum height of the color palette container, in pixels. Default is 300.
    /// </summary>
    [Parameter]
    public int MaxHeight { get; set; } = 300;

    /// <summary>
    /// Gets or sets the color generation mode for the palette. Default is <see cref="ColorPaletteMode.Provided"/>.
    /// </summary>
    [Parameter]
    public ColorPaletteMode Mode { get; set; } = ColorPaletteMode.Provided;

    /// <summary>
    /// Gets or sets a list of user-provided colors to be used when the mode is set to <see cref="ColorPaletteMode.Provided"/>.
    /// </summary>
    [Parameter]
    public List<string>? ProvidedColors { get; set; }

    /// <summary>
    /// Gets or sets the base color used for generating gradients and color schemes. Default is "#3B82F6".
    /// </summary>
    [Parameter]
    public string BaseColor { get; set; } = "#3B82F6";

    /// <summary>
    /// Gets or sets the strategy used for generating gradients. Default is <see cref="GradientStrategy.Shades"/>.
    /// </summary>
    [Parameter]
    public GradientStrategy GradientStrategy { get; set; } = GradientStrategy.Shades;

    /// <summary>
    /// Gets or sets the number of steps to use when generating gradients. Default is 24.
    /// </summary>
    [Parameter]
    public int GradientSteps { get; set; } = 24;

    /// <summary>
    /// Gets or sets the starting color for custom gradient generation.
    /// </summary>
    [Parameter]
    public string? GradientStart { get; set; }

    /// <summary>
    /// Gets or sets the ending color for custom gradient generation.
    /// </summary>
    [Parameter]
    public string? GradientEnd { get; set; }

    /// <summary>
    /// Gets or sets additional options for color generation, such as brightness and contrast adjustments.
    /// </summary>
    [Parameter]
    public GenerationOptions GenerationOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether multiple colors can be selected simultaneously.
    /// </summary>
    [Parameter]
    public bool MultiSelect { get; set; } = false;

    /// <summary>
    /// Gets or sets the currently selected color when <see cref="MultiSelect"/> is false.
    /// </summary>
    [Parameter]
    public string? SelectedColor { get; set; }

    /// <summary>
    /// Gets or sets an event callback that is invoked when the selected color changes.
    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedColorChanged { get; set; }

    /// <summary>
    /// Gets or sets the list of currently selected colors when <see cref="MultiSelect"/> is true.
    /// </summary>
    [Parameter]
    public List<string> SelectedColors { get; set; } = [];

    /// <summary>
    /// Gets or sets an event callback that is invoked when the list of selected colors changes.
    /// </summary>
    [Parameter]
    public EventCallback<List<string>> SelectedColorsChanged { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show a preview of the selected color(s) above the palette.
    /// </summary>
    [Parameter]
    public bool ShowPreview { get; set; } = true;

    /// <summary>
    /// Gets or sets a list of custom color plugins that can be used to generate additional color schemes.
    /// </summary>
    [Parameter]
    public List<IColorPlugin> Plugins { get; set; } = [];

    /// <summary>
    /// Gets or sets the maximum number of colors to generate in the palette. Default is 120.
    /// </summary>
    [Parameter]
    public int MaxColors { get; set; } = 120;

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            //await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);
        }
    }

    /// <summary>
    /// Generates the color palette based on the current settings and parameters.
    /// </summary>
    /// <returns>Returns a task which generates the colors of the palette when completed.</returns>
    private async Task GenerateColorsAsync()
    {
        _errorMessage = string.Empty;
        List<string> generated;

        if (Mode is ColorPaletteMode.CustomGradient && (string.IsNullOrWhiteSpace(GradientStart) || string.IsNullOrWhiteSpace(GradientEnd)))
        {
            _errorMessage = Localizer[LanguageResource.CX_ColorPalette_CustomGradient_Error];
            _colors = [];
            return;
        }

        try
        {

            generated = Mode switch
            {
                ColorPaletteMode.Provided => [.. ProvidedColors ?? DefaultProvided],
                ColorPaletteMode.Random => ColorUtils.GenerateRandomHex(_colorCount),
                ColorPaletteMode.Gradient => ColorUtils.GenerateGradient(BaseColor, _colorCount, GradientStrategy, GenerationOptions),
                ColorPaletteMode.CustomGradient => ColorUtils.GenerateCustomGradient(GradientStart!, GradientEnd!, _colorCount, GenerationOptions),
                ColorPaletteMode.Complementary => _plugins[0].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Analogous => _plugins[1].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Triadic => _plugins[2].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Tetradic => _plugins[3].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.SplitComplementary => _plugins[4].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Monochrome => _plugins[5].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Warm => _plugins[6].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Cool => _plugins[7].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Pastel => _plugins[8].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Neon => _plugins[9].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.Greyscale => _plugins[10].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.AccessibilitySafe => _plugins[11].Generate(BaseColor, _colorCount, GenerationOptions),
                ColorPaletteMode.None => [],
                ColorPaletteMode.Desaturate => _plugins[12].Generate(BaseColor, _colorCount, GenerationOptions),
                _ => ColorUtils.GenerateScheme(BaseColor, Mode, _colorCount, GenerationOptions),
            };

            if (Plugins is not null && Plugins.Count > 0)
            {
                foreach (var gen in Plugins)
                {
                    var pluginColors = gen.Generate(BaseColor, _colorCount, GenerationOptions);

                    if (pluginColors?.Count > 0)
                    {
                        generated = [.. generated, .. pluginColors];
                    }
                }
            }

            // Validates and normalize the colors.
            generated = [.. generated
                .Where(ColorUtils.IsValidHexOrCssName)
                .Select(ColorUtils.NormalizeToHex)
                .Where(ColorUtils.IsValidHex)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Take(MaxColors)];

            if (generated.Count == 0)
            {
                throw new InvalidOperationException("No valid color was generated.");
            }

            _colors = generated;

            _focusIndex = Math.Clamp(_focusIndex, 0, _colors.Count - 1);
            SelectedColor = _colors[_focusIndex];
        }
        catch (Exception ex)
        {
            _errorMessage = ex.Message;
            _colors = [];
        }

        _buttonsRef = new ElementReference[_colors.Count];
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Toggles the selection state of a color at the specified index.
    /// </summary>
    /// <param name="hex">Selected color to toggle.</param>
    /// <param name="index">Index of the selected color.</param>
    /// <returns>Returns a task which toggle the color when completed.</returns>
    private async Task ToggleSelectAsync(string hex, int index)
    {
        _focusIndex = index;

        if (MultiSelect)
        {
            if (_generatedColorsSet.Remove(hex))
            {
                SelectedColors.Remove(hex);
            }
            else
            {
                _generatedColorsSet.Add(hex);
                SelectedColors.Add(hex);
            }

            if (SelectedColorsChanged.HasDelegate)
            {
                await SelectedColorsChanged.InvokeAsync(SelectedColors);
            }
        }
        else
        {
            SelectedColor = hex;

            if (SelectedColorChanged.HasDelegate)
            {
                await SelectedColorChanged.InvokeAsync(hex);
            }
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (_numberOfColorsChanged)
        {
            _numberOfColorsChanged = false;
            _colorCount = Math.Min(MaxColors, GradientSteps);
        }

        await base.OnParametersSetAsync();

        if (_anyOtherPropertyChanged)
        {
            _anyOtherPropertyChanged = false;
            await GenerateColorsAsync();
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _numberOfColorsChanged = parameters.TryGetValue<int>(nameof(GradientSteps), out _) ||
                                 parameters.TryGetValue<int>(nameof(MaxColors), out _);

        _anyOtherPropertyChanged = parameters.TryGetValue<ColorPaletteMode>(nameof(Mode), out _) ||
                                   parameters.TryGetValue<List<string>?>(nameof(ProvidedColors), out _) ||
                                   parameters.TryGetValue<string>(nameof(BaseColor), out _) ||
                                   parameters.TryGetValue<GradientStrategy>(nameof(GradientStrategy), out _) ||
                                   parameters.TryGetValue<string?>(nameof(GradientStart), out _) ||
                                   parameters.TryGetValue<string?>(nameof(GradientEnd), out _) ||
                                   parameters.TryGetValue<GenerationOptions>(nameof(GenerationOptions), out _) ||
                                   parameters.TryGetValue<List<IColorPlugin>>(nameof(Plugins), out _) ||
                                   _numberOfColorsChanged;

        if (parameters.TryGetValue<List<string>>(nameof(SelectedColors), out var newList))
        {
            _generatedColorsSet.Clear();

            foreach (var c in newList)
            {
                _generatedColorsSet.Add(c);
            }
        }

        return base.SetParametersAsync(parameters);
    }
}

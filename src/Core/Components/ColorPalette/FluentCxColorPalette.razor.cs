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
    /// Gets or sets the label for the harmony mode picker. Default is "Harmony".
    /// </summary>
    [Parameter]
    public string HarmonyLabel { get; set; } = "Harmony";

    /// <summary>
    /// Gets or sets the label for the preset picker. Default is "Presets".
    /// </summary>
    [Parameter]
    public string PresetLabel { get; set; } = "Presets";

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
    /// Gets or sets a dictionary of preset color lists that can be selected by the user.
    /// </summary>
    [Parameter]
    public Dictionary<string, List<string>>? Presets { get; set; }

    /// <summary>
    /// Gets or sets the key of the currently selected preset from the <see cref="Presets"/> dictionary.
    /// </summary>
    [Parameter]
    public string? SelectedPreset { get; set; }

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
    /// Gets or sets a value indicating whether to display the harmony mode picker when applicable.
    /// </summary>
    [Parameter]
    public bool ShowHarmonyPicker { get; set; } = true;

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

    /// <summary>
    /// Gets a set of selected colors for efficient lookup.
    /// </summary>
    private HashSet<string> SelectedColorsSet => new(SelectedColors, StringComparer.OrdinalIgnoreCase);

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
            if (!string.IsNullOrWhiteSpace(SelectedPreset) && Presets is not null && Presets.TryGetValue(SelectedPreset, out var presetList))
            {
                generated = [.. presetList];
            }
            else
            {
                generated = Mode switch
                {
                    ColorPaletteMode.Provided => [.. ProvidedColors ?? DefaultProvided],
                    ColorPaletteMode.Random => ColorUtils.GenerateRandomHex(Math.Min(MaxColors, GradientSteps)),
                    ColorPaletteMode.Gradient => ColorUtils.GenerateGradient(BaseColor, Math.Min(MaxColors, GradientSteps), GradientStrategy, GenerationOptions),
                    ColorPaletteMode.CustomGradient => ColorUtils.GenerateCustomGradient(GradientStart!, GradientEnd!, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Complementary => new ComplementaryPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Analogous => new AnalogousPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Triadic => new TriadicPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Tetradic => new TetradicPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.SplitComplementary => new SplitComplementaryPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Monochrome => new MonochromaticPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Warm => new WarmPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Cool => new CoolPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Pastel => new PastelPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Neon => new NeonPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.Greyscale => new GrayscalePlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.AccessibilitySafe => new AccessibilitySafePlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    ColorPaletteMode.None => [],
                    ColorPaletteMode.Desaturate => new DesaturatePlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                    _ => ColorUtils.GenerateScheme(BaseColor, Mode, Math.Min(MaxColors, GradientSteps), GenerationOptions),
                };

                if (Plugins is not null && Plugins.Count > 0)
                {
                    foreach (var gen in Plugins)
                    {
                        var pluginColors = gen.Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);

                        if (pluginColors?.Count > 0)
                        {
                            generated = [.. generated, .. pluginColors];
                        }
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
            if (SelectedColorsSet.Contains(hex))
            {
                SelectedColors.Remove(hex);
            }
            else
            {
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
        await base.OnParametersSetAsync();
        await GenerateColorsAsync();
    }
}

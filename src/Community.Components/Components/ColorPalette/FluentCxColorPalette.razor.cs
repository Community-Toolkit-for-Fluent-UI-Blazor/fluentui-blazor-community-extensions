using System.Collections.ObjectModel;
using FluentUI.Blazor.Community.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a customizable color palette component that supports various color generation modes, gradient strategies,
/// and selection options.
/// </summary>
/// <remarks>This component allows users to generate and interact with a color palette using predefined modes,
/// gradients, or custom plugins. It supports single and multi-selection of colors, as well as advanced customization
/// options such as grid layout, item size, and maximum color limits. The palette can also be populated from an image or
/// preset configurations.</remarks>
public partial class FluentCxColorPalette
    : FluentComponentBase, IAsyncDisposable
{
    #region Fields

    /// <summary>
    /// Represents the default set of colors provided by the palette when no custom colors are specified.
    /// </summary>
    private static readonly List<string> DefaultProvided = [
        "#000000","#ffffff","#ef4444","#f97316","#f59e0b","#eab308","#84cc16","#22c55e",
        "#10b981","#06b6d4","#3b82f6","#6366f1","#8b5cf6","#a855f7","#ec4899","#f43f5e",
        "#6b7280","#94a3b8","#64748b","#374151","#111827"
    ];

    /// <summary>
    /// Represents the path to the JavaScript module associated with the color palette component.
    /// </summary>
    private const string JavaScriptModulePath = "./_content/FluentUI.Blazor.Community.Components/Components/ColorPalette/FluentCxColorPalette.razor.js";

    /// <summary>
    /// Represents the javascript module reference for the color palette component.
    /// </summary>
    private IJSObjectReference? _jsModule;

    /// <summary>
    /// Represents a unique identifier for the color palette instance.
    /// </summary>
    private ElementReference[] _buttonsRef = [];

    /// <summary>
    /// Represents the list of harmony modes available for color generation.
    /// </summary>
    private static readonly IReadOnlyList<string> HarmonyModes = new ReadOnlyCollection<string>(
        [
            nameof(ColorPaletteMode.Complementary),
            nameof(ColorPaletteMode.Analogous),
            nameof(ColorPaletteMode.Triadic),
            nameof(ColorPaletteMode.SplitComplementary),
            nameof(ColorPaletteMode.Monochrome),
        ]);

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

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxColorPalette"/> class.
    /// </summary>
    /// <remarks>This constructor generates a unique identifier for the instance using <see
    /// cref="Identifier.NewId"/>.</remarks>
    public FluentCxColorPalette()
    {
        Id = Identifier.NewId();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets or sets the global state service for localization and directionality.
    /// </summary>
    [Inject]
    private GlobalState GlobalState { get; set; } = default!;

    /// <summary>
    /// Gets or sets the JavaScript runtime for interop calls.
    /// </summary>
    [Inject]
    private IJSRuntime Runtime { get; set; } = default!;

    /// <summary>
    /// Gets or sets the number of columns in the color grid layout. Default is 10.
    /// </summary>
    [Parameter]
    public int Columns { get; set; } = 10;

    /// <summary>
    /// Gets or sets the size of each color item in the grid, in pixels. Default is 28.
    /// </summary>
    [Parameter]
    public int ItemSize { get; set; } = 28;

    /// <summary>
    /// Gets or sets the gap between grid items, in pixels. Default is 6.
    /// </summary>
    [Parameter]
    public int GridGap { get; set; } = 6;

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
    /// Gets or sets a function that provides the data of an image to extract colors from when the mode is set to <see cref="ColorPaletteMode.FromImage"/>.
    /// </summary>
    [Parameter]
    public Func<Task<byte[]>>? ImageDataProvider { get; set; }

    /// <summary>
    /// Gets the computed number of rows in the color grid based on the number of colors and columns.
    /// </summary>
    private int ComputedRows => Math.Max(1, (int)Math.Ceiling((double)_colors.Count / Columns));

    /// <summary>
    /// Gets a set of selected colors for efficient lookup.
    /// </summary>
    private HashSet<string> SelectedColorsSet => new(SelectedColors, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Gets or sets the harmony mode as a string for binding purposes.
    /// </summary>
    private string HarmonyModeString
    {
        get => Mode.ToString();
        set
        {
            if (Enum.TryParse<ColorPaletteMode>(value, out var mode))
            {
                Mode = mode;
                _ = GenerateColorsAsync();
            }
        }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Generates the color palette based on the current settings and parameters.
    /// </summary>
    /// <returns>Returns a task which generates the colors of the palette when completed.</returns>
    private async Task GenerateColorsAsync()
    {
        _errorMessage = string.Empty;
        List<string> generated;

        try
        {
            if (!string.IsNullOrWhiteSpace(SelectedPreset) &&
                Presets is not null &&
                Presets.TryGetValue(SelectedPreset, out var presetList))
            {
                generated = [.. presetList];
            }
            else
            {
                switch (Mode)
                {
                    case ColorPaletteMode.Provided:
                        {
                            generated = [.. ProvidedColors ?? DefaultProvided];
                        }

                        break;

                    case ColorPaletteMode.Random:
                        {
                            generated = ColorUtils.GenerateRandomHex(Math.Min(MaxColors, GradientSteps));
                        }

                        break;

                    case ColorPaletteMode.Gradient:
                        {
                            generated = ColorUtils.GenerateGradient(BaseColor, Math.Min(MaxColors, GradientSteps), GradientStrategy, GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.CustomGradient:
                        {
                            if (!string.IsNullOrWhiteSpace(GradientStart) && !string.IsNullOrWhiteSpace(GradientEnd))
                            {
                                generated = ColorUtils.GenerateCustomGradient(GradientStart, GradientEnd, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                            }
                            else
                            {
                                throw new InvalidOperationException("GradientStart et GradientEnd sont requis en mode CustomGradient.");
                            }
                        }

                        break;

                    case ColorPaletteMode.FromImage:
                        {
                            if (ImageDataProvider is not null)
                            {
                                var data = await ImageDataProvider();
                                using var stream = new MemoryStream(data);
                                generated = ImagePalette.ExtractColorsFromImage(stream, Math.Min(MaxColors, GradientSteps));
                            }
                            else
                            {
                                throw new InvalidOperationException("ImageDataProvider was not provided for FromImage mode.");
                            }
                        }

                        break;

                    case ColorPaletteMode.Complementary:
                        {
                            generated = new ComplementaryPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Analogous:
                        {
                            generated = new AnalogousPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Triadic:
                        {
                            generated = new TriadicPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Tetradic:
                        {
                            generated = new TetradicPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.SplitComplementary:
                        {
                            generated = new SplitComplementaryPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Monochrome:
                        {
                            generated = new MonochromaticPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Warm:
                        {
                            generated = new WarmPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Cool:
                        {
                            generated = new CoolPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Pastel:
                        {
                            generated = new PastelPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Neon:
                        {
                            generated = new NeonPlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.Greyscale:
                        {
                            generated = new GrayscalePlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.AccessibilitySafe:
                        {
                            generated = new AccessibilitySafePlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    case ColorPaletteMode.None:
                        {
                            generated = [];
                        }

                        break;

                    case ColorPaletteMode.Desaturate:
                        {
                            generated = new DesaturatePlugin().Generate(BaseColor, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;

                    default:
                        {
                            generated = ColorUtils.GenerateScheme(BaseColor, Mode, Math.Min(MaxColors, GradientSteps), GenerationOptions);
                        }

                        break;
                }

                if (Plugins is not null &&
                    Plugins.Count > 0)
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

    /// <summary>
    /// Sets focus to the button at the specified index.
    /// </summary>
    /// <param name="index">Index of the button to focus.</param>
    /// <returns>Returns a task which focus the button at the specified index when completed.</returns>
    private async Task SetFocusAsync(int index)
    {
        _focusIndex = index;
        await InvokeAsync(StateHasChanged);

                    if (_jsModule is not null)
            {
                await _jsModule.InvokeVoidAsync("blurAll", Id);
            }

        if (index >= 0 &&
            index < _buttonsRef.Length)
        {
            await _buttonsRef[index].FocusAsync();
        }
    }

    /// <summary>
    /// Manages keyboard navigation within the color grid.
    /// </summary>
    /// <param name="e">Event args associated to this instance.</param>
    /// <returns>Returns a task which navigates withing the color grid when completed.</returns>
    private async Task OnGridKeyDownAsync(FluentKeyCodeEventArgs e)
    {
        if (_colors.Count == 0)
        {
            return;
        }

        var next = _focusIndex;

        switch (e.Key)
        {
            case KeyCode.Left:
                next = GlobalState.Dir== LocalizationDirection.RightToLeft ? MoveRight(_focusIndex) : MoveLeft(_focusIndex);
                break;

            case KeyCode.Right:
                next = GlobalState.Dir == LocalizationDirection.RightToLeft ? MoveLeft(_focusIndex) : MoveRight(_focusIndex);
                break;

            case KeyCode.Up:
                next = MoveUp(_focusIndex);
                break;

            case KeyCode.Down:
                next = MoveDown(_focusIndex);
                break;

            case KeyCode.Home:
                next = (_focusIndex / Columns) * Columns;
                break;

            case KeyCode.End:
                next = Math.Min((_focusIndex / Columns) * Columns + (Columns - 1), _colors.Count - 1);
                break;

            case KeyCode.PageUp:
                next = 0;
                break;

            case KeyCode.PageDown:
                next = _colors.Count - 1;
                break;

            case KeyCode.Enter:
            case KeyCode.Space:
                await ToggleSelectAsync(_colors[_focusIndex], _focusIndex);
                return;

            default:
                return;
        }

        next = (next + _colors.Count) % _colors.Count;

        await SetFocusAsync(next);
    }

    /// <summary>
    /// Moves the focus to the left in the color grid, wrapping around if necessary.
    /// </summary>
    /// <param name="index">Current index of the focus.</param>
    /// <returns>Returns the new index of the focus.</returns>
    private int MoveLeft(int index)
    {
        return index % Columns == 0
            ? Math.Min(index + (Columns - 1), _colors.Count - 1)
            : index - 1;
    }

    /// <summary>
    /// Moves the focus to the right in the color grid, wrapping around if necessary.
    /// </summary>
    /// <param name="index">Current index of the focus.</param>
    /// <returns>Returns the new index of the focus.</returns>
    private int MoveRight(int index)
    {
        return (index % Columns == Columns - 1 || index + 1 >= _colors.Count)
                    ? index - (Columns - 1)
                    : index + 1;
    }

    /// <summary>
    /// Moves the focus up in the color grid, wrapping around if necessary.
    /// </summary>
    /// <param name="index">Current index of the focus.</param>
    /// <returns>Returns the new index of the focus.</returns>
    private int MoveUp(int index)
    {
        return index - Columns < 0
                    ? Math.Min((ComputedRows - 1) * Columns + (index % Columns), _colors.Count - 1)
                    : index - Columns;
    }

    /// <summary>
    /// Moves the focus down in the color grid, wrapping around if necessary.
    /// </summary>
    /// <param name="index">Current index of the focus.</param>
    /// <returns>Returns the new index of the focus.</returns>
    private int MoveDown(int index)
    {
        return index + Columns >= _colors.Count
           ? index % Columns
           : index + Columns;
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await GenerateColorsAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            _jsModule ??= await Runtime.InvokeAsync<IJSObjectReference>("import", JavaScriptModulePath);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
                _jsModule = null;
            }
        }
        catch (JSDisconnectedException)
        {
        }
    }

    #endregion Methods
}

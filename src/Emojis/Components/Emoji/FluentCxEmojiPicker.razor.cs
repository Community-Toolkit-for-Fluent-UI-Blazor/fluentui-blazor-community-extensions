using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace FluentUI.Blazor.Community.Components.Emojis;

/// <summary>
/// Represents a component that allows users to explore and select emojis from the Fluent Cx UI emoji set. 
/// </summary>
public partial class FluentCxEmojiPicker : FluentComponentBase
{
    private sealed class EmojiRow
    {
        public required IReadOnlyList<FluentCxEmoji> Items { get; init; }
    }

    private readonly List<EmojiRow> _rows = [];
    private bool _isCultureChanged;
    private Virtualize<EmojiRow>? _virtualize;
    private readonly EmojiStore _emojiStore = new();
    private static readonly Dictionary<string, Emoji> s_cachedEmojis = new()
    {
        ["Smileys & Emotion"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.SmileysEmotion.Color.Default.GrinningFace(),
        ["Animals & Nature"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.AnimalsNature.Color.Default.DogFace(),
        ["Food & Drink"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.FoodDrink.Color.Default.Hamburger(),
        ["Activities"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.Activities.Color.Default.Baseball(),
        ["Travel & Places"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.TravelPlaces.Color.Default.Airplane(),
        ["Objects"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.Objects.Color.Default.LightBulb(),
        ["Symbols"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.SmileysEmotion.Color.Default.RedHeart(),
        ["Flags"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.Flags.Color.Default.RainbowFlag(),
        ["People & Body"] = new Microsoft.FluentUI.AspNetCore.Components.Emojis.PeopleBody.Color.Medium.Artist(),
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentCxEmojiPicker"/> class.
    /// </summary>
    /// <param name="configuration">The library configuration to use.</param>
    public FluentCxEmojiPicker(LibraryConfiguration configuration)
        : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the culture to use for loading emoji data and performing searches.
    /// </summary>
    [Parameter]
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    /// <summary>
    /// Gets or sets the emoji font provider that specifies the font family to use for rendering emojis.
    /// </summary>
    [Parameter]
    public IEmojiFontProvider EmojiFontProvider { get; set; } = new MicrosoftEmojiProvider();

    /// <summary>
    /// Gets the CSS font-family string to use for rendering emojis, combining the specified emoji font family and fallback font family.
    /// </summary>
    private string FontFamily => $"{EmojiFontProvider.FontFamily}, {EmojiFontProvider.FallbackFontFamily}";

    /// <summary>
    /// Gets the internal CSS style string for the component.
    /// </summary>
    private string? InternalStyle => new StyleBuilder()
        .AddStyle("font-family", FontFamily)
        .AddStyle("font-size", "32px")
        .Build();

    private string? InternalFontFamilyStyle => new StyleBuilder()
        .AddStyle("font-family", FontFamily)
        .Build();

    /// <summary>
    /// Gets the internal CSS class for the component.
    /// </summary>
    private string? InternalClass => DefaultClassBuilder.
        AddClass("fluent-cx-emoji-explorer")
        .Build();

    /// <summary>
    /// Gets or sets the search text entered by the user for filtering emojis.
    /// </summary>
    private string SearchText { get; set; } = "";

    /// <summary>
    /// Gets or sets the currently selected emoji category.
    /// </summary>
    private string SelectedCategory { get; set; } = "";

    /// <summary>
    /// Gets or sets the list of emojis that are currently visible based on the selected category and search text.
    /// </summary>
    private List<FluentCxEmoji> VisibleEmojis { get; set; } = [];

    /// <summary>
    /// Gets or sets the event callback that is invoked when the selected emoji changes.
    /// </summary>
    [Parameter]
    public EventCallback<FluentCxEmoji> OnEmojiChanged { get; set; }

    /// <summary>
    /// Gets or sets the number of emojis to display per row in the emoji grid.
    /// </summary>
    [Parameter]
    public int EmojisPerRow { get; set; } = 8;

    private FluentCxEmoji? SelectedEmoji { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await LoadEmojiFromCultureAsync();
    }

    private async Task RebuildRowsAsync()
    {
        _rows.Clear();

        for (var i = 0; i < VisibleEmojis.Count; i += EmojisPerRow)
        {
            var slice = VisibleEmojis
                .Skip(i)
                .Take(EmojisPerRow)
                .ToList();

            _rows.Add(new EmojiRow { Items = slice });
        }

        if (_virtualize is not null)
        {
            await _virtualize.RefreshDataAsync();
        }
    }

    private async Task LoadEmojiFromCultureAsync()
    {
        await _emojiStore.LoadLanguageAsync(Culture.TwoLetterISOLanguageName);

        SelectedCategory = _emojiStore.Categories.Keys.FirstOrDefault() ?? "";
        UpdateVisibleEmojis();
        await RebuildRowsAsync();
    }

    private async Task OnSearchChangedAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            UpdateVisibleEmojis();
            await RebuildRowsAsync();
            return;
        }

        VisibleEmojis = [.. _emojiStore.Search(SearchText)];
        await RebuildRowsAsync();
    }

    private async Task SelectCategoryAsync(string cat)
    {
        SelectedCategory = cat;
        SearchText = "";
        SelectedEmoji = null;
        UpdateVisibleEmojis();
        await RebuildRowsAsync();
    }

    private void UpdateVisibleEmojis()
    {
        if (string.IsNullOrWhiteSpace(SelectedCategory))
        {
            VisibleEmojis = [.. _emojiStore.All];
            return;
        }

        VisibleEmojis = _emojiStore.Categories.TryGetValue(SelectedCategory, out var list) ? list : [];
    }

    private async Task SelectEmojiAsync(FluentCxEmoji emoji)
    {
        if (emoji is null)
        {
            return;
        }

        SelectedEmoji = emoji;

        if (OnEmojiChanged.HasDelegate)
        {
            await OnEmojiChanged.InvokeAsync(emoji);
        }
    }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _isCultureChanged = parameters.TryGetValue<CultureInfo>(nameof(Culture), out var newCulture) && newCulture.Name != Culture.Name;

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (_isCultureChanged)
        {
            await LoadEmojiFromCultureAsync();
            _isCultureChanged = false;
        }

        await base.OnParametersSetAsync();
    }

    private ValueTask<ItemsProviderResult<EmojiRow>> LoadEmojisAsync(ItemsProviderRequest request)
    {
        var items = _rows.Skip(request.StartIndex).Take(request.Count).ToList();

        return ValueTask.FromResult(new ItemsProviderResult<EmojiRow>(items, _rows.Count));
    }

    private static Emoji GetEmojiFromCategory(string category)
    {
        if (s_cachedEmojis.TryGetValue(category, out var value))
        {
            return value;
        }

        throw new NotSupportedException($"Unsupported category: {category}");
    }
}

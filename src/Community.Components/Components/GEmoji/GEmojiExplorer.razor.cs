using GEmojiSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

public partial class GEmojiExplorer
    : FluentComponentBase
{
    private static readonly List<string> _categories = [
     "Smileys & Emotion",
     "People & Body",
     "Animals & Nature",
     "Food & Drink",
     "Travel & Places",
     "Activities",
     "Objects",
     "Symbols",
     "Flags"
    ];

    private readonly Dictionary<string, List<GEmoji>> _emojiByCategory = [];
    private Virtualize<GEmoji>? _virtualizeList;
    private string? _selectedGroup;
    private List<GEmoji> _selectedEmojis = [];
    private bool _isSearchSelected;
    private string? _value;
    private readonly RenderFragment<GEmoji> _renderEmoji;
    private readonly List<GEmoji> _selectedItems = [];

    [Parameter]
    public string? MaxHeight { get; set; }

    [Parameter]
    public GEmojiLabels Labels { get; set; } = GEmojiLabels.Default;

    [Parameter]
    public string? SelectedEmoji { get; set; }

    [Parameter]
    public EventCallback<string?> SelectedEmojiChanged { get; set; }

    [Parameter]
    public GEmojiProviderDelegate? ItemsProvider { get; set; }

    private ValueTask<ItemsProviderResult<GEmoji>> GetItemsAsync(ItemsProviderRequest request)
    {
        int totalCount = _selectedEmojis.Count;
        var items = _selectedEmojis.Skip(request.StartIndex).Take(request.Count).ToList();

        return ValueTask.FromResult(new ItemsProviderResult<GEmoji>(items, totalCount));
    }

    private static Microsoft.FluentUI.AspNetCore.Components.Emoji GetEmoji(string category)
    {
        return category switch
        {
            "Food & Drink" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.FoodDrink.Flat.Default.TropicalDrink(),
            "Smileys & Emotion" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.SmileysEmotion.Flat.Default.SlightlySmilingFace(),
            "People & Body" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.PeopleBody.Flat.Default.Artist(),
            "Animals & Nature" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.AnimalsNature.Flat.Default.Bear(),
            "Travel & Places" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.TravelPlaces.Flat.Default.GlobeShowingEuropeAfrica(),
            "Activities" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.Activities.Flat.Default.Trophy(),
            "Objects" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.Objects.Flat.Default.LightBulb(),
            "Symbols" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.Symbols.Flat.Default.InputSymbols(),
            "Flags" => new Microsoft.FluentUI.AspNetCore.Components.Emojis.Flags.Flat.Default.TriangularFlag(),
            _ => throw new InvalidOperationException()
        };
    }

    private async Task OnEmojiSelectedAsync(string? context)
    {
        SelectedEmoji = context;

        if (SelectedEmojiChanged.HasDelegate)
        {
            await SelectedEmojiChanged.InvokeAsync(context);
        }
    }

    private async Task HandleSearchInputAsync()
    {
        if (string.IsNullOrWhiteSpace(_value))
        {
            _value = string.Empty;

            if (!string.IsNullOrEmpty(_selectedGroup))
            {
                _selectedItems.Clear();
                _selectedEmojis = _emojiByCategory[_selectedGroup];
            }
        }
        else
        {
            var searchTerm = _value.ToLower();

            if (searchTerm.Length > 0)
            {
                var e = new GEmojiProviderRequest(searchTerm);

                if (ItemsProvider is not null)
                {
                    await ItemsProvider(e);
                    _selectedItems.Clear();
                    _selectedItems.AddRange(e.Items);
                }
            }
        }
    }

    private async Task OnLoadItemsAsync(string? item)
    {
        if (string.IsNullOrEmpty(item))
        {
            return;
        }

        _isSearchSelected = false;
        _selectedGroup = item;
        _selectedEmojis = _emojiByCategory[_selectedGroup];
        _value = string.Empty;

        if (_virtualizeList is not null)
        {
            await _virtualizeList.RefreshDataAsync();
            await InvokeAsync(StateHasChanged);
        }
    }

    private void OnSearchVisible()
    {
        _isSearchSelected = true;
        _selectedGroup = string.Empty;
        _selectedEmojis.Clear();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await OnLoadItemsAsync(_categories.FirstOrDefault());
    }
}

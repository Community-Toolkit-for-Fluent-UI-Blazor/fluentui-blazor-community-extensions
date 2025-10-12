using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Gets or sets the video label information and its associated video track item to be displayed in the panel.
/// </summary>
public partial class VideoQualityPanel
{
    /// <summary>
    /// Represents the list of available quality options for the video.
    /// </summary>
    private readonly List<Option<int>> _qualityOptions = [];

    /// <summary>
    /// Represents the index of the currently selected quality option.
    /// </summary>
    private Option<int>? _selectedQualityIndex;

    /// <summary>
    /// Gets or sets the video label information along with the associated video track item.
    /// </summary>
    [Parameter]
    public (AudioVideoLabels, VideoTrackItem) Content { get; set; }

    /// <summary>
    /// Gets or sets the video state.
    /// </summary>
    [Inject]
    private VideoState VideoState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the dialog instance that hosts this panel.
    /// </summary>
    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();
        _qualityOptions.Clear();

        _qualityOptions.Add(new Option<int>()
        {
            Value = -1,
            Text = Content.Item1.DefaultLabel,
            Selected = VideoState.SelectedQuality == -1
        });

        foreach(var source in Content.Item2.Sources.OrderByDescending(x => x.Quality))
        {
            _qualityOptions.Add(new Option<int>()
            {
                Value = source.Quality,
                Text = $"{source.Quality}p",
                Selected = VideoState.SelectedQuality == source.Quality,
                Icon = source.IsHighDefinition ?  (new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.Hd(), Color.Accent, "start") : null
            });
        }

        _selectedQualityIndex = _qualityOptions.FirstOrDefault(x => x.Selected);
    }

    /// <summary>
    /// Performs cleanup and closes the dialog asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    private async Task OnCloseAsync()
    {
        VideoState.SelectedQuality = _selectedQualityIndex!.Value;
        await Dialog.CloseAsync();
    }

    /// <summary>
    /// Asynchronously cancels the current dialog operation.
    /// </summary>
    /// <returns>A task that represents the asynchronous cancel operation.</returns>
    private async Task OnCancelAsync()
    {
        await Dialog.CancelAsync();
    }
}

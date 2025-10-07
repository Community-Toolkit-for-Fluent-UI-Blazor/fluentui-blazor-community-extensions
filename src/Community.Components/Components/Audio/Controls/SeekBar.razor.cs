using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a seek bar component for audio or video playback, allowing users to navigate through the media timeline.
/// </summary>
public partial class SeekBar : FluentComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Variable to track if the user is currently dragging the seek bar thumb.
    /// </summary>
    private bool _isDragging;

    /// <summary>
    /// Represents a cancellation token source used for hiding the preview.
    /// </summary>
    private CancellationTokenSource? _cts;

    /// <summary>
    /// Indicates whether the preview is displayed.
    /// </summary>
    private bool _showPreview;

    /// <summary>
    /// Represents the preview time in seconds.
    /// </summary>
    private double _previewTime;

    /// <summary>
    /// Represents the width of the container.
    /// </summary>
    private double _containerWidth = 1;

    /// <summary>
    /// Represents a reference to a .NET object of type <see cref="SeekBar"/> that can be passed to JavaScript interop.
    /// </summary>
    /// <remarks>This field is used to enable JavaScript to invoke methods on the associated <see
    /// cref="SeekBar"/> instance. It is a readonly field and should be initialized with a valid <see
    /// cref="DotNetObjectReference{T}"/> instance.</remarks>
    private readonly DotNetObjectReference<SeekBar> _dotNetRef;

    /// <summary>
    /// Represents a reference to a JavaScript module that can be used for invoking JavaScript functions.
    /// </summary>
    /// <remarks>This field holds an instance of <see cref="IJSObjectReference"/> that represents a JavaScript
    /// module. It is nullable, indicating that the module may not be initialized. Ensure the module is initialized
    /// before attempting to use it.</remarks>
    private IJSObjectReference? _module;

    /// <summary>
    /// Initializes a new instance of the <see cref="SeekBar"/> class.
    /// </summary>
    public SeekBar()
    {
        Id = $"seekbar-{Identifier.NewId()}";
        _dotNetRef = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Gets or sets the duration of the track, in seconds.
    /// </summary>
    [Parameter]
    public double Duration { get; set; }

    /// <summary>
    /// Gets or sets the current playback time of the media, in seconds.
    /// </summary>
    [Parameter]
    public double CurrentTime { get; set; }

    /// <summary>
    /// Gets or sets the collection of chapters associated with the current context.
    /// </summary>
    [Parameter]
    public List<Chapter> Chapters { get; set; } = [];

    /// <summary>
    /// Gets or sets the media mode for the seek bar.
    /// </summary>
    [Parameter]
    public MediaMode Mode { get; set; } = MediaMode.Audio;

    /// <summary>
    /// Gets or sets a function that generates a thumbnail URL based on a given timeline.
    /// </summary>
    [Parameter]
    public Func<double, string>? ThumbnailProvider { get; set; }

    /// <summary>
    /// Gets or sets the step value used to increment or decrement the associated value.
    /// </summary>
    [Parameter]
    public double Step { get; set; } = 5;

    /// <summary>
    /// Gets or sets the callback that is invoked when a seek operation occurs.
    /// </summary>
    [Parameter]
    public EventCallback<double> OnSeek { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a seek operation starts.
    /// </summary>
    [Parameter]
    public EventCallback<double> OnSeekStart { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when a seek operation is completed.
    /// </summary>
    [Parameter]
    public EventCallback<double> OnSeekEnd { get; set; }

    /// <summary>
    /// Gets or sets the callback that is invoked when the chapter changes.
    /// </summary>
    [Parameter]
    public EventCallback<Chapter> OnChapterChanged { get; set; }

    /// <summary>
    /// Gets or sets the JavaScript runtime instance used for invoking JavaScript functions from .NET.
    /// </summary>
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Gets the progression percentage of the current time relative to the duration.
    /// </summary>
    private string Progression => string.Format(CultureInfo.InvariantCulture, "{0}%", CurrentTime / Duration * 100);

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _module = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/FluentUI.Blazor.Community.Components/Components/Audio/Controls/SeekBar.razor.js");
            await _module.InvokeVoidAsync("fluentCxSeekBar.initialize", Id, _dotNetRef);
            await MeasureWidthAsync();
        }
    }

    /// <summary>
    /// Invoked by JavaScript to handle resize events and update the component's state accordingly.
    /// </summary>
    /// <remarks>This method is intended to be called from JavaScript via the <see
    /// cref="JSInvokableAttribute"/> with the identifier "onResize". It triggers an asynchronous operation to measure
    /// the width of the component and update its state.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [JSInvokable("onResize")]
    public async Task OnResizeAsync() => await MeasureWidthAsync();

    /// <summary>
    /// Asynchronously measures the width of the container associated with the current instance.
    /// </summary>
    /// <remarks>This method invokes a JavaScript function to retrieve the width of the container element
    /// identified by the <c>Id</c> property. Ensure that the JavaScript module is properly initialized before calling
    /// this method.</remarks>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task MeasureWidthAsync()
    {
        if (_module is not null)
        {
            _containerWidth = await _module.InvokeAsync<double>("fluentCxSeekBar.getWidth", Id);
        }
    }

    /// <summary>
    /// Initiates a drag operation based on the specified pointer event.
    /// </summary>
    /// <remarks>This method sets the internal dragging state and triggers the <see cref="OnSeekStart"/> event
    /// if it has subscribers.  It also updates the seek position based on the pointer's client X-coordinate.</remarks>
    /// <param name="e">The pointer event arguments containing details about the pointer event, such as the client X-coordinate.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task StartDragAsync(PointerEventArgs e)
    {
        _isDragging = true;

        if (OnSeekStart.HasDelegate)
        {
            await OnSeekStart.InvokeAsync(CurrentTime);
        }

        await UpdateSeekFromClientXAsync(e.ClientX);
    }

    /// <summary>
    /// Handles the drag event and updates the seek position based on the pointer's horizontal position.
    /// </summary>
    /// <param name="e">The pointer event arguments containing information about the drag event, including the pointer's position.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnDragAsync(PointerEventArgs e)
    {
        if (_isDragging)
        {
            await UpdateSeekFromClientXAsync(e.ClientX);
        }
    }

    /// <summary>
    /// Ends the drag operation and triggers the seek end event if applicable.
    /// </summary>
    /// <remarks>This method is called to finalize a drag operation. If the drag operation was active, it
    /// stops the drag,  hides the preview, and invokes the <see cref="OnSeekEnd"/> event with the current time
    /// value.</remarks>
    /// <param name="e">The pointer event arguments associated with the drag operation.</param>
    /// <returns></returns>
    private async Task EndDragAsync(PointerEventArgs e)
    {
        if (_isDragging)
        {
            _isDragging = false;
            _showPreview = false;

            if (OnSeekEnd.HasDelegate)
            {
                await OnSeekEnd.InvokeAsync(CurrentTime);
            }

            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Initiates the touch interaction for seeking functionality.
    /// </summary>
    /// <remarks>This method sets the dragging state to active and triggers the seek start event with the
    /// current time. It then updates the seek position based on the horizontal client coordinate of the first touch
    /// point.</remarks>
    /// <param name="e">The touch event arguments containing details about the touch interaction. The first touch point is used to
    /// determine the starting position.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task StartTouchAsync(TouchEventArgs e)
    {
        _isDragging = true;
        await OnSeekStart.InvokeAsync(CurrentTime);
        var touch = e.Touches[0];
        await UpdateSeekFromClientXAsync(touch.ClientX);
    }

    /// <summary>
    /// Handles the touch move event and updates the seek position based on the touch's client X-coordinate.
    /// </summary>
    /// <remarks>This method is invoked during a touch move gesture when dragging is active. It processes the
    /// first touch point and updates the seek position accordingly. Ensure that dragging is enabled before invoking
    /// this method.</remarks>
    /// <param name="e">The touch event arguments containing information about the touch gesture.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnTouchMoveAsync(TouchEventArgs e)
    {
        if (_isDragging)
        {
            var touch = e.Touches[0];
            await UpdateSeekFromClientXAsync(touch.ClientX);
        }
    }

    /// <summary>
    /// Ends the current touch interaction, finalizing any ongoing drag operation.
    /// </summary>
    /// <remarks>This method is typically called when a touch interaction is completed. If a drag operation 
    /// was in progress, it stops the drag and hides the preview. Additionally, it triggers the  <see cref="OnSeekEnd"/>
    /// event with the current time value.</remarks>
    /// <param name="e">The <see cref="TouchEventArgs"/> containing details about the touch event.</param>
    /// <returns></returns>
    private async Task EndTouchAsync(TouchEventArgs e)
    {
        if (_isDragging)
        {
            _isDragging = false;
            _showPreview = false;
            await OnSeekEnd.InvokeAsync(CurrentTime);

            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Handles key press events to perform seek operations in a media playback context.
    /// </summary>
    /// <remarks>This method processes specific key inputs to adjust the playback position: <list
    /// type="bullet"> <item><description><see cref="KeyCode.Left"/> seeks backward by a predefined
    /// step.</description></item> <item><description><see cref="KeyCode.Right"/> seeks forward by a predefined
    /// step.</description></item> <item><description><see cref="KeyCode.Home"/> seeks to the beginning of the
    /// media.</description></item> <item><description><see cref="KeyCode.End"/> seeks to the end of the
    /// media.</description></item> </list></remarks>
    /// <param name="e">The key event arguments containing information about the key pressed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OnKeyDownAsync(FluentKeyCodeEventArgs e)
    {
        switch(e.Key)
        {
            case KeyCode.Left:
                await SeekToAsync(CurrentTime - Step);
                break;

            case KeyCode.Right:
                await SeekToAsync(CurrentTime + Step);
                break;

            case KeyCode.Home:
                await SeekToAsync(0);
                break;

            case KeyCode.End:
                await SeekToAsync(Duration);
                break;
        }
    }

    /// <summary>
    /// Updates the seek position of the media playback based on the specified horizontal offset.
    /// </summary>
    /// <remarks>The method calculates the seek position as a percentage of the total duration based on the
    /// provided offset and the width of the container. The seek operation is performed asynchronously and may include a
    /// preview of the media at the calculated position.</remarks>
    /// <param name="offsetX">The horizontal offset, in pixels, used to calculate the new seek position. Must be a value between 0 and the
    /// width of the container.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateSeekAsync(double offsetX)
    {
        var percent = Math.Clamp(offsetX / _containerWidth, 0, 1);
        await SeekToAsync(percent * Duration, preview: true);
    }

    /// <summary>
    /// Updates the seek position based on the specified client X-coordinate.
    /// </summary>
    /// <remarks>This method calculates the offset relative to the seek bar using the provided client
    /// X-coordinate and updates the seek position accordingly. If the module is not initialized, the method exits
    /// without performing any action.</remarks>
    /// <param name="clientX">The X-coordinate, in pixels, from the client that determines the seek position.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task UpdateSeekFromClientXAsync(double clientX)
    {
        if (_module is null)
        {
            return;
        }

        var offset = await _module.InvokeAsync<double>("fluentCxSeekBar.getOffsetX", Id, clientX);
        await UpdateSeekAsync(offset);
    }

    /// <summary>
    /// Seeks to the specified time within the media and optionally enables preview mode.
    /// </summary>
    /// <remarks>This method updates the current playback time and triggers the <see cref="OnSeek"/> event if
    /// it has subscribers.  If the seek operation results in a chapter change, the <see cref="OnChapterChanged"/> event
    /// is also triggered.</remarks>
    /// <param name="time">The target time, in seconds, to seek to. The value is clamped between 0 and the total duration of the media.</param>
    /// <param name="preview">A boolean value indicating whether to enable preview mode.  If <see langword="true"/>, the preview state is
    /// updated without committing the seek operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task SeekToAsync(double time, bool preview = false)
    {
        CurrentTime = Math.Clamp(time, 0, Duration);
        _previewTime = CurrentTime;
        _showPreview = preview;

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        if (OnSeek.HasDelegate)
        {
            await OnSeek.InvokeAsync(CurrentTime);
        }

        var chapter = Chapters.FirstOrDefault(c => c.Start <= CurrentTime && CurrentTime < c.End);

        if (chapter is not null && OnChapterChanged.HasDelegate)
        {
            await OnChapterChanged.InvokeAsync(chapter);
        }

        _ = HidePreviewAfterDelayAsync(_cts.Token);
    }

    /// <summary>
    /// Hides the preview after a delay of 2 seconds.
    /// </summary>
    /// <remarks>If the operation is canceled via the provided <paramref name="token"/>, the preview will not
    /// be hidden.</remarks>
    /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the delay operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task HidePreviewAfterDelayAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(2000, token); 
            _showPreview = false;
            await InvokeAsync(StateHasChanged);
        }
        catch (TaskCanceledException)
        {
        }
    }

    /// <summary>
    /// Handles the click event on the seek bar and updates the seek position based on the mouse click location.
    /// </summary>
    /// <remarks>This method does not perform any action if a drag operation is currently in
    /// progress.</remarks>
    /// <param name="e">The <see cref="MouseEventArgs"/> containing information about the mouse click event, including the client
    /// X-coordinate.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task OnClickSeek(MouseEventArgs e)
    {
        if (_isDragging)
        {
            return;
        }

        await UpdateSeekFromClientXAsync(e.ClientX);
    }

    /// <summary>
    /// Determines the color representation for a chapter based on its status.
    /// </summary>
    /// <param name="status">The status of the chapter. Must be one of the <see cref="ChapterStatus"/> enumeration values.</param>
    /// <returns>A CSS linear-gradient string representing the color associated with the specified chapter status. Returns "gray"
    /// if the status is not recognized.</returns>
    private static string GetChapterColor(ChapterStatus status) => status switch
    {
        ChapterStatus.Completed => "linear-gradient(90deg, #87d068, #5cb85c)",
        ChapterStatus.Current => "linear-gradient(90deg, #ffd773, #ffb900)",
        ChapterStatus.NotStarted => "linear-gradient(90deg, #ff8a8a, #e81123)",
        _ => "gray"
    };

    /// <summary>
    /// Formats a time value in seconds into a string representation in the format "mm:ss".
    /// </summary>
    /// <param name="seconds">Number of elapsed seconds.</param>
    /// <returns>Returns the formatted value.</returns>
    private static string FormatTime(double seconds) => TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss", CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        try
        {
            _dotNetRef.Dispose();

            if (_module is not null)
            {
                await _module.InvokeVoidAsync("fluentCxSeekBar.dispose", Id);
                await _module.DisposeAsync();
            }
        }
        catch(JSDisconnectedException)
        {
        }

        GC.SuppressFinalize(this);
    }
}

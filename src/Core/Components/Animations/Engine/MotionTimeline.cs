namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a timeline for managing and controlling the execution of multiple animation tracks as a coordinated
/// sequence.
/// </summary>
/// <remarks>The timeline provides methods to start, pause, resume, stop, and update the progress of animations.
/// It tracks the elapsed time and the state of the timeline, allowing for precise control over the animation flow. Use
/// the timeline to synchronize and monitor the completion of grouped animation tracks. The timeline is not thread-safe;
/// all operations should be performed on the same thread.</remarks>
public sealed class MotionTimeline
{
    /// <summary>
    /// Contains the collection of animation tracks managed by the current instance.
    /// </summary>
    private readonly List<IMotionTrack> _tracks = [];

    /// <summary>
    /// Gets the collection of animation tracks associated with the current instance.
    /// </summary>
    /// <remarks>The returned list is read-only and reflects the set of tracks managed by the instance. Use
    /// this property to enumerate or inspect the tracks, but modifications must be performed through the appropriate
    /// methods provided by the class.</remarks>
    public IReadOnlyList<IMotionTrack> Tracks => _tracks;

    /// <summary>
    /// Gets the total elapsed time measured by the timer.
    /// </summary>
    public TimeSpan Elapsed { get; private set; }

    /// <summary>
    /// Gets the current state of the timeline.
    /// </summary>
    /// <remarks>The state indicates whether the timeline is running, paused, or stopped. Use this property to
    /// determine the timeline's operational status before performing actions such as starting or stopping the
    /// timeline.</remarks>
    public MotionTimelineState State { get; private set; } = MotionTimelineState.Stopped;

    /// <summary>
    /// Gets a value indicating whether the timeline has reached the completed state.
    /// </summary>
    public bool IsCompleted => State == MotionTimelineState.Completed;

    /// <summary>
    /// Gets or sets the playback loop mode for the animation timeline.
    /// </summary>
    public MotionTimelineLoopMode LoopMode { get; set; } = MotionTimelineLoopMode.None;

    /// <summary>
    /// Gets or sets the number of times the loop should repeat.
    /// </summary>
    /// <remarks>A value of 0 indicates infinite looping when the loop mode is not set to None. Set this
    /// property to a positive integer to specify a finite number of repetitions.</remarks>
    public int LoopCount { get; set; }

    /// <summary>
    /// Gets the index of the current loop iteration.
    /// </summary>
    public int CurrentLoop { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the operation or sequence is performed in reverse order.
    /// </summary>
    public bool Reverse { get; private set; }

    /// <summary>
    /// Adds an animation track to the collection of tracks managed by the animation system.
    /// </summary>
    /// <param name="track">The animation track to add. Cannot be null.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="track"/> is null.</exception>
    public void AddTrack(IMotionTrack track)
    {
        ArgumentNullException.ThrowIfNull(track);

        _tracks.Add(track);
    }

    /// <summary>
    /// Removes all tracks from the collection.
    /// </summary>
    /// <remarks>Use this method to reset the collection to an empty state. After calling this method, the
    /// collection will contain no tracks.</remarks>
    public void ClearTracks()
    {
        _tracks.Clear();
    }

    /// <summary>
    /// Starts the timeline and resets the elapsed time to zero.
    /// </summary>
    /// <remarks>Call this method to begin or restart the timeline. The timeline state is set to running, and
    /// any previously elapsed time is cleared.</remarks>
    public void Start()
    {
        Elapsed = TimeSpan.Zero;
        CurrentLoop = 0;
        Reverse = false;
        State = MotionTimelineState.Running;
    }

    /// <summary>
    /// Pauses the timeline if it is currently running.
    /// </summary>
    /// <remarks>This method changes the timeline's state to paused. If the timeline is not running, calling
    /// this method has no effect.</remarks>
    public void Pause()
    {
        if (State == MotionTimelineState.Running)
        {
            State = MotionTimelineState.Paused;
        }
    }

    /// <summary>
    /// Resumes the timeline if it is currently paused.
    /// </summary>
    /// <remarks>This method transitions the timeline's state from paused to running. If the timeline is not
    /// paused, calling this method has no effect.</remarks>
    public void Resume()
    {
        if (State == MotionTimelineState.Paused)
        {
            State = MotionTimelineState.Running;
        }
    }

    /// <summary>
    /// Stops the timeline and resets the elapsed time to zero.
    /// </summary>
    /// <remarks>Call this method to halt any ongoing timeline activity and clear the elapsed duration. After
    /// calling this method, the timeline state will be set to stopped and the elapsed time will be reset. This method
    /// is typically used to ensure the timeline is in a known, inactive state before starting or restarting
    /// operations.</remarks>
    public void Stop()
    {
        State = MotionTimelineState.Stopped;
        Elapsed = TimeSpan.Zero;
    }

    /// <summary>
    /// deltaTime vient de ton AnimationRenderer (tick JS).
    /// </summary>
    public void Update(TimeSpan deltaTime)
    {
        if (State != MotionTimelineState.Running)
        {
            return;
        }

        Elapsed += deltaTime;

        var totalDuration = GetTotalDuration();

        if (Elapsed >= totalDuration)
        {
            switch (LoopMode)
            {
                case MotionTimelineLoopMode.None:
                    Elapsed = totalDuration;
                    UpdateTracks(Elapsed);
                    State = MotionTimelineState.Completed;
                    return;

                case MotionTimelineLoopMode.Repeat:
                    CurrentLoop++;

                    if (LoopCount > 0 && CurrentLoop >= LoopCount)
                    {
                        Elapsed = totalDuration;
                        UpdateTracks(Elapsed);
                        State = MotionTimelineState.Completed;
                        return;
                    }

                    Elapsed = TimeSpan.Zero;
                    break;

                case MotionTimelineLoopMode.Infinite:
                    Elapsed = TimeSpan.Zero;
                    break;

                case MotionTimelineLoopMode.PingPong:
                    CurrentLoop++;

                    if (LoopCount > 0 && CurrentLoop >= LoopCount)
                    {
                        Elapsed = totalDuration;
                        UpdateTracks(Elapsed);
                        State = MotionTimelineState.Completed;
                        return;
                    }

                    Reverse = !Reverse;
                    Elapsed = TimeSpan.Zero;
                    break;
            }
        }

        UpdateTracks(Elapsed);
    }

    /// <summary>
    /// Updates all tracks with the specified elapsed time, applying reverse logic if enabled.
    /// </summary>
    /// <remarks>If reverse mode is enabled, the tracks are updated using the reversed elapsed time, which may
    /// affect their progression direction. This method is typically used to synchronize track states with a time
    /// source.</remarks>
    /// <param name="elapsed">The amount of time that has elapsed since the last update. Used to advance or reverse the tracks' state.</param>
    private void UpdateTracks(TimeSpan elapsed)
    {
        foreach (var track in _tracks)
        {
            if (!Reverse)
            {
                track.Update(elapsed);
            }
            else
            {
                var total = track.Curve.Duration;
                var reversedTime = total - elapsed;

                if (reversedTime < TimeSpan.Zero)
                {
                    reversedTime = TimeSpan.Zero;
                }

                track.Update(reversedTime);
            }
        }
    }

    /// <summary>
    /// Calculates the total duration required for all tracks, including their delays and curve durations.
    /// </summary>
    /// <remarks>The total duration is determined by the track with the latest completion time, factoring in
    /// both its delay and curve duration. This method is useful for determining the overall timeline length when
    /// multiple tracks are scheduled.</remarks>
    /// <returns>A TimeSpan representing the maximum end time across all tracks. Returns TimeSpan.Zero if there are no tracks.</returns>
    internal TimeSpan GetTotalDuration()
    {
        if (_tracks.Count == 0)
        {
            return TimeSpan.Zero;
        }

        var max = TimeSpan.Zero;

        foreach (var track in _tracks)
        {
            var trackDuration = track.Delay + track.Curve.Duration;

            if (trackDuration > max)
            {
                max = trackDuration;
            }
        }

        return max;
    }
}

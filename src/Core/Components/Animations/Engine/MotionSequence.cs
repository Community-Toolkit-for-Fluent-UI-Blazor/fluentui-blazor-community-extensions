using FluentUI.Blazor.Community.Components.Extensions;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents a sequence of asynchronous motion steps that can be configured with various looping behaviors and
/// executed in order.
/// </summary>
/// <remarks>Use this class to build and execute a customizable sequence of motion steps, such as animations or
/// transitions, with support for repeat, infinite, and ping-pong looping modes. The sequence can be extended with
/// additional steps and started asynchronously. The Completed event is raised when all steps in the sequence have
/// finished executing. This class is sealed and cannot be inherited.</remarks>
public sealed class MotionSequence
{
    /// <summary>
    /// Represents the motion item associated with this instance.
    /// </summary>
    private readonly MotionItem _item;

    /// <summary>
    /// Represents the collection of asynchronous steps to be executed in sequence.
    /// </summary>
    private readonly List<Func<Task>> _steps = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="MotionSequence"/> class with the specified <see cref="MotionItem"/>.
    /// </summary>
    /// <param name="item">The motion item associated with this sequence, which provides the timeline for configuring looping behavior.</param>
    public MotionSequence(MotionItem item)
    {
        _item = item;
    }

    /// <summary>
    /// Occurs when the motion sequence has completed execution.
    /// </summary>
    /// <remarks>Subscribers can use this event to perform actions after the motion sequence finishes. The
    /// event provides details about the completed sequence through the MotionSequenceEventArgs parameter.</remarks>
    public event EventHandler<MotionSequenceEventArgs>? Completed;

    /// <summary>
    /// Adds an asynchronous step to the end of the motion sequence.
    /// </summary>
    /// <remarks>Each step is executed in the order it is added. Use this method to build complex sequences by
    /// chaining multiple asynchronous steps.</remarks>
    /// <param name="step">A function that returns a task representing the asynchronous operation to add as the next step in the sequence.
    /// Cannot be null.</param>
    /// <returns>The current instance of the motion sequence, enabling method chaining.</returns>
    public MotionSequence Then(Func<Task> step)
    {
        _steps.Add(step);

        return this;
    }

    /// <summary>
    /// Configures the motion sequence to repeat a specified number of times.
    /// </summary>
    /// <param name="count">The number of times the motion sequence should repeat. Must be greater than or equal to 1; values less than 1
    /// are treated as 1.</param>
    /// <returns>The current instance of the motion sequence with the repeat configuration applied.</returns>
    public MotionSequence WithRepeat(int count)
    {
        _item.Timeline.LoopMode = MotionTimelineLoopMode.Repeat;
        _item.Timeline.LoopCount = Math.Max(1, count);

        return this;
    }

    /// <summary>
    /// Configures the motion sequence to repeat indefinitely.
    /// </summary>
    /// <remarks>Use this method to set the motion sequence to loop without a predefined end. This is useful
    /// for animations or motions that should continue until explicitly stopped.</remarks>
    /// <returns>The current instance with infinite repeat enabled.</returns>
    public MotionSequence WithInfinityRepeat()
    {
        _item.Timeline.LoopMode = MotionTimelineLoopMode.Infinite;
        _item.Timeline.LoopCount = -1;

        return this;
    }

    /// <summary>
    /// Disables repetition for the current motion sequence, ensuring it plays only once.
    /// </summary>
    /// <remarks>Call this method to configure the motion sequence to execute a single time without looping.
    /// This is useful when you want the sequence to play once and then stop, overriding any previous repeat or loop
    /// settings.</remarks>
    /// <returns>The current instance of the motion sequence with repeat behavior disabled.</returns>
    public MotionSequence WithoutRepeat()
    {
        _item.Timeline.LoopMode = MotionTimelineLoopMode.None;
        _item.Timeline.LoopCount = 0;
        return this;
    }

    /// <summary>
    /// Configures the motion sequence to loop in a ping-pong manner, alternating direction on each iteration and
    /// repeating indefinitely.
    /// </summary>
    /// <remarks>Use this method to create animations that reverse direction at the end of each cycle and
    /// continue looping without limit. This is useful for effects that should oscillate smoothly between two
    /// states.</remarks>
    /// <returns>The current instance of the motion sequence with ping-pong looping enabled.</returns>
    public MotionSequence WithPingPong()
    {
        _item.Timeline.LoopMode = MotionTimelineLoopMode.PingPong;
        _item.Timeline.LoopCount = -1;

        return this;
    }

    /// <summary>
    /// Begins executing the motion sequence asynchronously and raises the Completed event when all steps have finished.
    /// </summary>
    /// <remarks>If the sequence contains no steps, the Completed event is raised immediately. The method
    /// executes all steps in the sequence and signals completion when the last step finishes.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task completes when the motion sequence has finished
    /// executing all steps.</returns>
    public async Task StartAsync()
    {
        if (_steps.Count == 0)
        {
            Completed?.Invoke(this, new MotionSequenceEventArgs(this));
            return;
        }

        var tasks = _steps.Select(step => step()).ToArray();
        var buckets = tasks.Interleaved();
        var last = buckets[^1];

        var t = await last.ConfigureAwait(false);
        await t.ConfigureAwait(false);

        Completed?.Invoke(this, new MotionSequenceEventArgs(this));
    }
}

namespace FluentUI.Blazor.Community.Components.Extensions;

/// <summary>
/// Provides methods for creating an array of tasks that complete in the order their corresponding input tasks complete.
/// </summary>
/// <remarks>This class enables consumers to process tasks as they finish, regardless of their original order in
/// the input array. It is useful for scenarios where the order of task completion is important, such as when processing
/// results as soon as they become available.</remarks>
public static class TaskInterleave
{
    /// <summary>
    /// Returns an array of tasks that complete in the order the input tasks complete, regardless of their original
    /// order in the source collection.
    /// </summary>
    /// <remarks>Each element in the returned array is a task that completes when one of the input tasks
    /// completes, with the first element corresponding to the first task to complete, the second to the second, and so
    /// on. This can be useful for processing tasks as they finish, rather than waiting for all to complete or
    /// processing them in their original order.</remarks>
    /// <param name="tasks">The collection of tasks to observe and interleave based on their completion order.</param>
    /// <returns>An array of tasks, each representing the completion of one of the input tasks. The array is ordered by the
    /// completion time of the input tasks, not their original order.</returns>
    public static Task<Task>[] Interleaved(this IEnumerable<Task> tasks)
    {
        var inputTasks = tasks.ToList();
        var buckets = new TaskCompletionSource<Task>[inputTasks.Count];
        var results = new Task<Task>[buckets.Length];

        for (var i = 0; i < buckets.Length; i++)
        {
            buckets[i] = new TaskCompletionSource<Task>(TaskCreationOptions.RunContinuationsAsynchronously);
            results[i] = buckets[i].Task;
        }

        var nextTaskIndex = -1;

        void Continuation(Task completed)
        {
            var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
            bucket.TrySetResult(completed);
        }

        foreach (var inputTask in inputTasks)
        {
            inputTask.ContinueWith(
                Continuation,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }

        return results;
    }

    /// <summary>
    /// Returns an array of tasks that complete in the order in which the supplied tasks complete, regardless of their
    /// original order in the input sequence.
    /// </summary>
    /// <remarks>Each element in the returned array is a task that yields the corresponding completed input
    /// task. This allows processing of results as tasks finish, rather than in the original order. The returned tasks
    /// themselves complete in the order of task completion, not the order of the input sequence.</remarks>
    /// <typeparam name="T">The type of the result produced by the input tasks.</typeparam>
    /// <param name="tasks">The sequence of tasks to observe for completion order.</param>
    /// <returns>An array of tasks, each of which completes when one of the input tasks completes. The first task in the array
    /// completes when the first input task completes, the second when the second input task completes, and so on.</returns>
    public static Task<Task<T>>[] Interleaved<T>(this IEnumerable<Task<T>> tasks)
    {
        var inputTasks = tasks.ToList();
        var buckets = new TaskCompletionSource<Task<T>>[inputTasks.Count];
        var results = new Task<Task<T>>[buckets.Length];

        for (var i = 0; i < buckets.Length; i++)
        {
            buckets[i] = new TaskCompletionSource<Task<T>>(TaskCreationOptions.RunContinuationsAsynchronously);
            results[i] = buckets[i].Task;
        }

        var nextTaskIndex = -1;

        void Continuation(Task<T> completed)
        {
            var bucket = buckets[Interlocked.Increment(ref nextTaskIndex)];
            bucket.TrySetResult(completed);
        }

        foreach (var inputTask in inputTasks)
        {
            inputTask.ContinueWith(
                Continuation,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        }

        return results;
    }
}

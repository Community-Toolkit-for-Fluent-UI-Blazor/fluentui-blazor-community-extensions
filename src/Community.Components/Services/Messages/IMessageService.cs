using Microsoft.AspNetCore.SignalR.Client;

namespace FluentUI.Blazor.Community.Services;

public interface IMessageService
    : IAsyncDisposable
{
    bool IsConnected { get; }

    string? ConnectionId { get; }

    Task StartAsync(CancellationToken cancellationToken = default);

    Task StopAsync(CancellationToken cancellationToken = default);

    IMessageService ListenOn(string name, Action action);

    IMessageService ListenOn(string name, Func<Task> task);

    IMessageService ListenOn<T>(string name, Action<T> action);

    IMessageService ListenOn<T>(string name, Func<T, Task> task);

    IMessageService ListenOn<T1, T2>(string name, Action<T1, T2> action);

    IMessageService ListenOn<T1, T2>(string name, Func<T1, T2, Task> task);

    IMessageService ListenOff(string name);

    ValueTask SendAsync(string methodName, CancellationToken cancellationToken = default);

    ValueTask SendAsync<T>(string methodName, T value, CancellationToken cancellationToken = default);

    ValueTask SendAsync<T1, T2>(string methodName, T1 value, T2 value2, CancellationToken cancellationToken = default);

    ValueTask SendAsync<T1, T2, T3>(string methodName, T1 value, T2 value2, T3 value3, CancellationToken cancellationToken = default);

    ValueTask SendAsync<T1, T2, T3, T4>(string methodName, T1 value, T2 value2, T3 value3, T4 value4, CancellationToken cancellationToken = default);
}

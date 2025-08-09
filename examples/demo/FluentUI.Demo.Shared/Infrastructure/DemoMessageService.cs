using FluentUI.Blazor.Community.Components;
using FluentUI.Blazor.Community.Services;

namespace FluentUI.Demo.Shared.Infrastructure;

internal sealed class DemoMessageService(string name)
    : IMessageService
{
    private readonly Dictionary<string, object> _listeners = new();

    public bool IsConnected => true;

    public string? ConnectionId { get; } = Guid.NewGuid().ToString();

    private DemoMessageService AddInternal(string name, Delegate action)
    {
        if (_listeners.ContainsKey(name))
        {
            _listeners[name] = action;
        }
        else
        {
            _listeners.Add(name, action);
        }

        return this;
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public IMessageService ListenOff(string name)
    {
        _listeners.Remove(name);

        return this;
    }

    public IMessageService ListenOn(string name, Action action)
    {
        return AddInternal(name, action);
    }

    public IMessageService ListenOn(string name, Func<Task> task)
    {
        return AddInternal(name, task);
    }

    public IMessageService ListenOn<T>(string name, Action<T> action)
    {
        return AddInternal(name, action);
    }

    public IMessageService ListenOn<T>(string name, Func<T, Task> task)
    {
        return AddInternal(name, task);
    }

    public IMessageService ListenOn<T1, T2>(string name, Action<T1, T2> action)
    {
        return AddInternal(name, action);
    }

    public IMessageService ListenOn<T1, T2>(string name, Func<T1, T2, Task> task)
    {
        return AddInternal(name, task);
    }

    public ValueTask SendAsync(string methodName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async ValueTask SendAsync<T>(string methodName, T value, CancellationToken cancellationToken = default)
    {
        switch (methodName)
        {
            case ChatMessageListViewConstants.PinOrUnpin:
                {
                    if (value is long roomId)
                    {
                        if (_listeners.TryGetValue(ChatMessageListViewConstants.PinOrUnpin, out var action) && action is Func<long, Task> pinOrUnpinAction)
                        {
                            await pinOrUnpinAction(roomId);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid value type for PinOrUnpin", nameof(value));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid value type for PinOrUnpin", nameof(value));
                    }
                }

                break;

            case ChatMessageListViewConstants.MessageDeleted:
                {
                    if (value is long roomId)
                    {
                        if (_listeners.TryGetValue(ChatMessageListViewConstants.MessageDeleted, out var action) && action is Func<long, Task> deleteAction)
                        {
                            await deleteAction(roomId);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid value type for MessageDeleted", nameof(value));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid value type for MessageDeleted", nameof(value));
                    }
                }

                break;

            case ChatMessageListViewConstants.ReactOnMessage:
                {
                    if (value is long roomId)
                    {
                        if (_listeners.TryGetValue(ChatMessageListViewConstants.ReactOnMessage, out var action) && action is Func<long, Task> reactAction)
                        {
                            await reactAction(roomId);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid value type for ReactOnMessage", nameof(value));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid value type for ReactOnMessage", nameof(value));
                    }
                }

                break;

            default:
                throw new NotImplementedException($"Method {methodName} is not implemented.");
        }
    }

    public async ValueTask SendAsync<T1, T2>(string methodName, T1 value, T2 value2, CancellationToken cancellationToken = default)
    {
        switch (methodName)
        {
            case ChatMessageListViewConstants.PinOrUnpinAsync:
                await SendAsync(ChatMessageListViewConstants.PinOrUnpin, value, cancellationToken);
                break;

            case ChatMessageListViewConstants.ReceiveMessages:
                {
                    if (value is long roomId && value2 is IEnumerable<long> messageIdCollection)
                    {
                        if (_listeners.TryGetValue(ChatMessageListViewConstants.ReceiveMessages, out var action) && action is Func<long, IEnumerable<long>, Task> func)
                        {
                            await func(roomId, messageIdCollection);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid value type for ReceiveMessages", nameof(value));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid value type for ReceiveMessages", nameof(value));
                    }
                }

                break;

            case ChatMessageListViewConstants.ReceiveMessage:
                {
                    if (value is long roomId && value2 is long messageId)
                    {
                        if (_listeners.TryGetValue(ChatMessageListViewConstants.ReceiveMessage, out var action) && action is Func<long, long, Task> func)
                        {
                            await func(roomId, messageId);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid value type for ReceiveMessages", nameof(value));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid value type for ReceiveMessages", nameof(value));
                    }
                }

                break;

            case ChatMessageListViewConstants.DeleteMessageAsync:
                {
                    await SendAsync(ChatMessageListViewConstants.MessageDeleted, value, cancellationToken);
                }

                break;

            case ChatMessageListViewConstants.SendReactOnMessageAsync:
                {
                    await SendAsync(ChatMessageListViewConstants.ReactOnMessage, value, cancellationToken);
                }

                break;

            case ChatMessageListViewConstants.MessageRead:
                {
                    if (value is long roomId && value2 is long messageId)
                    {
                        if (_listeners.TryGetValue(ChatMessageListViewConstants.MessageRead, out var action) && action is Func<long, long, Task> func)
                        {
                            await func(roomId, messageId);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid value type for ReceiveMessages", nameof(value));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid value type for ReceiveMessages", nameof(value));
                    }
                }

                break;

            default: throw new NotImplementedException($"Method {methodName} is not implemented.");
        }
    }

    public ValueTask SendAsync<T1, T2, T3>(string methodName, T1 value, T2 value2, T3 value3, CancellationToken cancellationToken = default)
    {
        return methodName switch
        {
            ChatMessageListViewConstants.SendMessagesAsync => SendAsync(ChatMessageListViewConstants.ReceiveMessages, value, value2, cancellationToken),
            ChatMessageListViewConstants.SendMessageAsync => SendAsync(ChatMessageListViewConstants.ReceiveMessage, value, value2, cancellationToken),
            ChatMessageListViewConstants.MessageReadAsync => SendAsync(ChatMessageListViewConstants.MessageRead, value, value2, cancellationToken),
            _ => throw new NotImplementedException($"Method {methodName} is not implemented.")
        };
    }

    public ValueTask SendAsync<T1, T2, T3, T4>(string methodName, T1 value, T2 value2, T3 value3, T4 value4, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}

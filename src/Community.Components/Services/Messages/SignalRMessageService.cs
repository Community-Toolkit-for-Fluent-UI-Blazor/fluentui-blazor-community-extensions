using System.Net;
using FluentUI.Blazor.Community.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace FluentUI.Blazor.Community.Services;

internal sealed class SignalRMessageService
    : IMessageService
{
    private readonly HubConnection _hubConnection;
    private readonly List<IDisposable> _subscriptions = [];

    public SignalRMessageService(string name, IConfiguration configuration, NavigationManager navigationManager, ICookieProvider cookieProvider)
    {
        var uri = navigationManager.ToAbsoluteUri(name);
        _hubConnection ??= new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithStatefulReconnect()
            .WithUrl(uri, options =>
            {
                var container = new CookieContainer();
                var section = configuration.GetSection("Application")
                                           .GetSection("Azure")
                                           .GetSection("SignalR");
                var cookieInstance = new Cookie()
                {
                    Name = section.GetValue("CookieName", string.Empty),
                    Domain = section.GetValue("Domain", string.Empty),
                    Value = cookieProvider.Cookie
                };

                container.Add(cookieInstance);
                options.Cookies = container;
            })
            .Build();
    }

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public string? ConnectionId => _hubConnection?.ConnectionId;

    public async ValueTask DisposeAsync()
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Dispose();
        }

        _subscriptions.Clear();

        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }

    public IMessageService ListenOff(string name)
    {
        _hubConnection?.Remove(name);

        return this;
    }

    public IMessageService ListenOn(string name, Action action)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));

        _subscriptions.Add(_hubConnection.On(name, action));

        return this;
    }

    public IMessageService ListenOn(string name, Func<Task> task)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));

        _subscriptions.Add(_hubConnection.On(name, task));

        return this;
    }

    public IMessageService ListenOn<T>(string name, Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));

        _subscriptions.Add(_hubConnection.On(name, action));

        return this;
    }

    public IMessageService ListenOn<T>(string name, Func<T, Task> task)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));

        _subscriptions.Add(_hubConnection.On(name, task));

        return this;
    }

    public IMessageService ListenOn<T1, T2>(string name, Action<T1, T2> action)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));

        _subscriptions.Add(_hubConnection.On(name, action));

        return this;
    }

    public IMessageService ListenOn<T1, T2>(string name, Func<T1, T2, Task> task)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));

        _subscriptions.Add(_hubConnection.On(name, task));

        return this;
    }

    public async ValueTask SendAsync(string name, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
        await _hubConnection.SendAsync(name, cancellationToken);
    }

    public async ValueTask SendAsync<T>(string name, T value, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
        await _hubConnection.SendAsync(name, value, cancellationToken);
    }

    public async ValueTask SendAsync<T1, T2>(string name, T1 value, T2 value2, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
        await _hubConnection.SendAsync(name, value, value2, cancellationToken);
    }

    public async ValueTask SendAsync<T1, T2, T3>(string name, T1 value, T2 value2, T3 value3, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
        await _hubConnection.SendAsync(name, value, value2, value3, cancellationToken);
    }

    public async ValueTask SendAsync<T1, T2, T3, T4>(string name, T1 value, T2 value2, T3 value3, T4 value4, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(_hubConnection, nameof(_hubConnection));
        await _hubConnection.SendAsync(name, value, value2, value3, value4, cancellationToken);
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StartAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            await _hubConnection.StopAsync(cancellationToken);
        }
    }
}

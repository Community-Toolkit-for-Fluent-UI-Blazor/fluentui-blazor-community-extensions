using FluentUI.Blazor.Community.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FluentUI.Blazor.Community.Hubs;

[Authorize]
public class ChatHub(ISignalRUserCache cache)
    : Hub
{
    private List<string> GetConnections(List<string> usernames)
    {
        List<string> result = [];

        foreach (var u in usernames)
        {
            var connectionId = cache.GetConnectionId(u);

            if (!string.IsNullOrEmpty(connectionId))
            {
                result.Add(connectionId);
            }
        }

        return result;
    }

    public async Task PinOrUnpinAsync(long roomId, List<string> usernames)
    {
        var connections = GetConnections(usernames);

        if (connections.Count != 0)
        {
            await Clients.Clients(connections).SendAsync("PinOrUnpin", roomId);
        }
    }

    public async Task SendMessagesAsync(
        long roomId,
        IEnumerable<long> messageIdCollection,
        List<string> usernames)
    {
        var connections = GetConnections(usernames);

        if (connections.Count != 0)
        {
            await Clients.Clients(connections).SendAsync("ReceiveMessages", roomId, messageIdCollection);
        }
    }

    public async Task SendMessageAsync(
        long roomId,
        long messageId,
        List<string> usernames)
    {
        var connections = GetConnections(usernames);

        if (connections.Count != 0)
        {
            await Clients.Clients(connections).SendAsync("ReceiveMessage", roomId, messageId);
        }
    }

    public async Task DeleteMessageAsync(
        long roomId,
        List<string> usernames)
    {
        var connections = GetConnections(usernames);

        if (connections.Count != 0)
        {
            await Clients.Clients(connections).SendAsync("MessageDeleted", roomId);
        }
    }

    public async Task SendReactOnMessageAsync(
        long roomId,
        List<string> usernames)
    {
        var connections = GetConnections(usernames);

        if (connections.Count != 0)
        {
            await Clients.Clients(connections).SendAsync("ReactOnMessage", roomId);
        }
    }

    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var userName = Context.User?.Identity?.Name;

        if (!string.IsNullOrEmpty(userName))
        {
            cache.TryAdd(userName, Context.ConnectionId);
        }
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);

        var userName = Context.User?.Identity?.Name;

        if (!string.IsNullOrEmpty(userName))
        {
            cache.Remove(userName);
        }
    }
}

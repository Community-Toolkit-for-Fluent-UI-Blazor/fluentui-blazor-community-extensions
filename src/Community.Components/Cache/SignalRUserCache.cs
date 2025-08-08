namespace FluentUI.Blazor.Community.Cache;

internal class SignalRUserCache
    : ISignalRUserCache
{
    private readonly Dictionary<string, string> _cache = [];

    public string GetConnectionId(string user)
    {
        if (_cache.TryGetValue(user, out var connectionId))
        {
            return connectionId;
        }

        return string.Empty;
    }

    public bool Remove(string user)
    {
        return _cache.Remove(user);
    }

    public bool TryAdd(string user, string connectionId)
    {
        return _cache.TryAdd(user, connectionId);
    }
}

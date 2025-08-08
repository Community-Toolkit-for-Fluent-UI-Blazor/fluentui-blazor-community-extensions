namespace FluentUI.Blazor.Community.Cache;

public interface ISignalRUserCache
{
    bool TryAdd(string user, string connectionId);

    bool Remove(string user);

    string GetConnectionId(string user);
}

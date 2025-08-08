namespace FluentUI.Blazor.Community.Services;

public interface IMessageServiceFactory
{
    IMessageService Create(string name);
}

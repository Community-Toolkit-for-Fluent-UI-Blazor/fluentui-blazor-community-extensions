namespace FluentUI.Blazor.Community.Components.Chat.Transport;

/// <summary>
/// Represents a transport mechanism for sending and receiving chat messages.
/// </summary>
public interface IMessageTransport
{
    /// <summary>
    /// Connects to the message transport, establishing any necessary connections or subscriptions.
    /// </summary>
    /// <returns>Returns a task that represents the asynchronous connect operation.</returns>
    Task ConnectAsync();

    /// <summary>
    /// Disconnects from the message transport, cleaning up any connections or subscriptions established during ConnectAsync.
    /// </summary>
    /// <returns>Returns a task that represents the asynchronous disconnect operation.</returns>
    Task DisconnectAsync();

    /// <summary>
    /// Sends a message envelope through the transport.
    /// </summary>
    /// <param name="envelope">The message envelope to send.</param>
    /// <returns>Returns a task that represents the asynchronous send operation.</returns>
    Task SendAsync(TransportEnvelope envelope);

    /// <summary>
    /// Registers a message handler that will be invoked whenever a message is received through the transport.
    /// </summary>
    /// <param name="handler">The message pump handler to be invoked for each received message.</param>
    void RegisterMessageHandler(Func<TransportEnvelope, Task> handler);

    /// <summary>
    /// Joins the specified room asynchronously.
    /// </summary>
    /// <param name="roomId">The unique identifier of the room to join.</param>
    /// <returns>A task that represents the asynchronous join operation.</returns>
    Task JoinRoomAsync(long roomId);

    /// <summary>
    /// Leaves the specified room asynchronously.
    /// </summary>
    /// <param name="roomId">The unique identifier of the room to leave.</param>
    /// <returns>A task that represents the asynchronous leave operation.</returns>
    Task LeaveRoomAsync(long roomId);

}

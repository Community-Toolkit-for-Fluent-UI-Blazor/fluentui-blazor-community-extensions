using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;

namespace FluentUI.Blazor.Community.Components.Chat.Transport;

/// <summary>
/// Provides a SignalR-based implementation of <see cref="IMessageTransport"/> for real-time bidirectional communication
/// with a SignalR hub.
/// </summary>
/// <remarks>The hub must implement methods named "SendEnvelope" and "ReceiveEnvelope" to handle outgoing and
/// incoming messages. The connection uses automatic reconnection on network failures.</remarks>
internal sealed class SignalRMessageTransport : IMessageTransport
{
    private readonly HubConnection _connection;
    private Func<TransportEnvelope, Task>? _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignalRMessageTransport"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The configuration used to retrieve the Hub URL.</param>
    /// <param name="navigationManager">The navigation manager used to construct the full Hub URL.</param>
    public SignalRMessageTransport(
        IConfiguration configuration,
        NavigationManager navigationManager)
    {
        var hubUrlValue = configuration.GetValue<string>("FluentUI:Blazor:Hubs:Chat");
        var baseUri = navigationManager.BaseUri;
        var hubUrl = new Uri(new Uri(baseUri), hubUrlValue).ToString();

        ArgumentException.ThrowIfNullOrEmpty(hubUrlValue, "HubUrl configuration value is required.");

        _connection = new HubConnectionBuilder()
            .WithAutomaticReconnect()
            .WithUrl(hubUrl)
            .Build();
    }

    /// <inheritdoc />
    public async Task ConnectAsync()
    {
        _connection.On<string, long, long, JsonElement>(
            "ReceiveEnvelope",
            async (type, roomId, senderId, payload) =>
            {
                if (_handler is null)
                {
                    return;
                }

                var envelope = new TransportEnvelope
                {
                    Type = type,
                    RoomId = roomId,
                    SenderId = senderId,
                    Payload = payload
                };

                await _handler(envelope);
            });

        await _connection.StartAsync();
    }

    /// <inheritdoc />
    public Task DisconnectAsync() => _connection.StopAsync();

    /// <inheritdoc />
    public Task SendAsync(TransportEnvelope envelope, CancellationToken cancellationToken)
        => _connection.SendAsync(
            "SendEnvelopeAsync",
            envelope.Type,
            envelope.RoomId,
            envelope.SenderId,
            envelope.Payload,
            cancellationToken);

    /// <inheritdoc />
    public void RegisterMessageHandler(Func<TransportEnvelope, Task> handler) => _handler = handler;

    /// <inheritdoc />
    public Task JoinRoomAsync(long roomId, CancellationToken cancellationToken) => _connection.InvokeAsync("JoinRoomAsync", roomId, cancellationToken);

    /// <inheritdoc />
    public Task LeaveRoomAsync(long roomId, CancellationToken cancellationToken)  => _connection.InvokeAsync("LeaveRoomAsync", roomId, cancellationToken);

}

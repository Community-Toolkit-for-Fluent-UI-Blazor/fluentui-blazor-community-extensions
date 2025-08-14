namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// Represents an exception for the chat room list.
/// </summary>
[Serializable]
internal class ChatRoomListException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomListException" /> class.
    /// </summary>
    public ChatRoomListException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomListException" /> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ChatRoomListException(string? message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatRoomListException" /> class with a specified error
    ///  message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
    ///  if no inner exception is specified.</param>
    public ChatRoomListException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}

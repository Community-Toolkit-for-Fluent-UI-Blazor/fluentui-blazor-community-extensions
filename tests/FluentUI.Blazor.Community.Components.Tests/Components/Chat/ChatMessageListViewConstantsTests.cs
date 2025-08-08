using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components;

public class ChatMessageListViewConstantsTests
{
    [Fact]
    public void ReceiveMessage_Constant_IsCorrect()
    {
        Assert.Equal("ReceiveMessage", ChatMessageListViewConstants.ReceiveMessages);
    }

    [Fact]
    public void MessageDeleted_Constant_IsCorrect()
    {
        Assert.Equal("MessageDeleted", ChatMessageListViewConstants.MessageDeleted);
    }

    [Fact]
    public void ReactOnMessage_Constant_IsCorrect()
    {
        Assert.Equal("ReactOnMessage", ChatMessageListViewConstants.ReactOnMessage);
    }

    [Fact]
    public void PinOrUnpin_Constant_IsCorrect()
    {
        Assert.Equal("PinOrUnpin", ChatMessageListViewConstants.PinOrUnpin);
    }

    [Fact]
    public void SendReactOnMessageAsync_Constant_IsCorrect()
    {
        Assert.Equal("SendReactOnMessageAsync", ChatMessageListViewConstants.SendReactOnMessageAsync);
    }

    [Fact]
    public void PinOrUnpinAsync_Constant_IsCorrect()
    {
        Assert.Equal("PinOrUnpinAsync", ChatMessageListViewConstants.PinOrUnpinAsync);
    }

    [Fact]
    public void DeleteMessageAsync_Constant_IsCorrect()
    {
        Assert.Equal("DeleteMessageAsync", ChatMessageListViewConstants.DeleteMessageAsync);
    }

    [Fact]
    public void MessageReadAsync_Constant_IsCorrect()
    {
        Assert.Equal("MessageReadAsync", ChatMessageListViewConstants.MessageReadAsync);
    }

    [Fact]
    public void SendMessagesAsync_Constant_IsCorrect()
    {
        Assert.Equal("SendMessagesAsync", ChatMessageListViewConstants.SendMessagesAsync);
    }
}

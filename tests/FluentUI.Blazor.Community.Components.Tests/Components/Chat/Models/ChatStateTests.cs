using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatStateTests
{
    [Fact]
    public void Room_Setter_TriggersRoomChangedEvent_WhenRoomChanges()
    {
        // Arrange
        var state = new ChatState();
        var owner = new ChatUser { Id = 1, DisplayName = "Owner" };
        var room1 = new ChatRoom { Id = 1, Name = "Room1", Owner = owner, Users = new List<ChatUser>() };
        var room2 = new ChatRoom { Id = 2, Name = "Room2", Owner = owner, Users = new List<ChatUser>() };
        ChatRoom? eventRoom = null;
        state.RoomChanged += (s, r) => eventRoom = r;

        // Act
        state.Room = room1;
        var firstEvent = eventRoom;
        state.Room = room1; // No event expected
        state.Room = room2;
        var secondEvent = eventRoom;

        // Assert
        Assert.Equal(room1, firstEvent);
        Assert.Equal(room2, secondEvent);
    }

    [Fact]
    public void IsLoading_SetAndGet_Works()
    {
        var state = new ChatState();
        state.IsLoading = true;
        Assert.True(state.IsLoading);
        state.IsLoading = false;
        Assert.False(state.IsLoading);
    }

    [Fact]
    public void ClearDraft_RemovesAndRecreatesDraft()
    {
        var state = new ChatState();
        var owner = new ChatUser { Id = 1, DisplayName = "Owner" };
        var room = new ChatRoom { Id = 42, Name = "Test", Owner = owner, Users = new List<ChatUser>() };
        state.Room = room;

        // Get draft to create it
        var draft1 = state.GetDraft();
        Assert.NotNull(draft1);

        // Clear draft
        state.ClearDraft(room.Id);

        // Get draft again, should be a new instance
        var draft2 = state.GetDraft();
        Assert.NotNull(draft2);
        Assert.NotSame(draft1, draft2);
    }

    [Fact]
    public void GetDraft_ReturnsNull_WhenNoRoom()
    {
        var state = new ChatState();
        Assert.Null(state.GetDraft());
    }

    [Fact]
    public void CreateChatRoom_SetsRoomWithCorrectProperties()
    {
        var state = new ChatState();
        var owner = new ChatUser { Id = 1, DisplayName = "Owner" };
        var user1 = new ChatUser { Id = 2, DisplayName = "User1" };
        var user2 = new ChatUser { Id = 3, DisplayName = "User2" };

        state.CreateChatRoom(99, "MyRoom", owner, user1, user2);

        Assert.NotNull(state.Room);
        Assert.Equal(99, state.Room.Id);
        Assert.Equal("MyRoom", state.Room.Name);
        Assert.Equal(owner, state.Room.Owner);
        Assert.Contains(user1, state.Room.Users);
        Assert.Contains(user2, state.Room.Users);
        Assert.True(state.Room.IsEmpty);
    }
}

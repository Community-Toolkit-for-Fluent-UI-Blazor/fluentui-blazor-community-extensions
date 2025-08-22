using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests;
using Moq;

namespace FluentUI.Blazor.Community.Components.Tests.Components.Chat;

public class ChatRoomListViewTests : TestBase
{
    public ChatRoomListViewTests()
    {
        JSInterop.Mode = Bunit.JSRuntimeMode.Loose;
        Services.AddSingleton(UnitTestLibraryConfiguration);
        Services.AddFluentUIComponents();
        Services.AddScoped<ChatState>();
    }

    private ValueTask<IEnumerable<ChatRoom>> OnGetBlockedChatRoomsAsync(ChatRoomItemsRequest request, CancellationToken cancellationToken)
    {
        return new ValueTask<IEnumerable<ChatRoom>>(
        [
            new ChatRoom { Id = 1, Name = "Test Room 1", IsBlocked = true },
        ]);
    }

    private ValueTask<IEnumerable<ChatRoom>> OnGetHiddenChatRoomsAsync(ChatRoomItemsRequest request, CancellationToken cancellationToken)
    {
        return new ValueTask<IEnumerable<ChatRoom>>(
        [
            new ChatRoom { Id = 1, Name = "Test Room 1", IsHidden = true },
        ]);
    }

    private ValueTask<IEnumerable<ChatRoom>> OnGetSearchChatRoomsAsync(ChatRoomItemsRequest request, CancellationToken cancellationToken)
    {
        return new ValueTask<IEnumerable<ChatRoom>>(
        [
            new ChatRoom { Id = 1, Name = "Test Room 1" },
        ]);
    }

    [Fact]
    public async Task OnUnblockRoomsAsync_LoadsBlockedRooms()
    {
        // Arrange
        var comp = RenderComponent<ChatRoomListView>(
            parameters => parameters.Add(p => p.ItemsProvider, OnGetBlockedChatRoomsAsync));

        // Act
        var task = typeof(ChatRoomListView).GetMethod("OnUnblockRoomsAsync", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(comp.Instance, null) as Task;

        if (task is not null)
        {
            await task;
        }

        // Assert
        var blockedRooms = typeof(ChatRoomListView).GetField("_blockedRooms", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(comp.Instance) as List<ChatRoom>;

        Assert.NotNull(blockedRooms);
        Assert.Single(blockedRooms);
        Assert.True(blockedRooms[0].IsBlocked);
    }

    [Fact]
    public async Task OnUnhideRoomsAsync_LoadsHiddenRooms()
    {
        var comp = RenderComponent<ChatRoomListView>(
           parameters => parameters.Add(p => p.ItemsProvider, OnGetHiddenChatRoomsAsync));

        // Act
        var task = typeof(ChatRoomListView).GetMethod("OnUnhideRoomsAsync", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(comp.Instance, null) as Task;

        if (task is not null)
        {
            await task;
        }

        // Assert
        var hiddenRooms = typeof(ChatRoomListView).GetField("_hiddenRooms", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(comp.Instance) as List<ChatRoom>;

        Assert.NotNull(hiddenRooms);
        Assert.Single(hiddenRooms);
        Assert.True(hiddenRooms[0].IsHidden);
    }

    [Fact]
    public async Task OnChatRoomSearchAsync_SetsItems()
    {
        var rooms = await OnGetSearchChatRoomsAsync(new(), CancellationToken.None);
        var args = new OptionsSearchEventArgs<ChatRoom> { Text = "test" };
        var comp = RenderComponent<ChatRoomListView>(
          parameters => parameters.Add(p => p.ItemsProvider, OnGetSearchChatRoomsAsync)
                                  .Add(p => p.RoomSearchFunction, (_, _) => Task.FromResult(rooms)));

        // Act
        var task = typeof(ChatRoomListView).GetMethod("OnChatRoomSearchAsync", BindingFlags.NonPublic | BindingFlags.Instance)!.Invoke(comp.Instance, [args]) as Task;

        if (task is not null)
        {
            await task;
        }

        Assert.Equal(rooms, args.Items);
    }

    //[Fact]
    //public async Task OnSearchFunctionAsync_ReturnsUsers()
    //{
    //    var users = new List<ChatUser> { new ChatUser() };
    //    var comp = CreateComponent(userSearchFunction: (_, _) => Task.FromResult<IEnumerable<ChatUser>>(users));

    //    var result = await comp.InvokeMethodAsync<IEnumerable<ChatUser>>("OnSearchFunctionAsync", "abc");

    //    Assert.Equal(users, result);
    //}

    //[Fact]
    //public async Task OnNewChatGroupAsync_ThrowsIfIdInvalid()
    //{
    //    var dialogService = new Mock<IDialogService>();
    //    var dialogResult = new Mock<IDialogReference>();
    //    dialogResult.Setup(d => d.Result).ReturnsAsync(DialogResult.Ok(new List<ChatUser> { new ChatUser() }));
    //    dialogService.Setup(d => d.ShowDialogAsync<ChatUserGroupSelectorDialog>(It.IsAny<object>(), It.IsAny<DialogParameters>()))
    //        .ReturnsAsync(dialogResult.Object);

    //    var comp = CreateComponent(dialogService: dialogService.Object);
    //    comp.Owner = new ChatUser();
    //    comp.OnNewChatGroup = EventCallback.Factory.Create<ChatGroupEventArgs>(this, (ChatGroupEventArgs e) => { e.ChatGroupIdReturnValue = 0; return Task.CompletedTask; });

    //    await Assert.ThrowsAsync<ChatRoomListException>(() => comp.InvokeMethodAsync("OnNewChatGroupAsync"));
    //}

    //[Fact]
    //public void Format_ReturnsEmptyMarkupString_WhenNoSections()
    //{
    //    var comp = CreateComponent();
    //    var msg = new Mock<IChatMessage>();
    //    msg.Setup(m => m.Sections).Returns(new List<IChatMessageSection>());
    //    var result = comp.InvokeMethod<MarkupString>("Format", msg.Object);
    //    Assert.Equal(string.Empty, result.Value);
    //}

    //[Fact]
    //public void Format_ReturnsGiftSender_WhenGiftAndOwner()
    //{
    //    var comp = CreateComponent();
    //    var owner = new ChatUser();
    //    comp.Owner = owner;
    //    var msg = new Mock<IChatMessage>();
    //    msg.Setup(m => m.Sections).Returns(new List<IChatMessageSection> { Mock.Of<IChatMessageSection>() });
    //    msg.Setup(m => m.MessageType).Returns(ChatMessageType.Gift);
    //    msg.Setup(m => m.Sender).Returns(owner);
    //    comp.ChatRoomLabels = new ChatRoomLabels { GiftSender = "You sent a gift" };

    //    var result = comp.InvokeMethod<MarkupString>("Format", msg.Object);
    //    Assert.Equal("You sent a gift", result.Value);
    //}

    //[Fact]
    //public async Task OnSelectedChatRoomChangedAsync_UpdatesRoom()
    //{
    //    var state = new ChatState();
    //    var comp = CreateComponent(chatState: state);
    //    var room = new ChatRoom { Id = 42 };
    //    comp.SetFieldValue("_chatRooms", new List<ChatRoom> { room });
    //    comp.SetFieldValue("_selectedRoom", "42");

    //    await comp.InvokeMethodAsync("OnSelectedChatRoomChangedAsync");

    //    Assert.Equal(room, state.Room);
    //}

    //[Fact]
    //public async Task LoadChatRoomsAsync_LoadsRoomsAndSetsSelected()
    //{
    //    var state = new ChatState();
    //    var rooms = new List<ChatRoom> { new ChatRoom { Id = 99 } };
    //    var provider = new ChatRoomItemsProvider(_ => Task.FromResult<IEnumerable<ChatRoom>>(rooms));
    //    var comp = CreateComponent(chatState: state, itemsProvider: provider);

    //    await comp.InvokeMethodAsync("LoadChatRoomsAsync", 99L);

    //    Assert.Equal(rooms[0], state.Room);
    //    var selectedRoom = comp.GetFieldValue<string>("_selectedRoom");
    //    Assert.Equal("99", selectedRoom);
    //}

    //[Fact]
    //public async Task OnRenameAsync_UpdatesRoomNameAndInvokesCallback()
    //{
    //    var dialogService = new Mock<IDialogService>();
    //    var dialogResult = new Mock<IDialogReference>();
    //    dialogResult.Setup(d => d.Result).ReturnsAsync(DialogResult.Ok("newname"));
    //    dialogService.Setup(d => d.ShowDialogAsync<ChatRoomRenameDialog>(It.IsAny<object>(), It.IsAny<DialogParameters>()))
    //        .ReturnsAsync(dialogResult.Object);

    //    var state = new ChatState { Room = new ChatRoom { Id = 1, Name = "old" } };
    //    bool callbackInvoked = false;
    //    var comp = CreateComponent(dialogService: dialogService.Object, chatState: state);
    //    comp.OnRename = EventCallback.Factory.Create<ChatRoom>(this, (ChatRoom r) => { callbackInvoked = true; return Task.CompletedTask; });

    //    await comp.InvokeMethodAsync("OnRenameAsync");

    //    Assert.Equal("newname", state.Room.Name);
    //    Assert.True(callbackInvoked);
    //}

    //[Fact]
    //public async Task OnDeleteAsync_InvokesDeleteAndClearsRoom()
    //{
    //    var dialogService = new Mock<IDialogService>();
    //    var dialogResult = new Mock<IDialogReference>();
    //    dialogResult.Setup(d => d.Result).ReturnsAsync(DialogResult.Ok());
    //    dialogService.Setup(d => d.ShowConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //        .ReturnsAsync(dialogResult.Object);

    //    var state = new ChatState { Room = new ChatRoom { Id = 1 } };
    //    bool callbackInvoked = false;
    //    var comp = CreateComponent(dialogService: dialogService.Object, chatState: state);
    //    comp.OnDelete = EventCallback.Factory.Create<ChatRoom>(this, (ChatRoom r) => { callbackInvoked = true; return Task.CompletedTask; });

    //    await comp.InvokeMethodAsync("OnDeleteAsync");

    //    Assert.Null(state.Room);
    //    Assert.True(callbackInvoked);
    //}

    //[Fact]
    //public async Task OnHideAsync_SetsRoomHiddenAndInvokesCallback()
    //{
    //    var dialogService = new Mock<IDialogService>();
    //    var dialogResult = new Mock<IDialogReference>();
    //    dialogResult.Setup(d => d.Result).ReturnsAsync(DialogResult.Ok());
    //    dialogService.Setup(d => d.ShowConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //        .ReturnsAsync(dialogResult.Object);

    //    var state = new ChatState { Room = new ChatRoom { Id = 1 } };
    //    bool callbackInvoked = false;
    //    var comp = CreateComponent(dialogService: dialogService.Object, chatState: state);
    //    comp.OnHide = EventCallback.Factory.Create<ChatRoom>(this, (ChatRoom r) => { callbackInvoked = true; return Task.CompletedTask; });

    //    await comp.InvokeMethodAsync("OnHideAsync");

    //    Assert.True(state.Room.IsHidden);
    //    Assert.True(callbackInvoked);
    //}

    //[Fact]
    //public async Task OnBlockAsync_SetsRoomBlockedAndInvokesCallback()
    //{
    //    var dialogService = new Mock<IDialogService>();
    //    var dialogResult = new Mock<IDialogReference>();
    //    dialogResult.Setup(d => d.Result).ReturnsAsync(DialogResult.Ok());
    //    dialogService.Setup(d => d.ShowConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //        .ReturnsAsync(dialogResult.Object);

    //    var state = new ChatState { Room = new ChatRoom { Id = 1 } };
    //    bool callbackInvoked = false;
    //    var comp = CreateComponent(dialogService: dialogService.Object, chatState: state);
    //    comp.OnBlock = EventCallback.Factory.Create<ChatRoom>(this, (ChatRoom r) => { callbackInvoked = true; return Task.CompletedTask; });

    //    await comp.InvokeMethodAsync("OnBlockAsync");

    //    Assert.True(state.Room.IsBlocked);
    //    Assert.True(callbackInvoked);
    //}

    //[Fact]
    //public async Task OnUnblockAsync_SetsRoomUnblockedAndInvokesCallback()
    //{
    //    var dialogService = new Mock<IDialogService>();
    //    var dialogResult = new Mock<IDialogReference>();
    //    dialogResult.Setup(d => d.Result).ReturnsAsync(DialogResult.Ok());
    //    dialogService.Setup(d => d.ShowConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //        .ReturnsAsync(dialogResult.Object);

    //    var state = new ChatState { Room = new ChatRoom { Id = 1, IsBlocked = true } };
    //    bool callbackInvoked = false;
    //    var comp = CreateComponent(dialogService: dialogService.Object, chatState: state);
    //    comp.OnUnblock = EventCallback.Factory.Create<ChatRoom>(this, (ChatRoom r) => { callbackInvoked = true; return Task.CompletedTask; });

    //    await comp.InvokeMethodAsync("OnUnblockAsync");

    //    Assert.False(state.Room.IsBlocked);
    //    Assert.True(callbackInvoked);
    //}

    //[Fact]
    //public async Task OnUnhideAsync_SetsRoomUnhiddenAndInvokesCallback()
    //{
    //    var dialogService = new Mock<IDialogService>();
    //    var dialogResult = new Mock<IDialogReference>();
    //    dialogResult.Setup(d => d.Result).ReturnsAsync(DialogResult.Ok());
    //    dialogService.Setup(d => d.ShowConfirmationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
    //        .ReturnsAsync(dialogResult.Object);

    //    var state = new ChatState { Room = new ChatRoom { Id = 1, IsHidden = true } };
    //    bool callbackInvoked = false;
    //    var comp = CreateComponent(dialogService: dialogService.Object, chatState: state);
    //    comp.OnUnhide = EventCallback.Factory.Create<ChatRoom>(this, (ChatRoom r) => { callbackInvoked = true; return Task.CompletedTask; });

    //    await comp.InvokeMethodAsync("OnUnhideAsync");

    //    Assert.False(state.Room.IsHidden);
    //    Assert.True(callbackInvoked);
    //}

    //[Fact]
    //public void OnMoreButtonClick_SetsAnchorIdAndShowsPopover()
    //{
    //    var comp = CreateComponent();
    //    comp.InvokeMethod("OnMoreButtonClick", "btn-1");
    //    var anchorId = comp.GetFieldValue<string>("_anchorId");
    //    var showPopover = comp.GetFieldValue<bool>("_showPopoverMenu");
    //    Assert.Equal("btn-1", anchorId);
    //    Assert.True(showPopover);
    //}
}

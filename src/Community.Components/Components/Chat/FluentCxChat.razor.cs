using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

/// <summary>
/// 
/// </summary>
[CascadingTypeParameter(nameof(TItem))]
public partial class FluentCxChat<TItem>
    : FluentComponentBase
{
    private class ChatOptions
    {
        public bool WriterVisible { get; set; }

        public Expression<Func<IChatMessage, bool>>? Filter { get; set; }
    }

    private bool _isMobile;
    private bool _showRoom = true;
    private readonly RenderFragment<bool> _renderChatRoomListView;
    private readonly RenderFragment _renderTabs;
    private readonly RenderFragment<ChatOptions> _renderChatList;
    private readonly RenderFragment<Expression<Func<IChatFile, bool>>> _renderDocumentList;

    private readonly ChatOptions _pinnedOptions = new()
    {
        WriterVisible = false,
        Filter = x => x.IsPinned
    };

    private readonly ChatOptions _normalOptions = new()
    {
        WriterVisible = true,
    };

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatRoomListSettings RoomListSettings { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatMessageListViewSettings<TItem> MessageListViewSettings { get; set; } = new();

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatTabSettings MessageTabSettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatTabSettings PinnedMessageTabSettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatTabSettings ImageTabSettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatTabSettings VideoTabSettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatTabSettings AudioTabSettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatTabSettings OtherTabSettings { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatUser? Owner { get; set; }

    /// <summary>
    /// Gets or sets the labels for the chat room.
    /// </summary>
    [Parameter]
    public ChatRoomLabels RoomLabels { get; set; } = ChatRoomLabels.Default;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatMessageListLabels MessageListLabels { get; set; } = ChatMessageListLabels.Default;

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public ChatViews Views { get; set; } = ChatViews.None;

    /// <summary>
    /// Gets or sets the items provider for fetching chat rooms.
    /// </summary>
    [Parameter]
    public ChatRoomItemsProvider? RoomProvider { get; set; }

    /// <summary>
    /// Gets or sets the labels for the chat UI.
    /// </summary>
    [Parameter]
    public ChatLabels Labels { get; set; } = ChatLabels.Default;

    /// <summary>
    /// Gets or sets the provider for the documents.
    /// </summary>
    [Parameter]
    public ItemsProviderDelegate<IChatFile>? DocumentsProvider { get; set; }

    /// <summary>
    /// Gets a value indicating if the chat is render in tabs or not.
    /// </summary>
    private bool ShowTabs
    {
        get
        {
            var value = (int)Views;

            return value != 0 &&
                   value != 1 &&
                   value != 2 &&
                   value != 4 &&
                   value != 8 &&
                   value != 16 &&
                   value != 32;
        }
    }
}

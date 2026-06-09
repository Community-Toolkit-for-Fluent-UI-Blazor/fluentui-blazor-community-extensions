---
title: Chat
order: 0000
category: 30|Components
icon: Chat
route: /Chat
---

# Chat

The Chat component is used to display a conversation between two or more participants.
It can be useful for building chat interfaces, messaging applications, or any other scenario where you need to display
a series of messages in a conversational format.

Some features of the Chat component include:
- Block rooms : Each room can be blocked
- Hide rooms : Each room can be hidden
- Archive rooms : Each room can be archived

You can enable the ability to block, hide, or archive rooms by setting the corresponding properties on the Chat component.
For example, setting `OnRoomBlockChanged` EventCallback will allow users to block rooms, while setting `OnRoomHideChanged` EventCallback will allow users to hide rooms.

## Examples

The chat example demonstrates how to use the Chat component.
It's composed of a list of chat rooms and each rooms can have it's own list of messages.

{{ ChatExample }}


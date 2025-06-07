// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public interface IItemValue<TItem>
{
    TItem Value { get; set; }
}

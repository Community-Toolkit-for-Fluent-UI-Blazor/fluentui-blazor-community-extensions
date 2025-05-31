// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

[Flags]
public enum AcceptFile
    : ulong
{
    None = 0,
    Audio = 1,
    Image = 2,
    Video = 4,
    Pdf = 8,
    Excel = 16,
    Word = 32,
    Powerpoint = 64,
    Document = Excel | Word | Powerpoint | Pdf
}

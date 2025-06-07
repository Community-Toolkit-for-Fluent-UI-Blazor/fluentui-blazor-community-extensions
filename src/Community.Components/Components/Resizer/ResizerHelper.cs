// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Blazor.Community.Components;

internal static class ResizerHelper
{
    /// <summary>
    /// 
    /// </summary>
    private static readonly Dictionary<ResizerHandler, string> _ltrResize = new(EqualityComparer<ResizerHandler>.Default)
    {
        [ResizerHandler.Horizontally] = "top: 0px; right: 0px; bottom: 0px; width: 9px;",
        [ResizerHandler.Vertically] = "left: 0px; right: 0px; bottom: 0px; height: 9px;",
        [ResizerHandler.Both] = "right: 0px; bottom: 0px; width: 9px; height: 9px;"
    };

    /// <summary>
    /// 
    /// </summary>
    private static readonly Dictionary<ResizerHandler, string> _rtlResize = new(EqualityComparer<ResizerHandler>.Default)
    {
        [ResizerHandler.Horizontally] = "top: 0px; left: 0px; bottom: 0px; width: 9px;",
        [ResizerHandler.Vertically] = "left: 0px; left: 0px; bottom: 0px; height: 9px;",
        [ResizerHandler.Both] = "left: 0px; bottom: 0px; width: 9px; height: 9px;"
    };

    internal static Dictionary<ResizerHandler, string> GetFromLocalizationDirection(LocalizationDirection localizationDirection)
    {
        return localizationDirection switch
        {
            LocalizationDirection.RightToLeft => _rtlResize,
            _ => _ltrResize
        };
    }
}

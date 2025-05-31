// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace FluentUI.Blazor.Community.Components;

public static class AcceptFileProvider
{
    public static string Get(AcceptFile value)
    {
        if (value == AcceptFile.None)
        {
            return string.Empty;
        }

        List<string> values = [];

        if (value.HasFlag(AcceptFile.Audio))
        {
            values.Add("audio/*");
        }

        if (value.HasFlag(AcceptFile.Video))
        {
            values.Add("video/*");
        }

        if (value.HasFlag(AcceptFile.Image))
        {
            values.Add("image/*");
        }

        if (value.HasFlag(AcceptFile.Excel))
        {
            values.AddRange(".xls", ".xlsx");
        }

        if (value.HasFlag(AcceptFile.Pdf))
        {
            values.AddRange(".pdf");
        }

        if (value.HasFlag(AcceptFile.Powerpoint))
        {
            values.AddRange(".ppt", ".pptx");
        }

        if (value.HasFlag(AcceptFile.Word))
        {
            values.AddRange(".doc", ".docx");
        }

        return string.Join(", ", values);
    }
}


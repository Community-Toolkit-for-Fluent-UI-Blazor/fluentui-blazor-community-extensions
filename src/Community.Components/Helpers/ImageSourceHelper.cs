namespace FluentUI.Blazor.Community.Helpers;

public static class ImageSourceHelper
{
    public static string? GetBase64Content(byte[] data, string contentType)
    {
        if (data is null || data.Length == 0)
        {
            return string.Empty;
        }

        var base64String = Convert.ToBase64String(data);
        return $"data:{contentType};base64,{base64String}";
    }
}

# Image processing Middleware

## Overview

The <code>FluentCxPicture</code> comes with built-in image processing capabilities that allow you to manipulate images on-the-fly. This is particularly useful for optimizing images for different devices and screen sizes.

You can define image processing options directly in the <code>Src</code> attribute of the <code>FluentCxPicture</code> component. The following options are available:

| Option | Description | Example |
|--------|-------------|---------|
| <code>width</code> | Sets the width of the image. | <code>?width=300</code> |
| <code>height</code> | Sets the height of the image. | <code>?height=200</code> |
| <code>format</code> | Converts the image to the specified format (e.g., jpg, png, webp). | <code>?format=webp</code> |
| <code>quality</code> | Sets the quality of the image (1-100). | <code>?quality=80</code> |

## Middleware Setup

To enable image processing, you need to set up the middleware in your Blazor Server application. Add the following code to your `Program.cs` file:
```csharp
builder.Services.AddScoped<ImageProcessingMiddleware>();
builder.Services.AddSingleton<IImageSourceStrategy, Base64ImageStrategy>();
builder.Services.AddSingleton<IImageSourceStrategy, FtpImageStrategy>();
builder.Services.AddSingleton<IImageSourceStrategy, HttpImageStrategy>();
builder.Services.AddSingleton<IImageSourceStrategy, IpfsImageStrategy>();
```

The middleware will intercept requests for images and apply the specified processing options.
For that, the request URL must follow this pattern:
```/image-proxy/{encodedImageUrl}?{options}
```

Where:

- `{encodedImageUrl}` is the Base64 URL-encoded original image URL, or the relative path to the image in your `wwwroot` folder or an url to an external image.
- `{options}` are the image processing options as query parameters.

The middleware will decode the image URL, apply the specified processing options, and return the processed image.

The middleware consume many strategies. Theses strategies are implementations of the `IImageSourceStrategy` interface. We provide 3 strategies :

- `Base64ImageStrategy`: This strategy handles Base64 encoded image URLs.
- `FtpImageStrategy` : This strategy handles image URLs that use the FTP protocol.
- `HttpImageStrategy` : This strategy handles image URLs that use the HTTP or HTTPS protocols.
- `IpfsImageStrategy` : This strategy handles image URLs that use the IPFS protocol.

You can also implement your own strategies by implementing the `IImageSourceStrategy` interface and registering them in the DI container.

By copying / pasting the following code in your own classes, you will be able to use :
- `Azure Blob Storage`
- `Google Cloud`
- `S3`

We don't provide these strategies by default because they require additional dependencies and configuration.

### Azure Blob Storage Image Strategy

Below is the implementation of an image source strategy for retrieving images from Azure Blob Storage. This strategy uses the Azure.Storage.Blobs library to access and download images stored in Azure Blob Storage.

```csharp
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Provides an image source strategy for retrieving image data from Azure Blob Storage.
/// </summary>
/// <remarks>This strategy is designed to handle URLs pointing to Azure Blob Storage resources.  It determines
/// whether a given URL can be processed and retrieves the image data as a byte array.</remarks>
public class AzureBlobImageStrategy : IImageSourceStrategy
{
    /// <inheritdoc />
    public int Priority => 4;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.Contains(".blob.core.windows.net");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var blobClient = new BlobClient(new Uri(url));
            using var ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"Azure Blob fetch failed for {url}");
            return [];
        }
    }
}
```

### Google Cloud Storage Image Strategy

Below is the implementation of an image source strategy for retrieving images from Google Cloud Storage. This strategy uses the Google.Cloud.Storage.V1 library to access and download images stored in Google Cloud Storage.

```csharp
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Provides an implementation of <see cref="IImageSourceStrategy"/> for handling image URLs hosted on Google Cloud
/// Storage.
/// </summary>
/// <remarks>This strategy is specifically designed to handle URLs that point to objects in Google Cloud Storage,
/// identified by the presence of "storage.googleapis.com" in the URL. It uses the Google Cloud Storage API to fetch the
/// image bytes.</remarks>
public class GoogleCloudImageStrategy : IImageSourceStrategy
{
    /// <inheritdoc />
    public int Priority => 5;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.Contains("storage.googleapis.com");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var uri = new Uri(url);
            var bucket = uri.Host;
            var objectName = uri.AbsolutePath.TrimStart('/');
            var storageClient = await StorageClient.CreateAsync();
            using var ms = new MemoryStream();
            await storageClient.DownloadObjectAsync(bucket, objectName, ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"Google Cloud fetch failed for {url}");
            return [];
        }
    }
}
```

### Amazon S3 Image Strategy

Below is the implementation of an image source strategy for retrieving images from Amazon S3. This strategy uses the Amazon.S3 library to access and download images stored in Amazon S3.

```csharp
using Amazon.S3;
using Microsoft.Extensions.Logging;

namespace FluentUI.Blazor.Community.Components.Middleware;

/// <summary>
/// Provides an implementation of <see cref="IImageSourceStrategy"/> for handling image retrieval from Amazon S3.
/// </summary>
/// <remarks>This strategy is designed to handle URLs that point to Amazon S3 resources, specifically those
/// containing ".s3.amazonaws.com" in the URL. It uses an <see cref="IAmazonS3"/> client to fetch the image
/// data.</remarks>
public class S3ImageStrategy : IImageSourceStrategy
{
    /// <summary>
    /// Represents the S3 client used to fetch images.
    /// </summary>
    private readonly IAmazonS3 _s3Client;

    /// <inheritdoc />
    public int Priority => 6;

    /// <inheritdoc />
    public S3ImageStrategy(IAmazonS3 s3Client) => _s3Client = s3Client;

    /// <inheritdoc />
    public bool CanHandle(string url) => url.Contains(".s3.amazonaws.com");

    /// <inheritdoc />
    public async Task<byte[]> GetImageBytesAsync(string url, ILogger logger)
    {
        try
        {
            var uri = new Uri(url);
            var bucket = uri.Host.Split('.')[0];
            var key = uri.AbsolutePath.TrimStart('/');
            var response = await _s3Client.GetObjectAsync(bucket, key);
            using var ms = new MemoryStream();
            await response.ResponseStream.CopyToAsync(ms);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"S3 fetch failed for {url}");
            return [];
        }
    }
}
```
### Usage Example

Once the middleware is set up, you can use the image processing features in your Blazor components. Here is an example of how to use the <code>FluentCxPicture</code> component with image processing options:
```razor
<FluentCxPicture Src="/image-proxy/aHR0cHM6Ly9leGFtcGxlLmNvbS9pbWFnZS5qcGc=?width=300&format=webp" Alt="Processed Image" />
```

using Ashampoo.Translation.Systems.Components.Services;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Components.Components;

/// <summary>
/// A component for uploading a file and creating a format object from it.
/// </summary>
public partial class UploadFormat : ComponentBase
{
    /// <summary>
    /// Callback when the file is uploaded, and the format is created.
    /// </summary>
    [Parameter] public EventCallback<(IFormat?, string)> OnFormatUploaded { get; init; }

    /// <summary>
    /// Child content to display in the upload button.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Bool indicating if uploading is disabled.
    /// </summary>
    [Parameter] public bool Disabled { get; set; } = false;

    /// <summary>
    /// The color of the upload button.
    /// </summary>
    [Parameter] public Color Color { get; set; } = Color.Primary;

    /// <summary>
    /// The variant of the upload button.
    /// </summary>
    [Parameter] public Variant Variant { get; set; } = Variant.Filled;

    /// <summary>
    /// Css class for the upload button.
    /// </summary>
    [Parameter] public string Class { get; set; } = "";

    /// <summary>
    /// The start icon of the upload button.
    /// </summary>
    [Parameter] public string StartIcon { get; set; } = Icons.Filled.CloudUpload;

    [Inject] private IFormatService FormatService { get; init; } = default!;

    [Inject] private IFormatFactory FormatFactory { get; init; } = default!;

    private bool processing = false;
    private bool uploadFailed = false;
    private string uploadFailedExceptionMessage = "";
    private IFormat? format = null;

    private string id = Guid.NewGuid().ToString();

    private string fileName = "";

    /// <summary>
    /// Upload a file from the computer and create an <see cref="IFormat"/>.
    /// </summary>
    /// <param name="e"></param>
    /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
    private async Task UploadFileAsync(InputFileChangeEventArgs e)
    {
        processing = true; // Disable the upload button.
        Disabled = true;
        uploadFailed = false;
        try
        {
            // Read the file, allow for a maximum of 1MB.
            var stream = e.File.OpenReadStream(1024000L);
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms); // Copy the stream to a memory stream for async reading.
            ms.Position = 0;

            // Create the format.
            format = await FormatService.ReadFromStreamAsync(ms, e.File.Name, FormatOptionsCallback);
            
            // Get the file name
            fileName = Path.GetFileNameWithoutExtension(e.File.Name);
        }
        catch (Exception exception) // TODO: Handle?
        {
            uploadFailedExceptionMessage = exception.Message; // Display the exception message.
            uploadFailed = true; // Set the upload failed flag.
            format = null; // Clear the format.
            fileName = ""; // Clear the file name.
        }

        await OnFormatUploaded.InvokeAsync((format, fileName)); // Invoke the callback.
        processing = false; // Enable the upload button.
        Disabled = false;
    }

    /// <summary>
    /// Callback to configure the options for the <see cref="format" />.
    /// </summary>
    /// <param name="options"></param>
    /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
    private async Task FormatOptionsCallback(FormatOptions options)
    {
        await FormatService.ConfigureFormatOptionsAsync(options);
    }
}
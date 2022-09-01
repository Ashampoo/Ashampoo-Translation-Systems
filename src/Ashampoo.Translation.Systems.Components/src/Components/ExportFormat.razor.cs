using Ashampoo.Translation.Systems.Components.Services;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ashampoo.Translation.Systems.Components.Components;

/// <summary>
/// Component for exporting a format to a file.
/// </summary>
public partial class ExportFormat : ComponentBase
{
    /// <summary>
    /// The format to export, passed in from the parent component as a parameter.
    /// </summary>
    [Parameter] public IFormat? Format { get; set; }

    /// <summary>
    /// The name of the file to export to.
    /// </summary>
    [Parameter] public string FileName { get; set; } = "";

    [Inject] private IFormatService FormatService { get; init; } = default!;

    [Inject] private IFormatFactory FormatFactory { get; init; } = default!;

    [Inject] private IFileService FileService { get; init; } = default!;

    /// <summary>
    /// Export the <see cref="IFormat"/> to a file and save it to the computer.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
    private async Task SaveFile()
    {
        if (Format is null) return;

        var newFormatId = await FormatService.GetFormatIdAsync(); // Get a new format id.
        if (newFormatId == null) return;

        // Convert the format into the new format
        var convertedFormat = await FormatService.ConvertToAsync(Format, newFormatId, FormatOptionsCallback);
        if (convertedFormat.Count == 0) return;
        
        // Get the format provider for the new format
        var formatProvider = FormatFactory.GetFormatProvider(convertedFormat);

        var ms = new MemoryStream();
        convertedFormat.Write(ms); // Write the converted format to the memory stream for async purposes.
        ms.Position = 0;

        var fileExtension = formatProvider.SupportedFileExtensions; // Get the file extension for the new format.

        await FileService.SaveFile(ms, FileName, fileExtension); // Save the file to the computer.
    }

    /// <summary>
    /// Callback to configure the options of the <see cref="IFormat"/>.
    /// </summary>
    /// <param name="options"></param>
    /// <returns><see cref="Task{TResult}"/></returns>
    private async Task<bool> FormatOptionsCallback(FormatOptions options)
    {
        await FormatService.ConfigureFormatOptionsAsync(options);

        return !options.IsCanceled;
    }
}
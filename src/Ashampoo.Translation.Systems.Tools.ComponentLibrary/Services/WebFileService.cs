using System.IO.Compression;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Microsoft.JSInterop;

namespace Ashampoo.Translation.Systems.Tools.ComponentLibrary.Services;

public class WebFileService : IFileService
{
    private readonly IJSRuntime js;

    private IJSObjectReference? module;

    public WebFileService(IJSRuntime js)
    {
        this.js = js;
    }
    
    /// <summary>
    /// Load the js module for the file service when needed.
    /// </summary>
    private async Task LoadModule()
    {
        module = await js.InvokeAsync<IJSObjectReference>("import",
            "./_content/Ashampoo.Translation.Systems.Tools.ComponentLibrary/Scripts/WebFileService.js");
    }

    public async Task SaveFile(Stream stream, string fileName, string[]? fileExtension = null)
    {
        if (module is null) await LoadModule();

        using var streamRef = new DotNetStreamReference(stream: stream); // Create a reference to the stream for js.
        
        await js.InvokeVoidAsync("saveFile", streamRef, fileName, fileExtension);
    }

    public async Task SaveFormatsAsync(IEnumerable<IFormat> formats, string fileName, string[]? fileExtensions = null)
    {
        // Create a zip archive in a mnemory stream.
        await using var ms = new MemoryStream(); 
        using var archive = new ZipArchive(ms, ZipArchiveMode.Create, true); 
        
        foreach (var format in formats)
        {
            // Create a new entry in the archive.
            var formatFile =
                archive.CreateEntry($"{fileName}-{format.Header.TargetLanguage}.{fileExtensions?[0] ?? "txt"}");
            await using var entryStream = formatFile.Open(); // Open the entry stream.

            await using var tempMemoryStream = new MemoryStream(); 
            format.Write(tempMemoryStream); // Write the format to the memory stream for async purposes.
            tempMemoryStream.Seek(0, SeekOrigin.Begin);
            await tempMemoryStream.CopyToAsync(entryStream); // Copy the memory stream to the entry stream.
        }

        archive.Dispose(); // Dispose the archive.

        ms.Seek(0, SeekOrigin.Begin);
        await SaveFile(ms, fileName, new[] { ".zip" }); // Save the archive.
    }
}
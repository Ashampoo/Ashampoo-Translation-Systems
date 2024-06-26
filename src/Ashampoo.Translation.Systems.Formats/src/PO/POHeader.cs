using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Header for <see cref="POFormat"/>.
/// </summary>
public class POHeader : AbstractFormatHeader
{
    /// <inheritdoc />
    public override Language? SourceLanguage { get; set; }

    /// <inheritdoc />
    public override Dictionary<string, string> AdditionalHeaders { get; set; } = new();

    /// <inheritdoc />
    public override Language TargetLanguage
    {
        get => Language.Parse(AdditionalHeaders["Language"]);
        set => AdditionalHeaders["Language"] = value.ToString();
    }

    /// <summary>
    /// The author of the po file.
    /// </summary>
    public string? Author
    {
        get
        {
            var found = AdditionalHeaders.TryGetValue("Last-Translator", out var value);
            return found ? value : null;
        }
        set
        {
            if (value is null)
                AdditionalHeaders.Remove("Last-Translator");
            else
                AdditionalHeaders["Last-Translator"] = value;
        }
    }

    /// <summary>
    /// Write the header to the given <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer"></param>
    public async Task WriteAsync(TextWriter writer)
    {
        await writer.WriteLineAsync("msgid \"\"");
        await writer.WriteLineAsync("msgstr \"\"");
        foreach (var (key, value) in AdditionalHeaders)
        {
            // skip empty values.
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) continue;

            await writer.WriteLineAsync($"\"{key}: {value}\\n\"");
        }

        await writer.WriteLineAsync();
    }
    
    /// <summary>
    /// Write the header to the given <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer"></param>
    public void Write(TextWriter writer)
    {
        writer.WriteLine("msgid \"\"");
        writer.WriteLine("msgstr \"\"");
        foreach (var (key, value) in AdditionalHeaders)
        {
            // skip empty values.
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) continue;

            writer.WriteLine($"\"{key}: {value}\\n\"");
        }

        writer.WriteLine();
    }
}
using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Header for <see cref="POFormat"/>.
/// </summary>
public class POHeader : AbstractFormatHeader
{
    /// <inheritdoc />
    public override string? SourceLanguage { get; set; }

    /// <inheritdoc />
    public override string TargetLanguage
    {
        get => this["Language"] ?? throw new NullReferenceException("TargetLanguage is not set.");
        set
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            this["Language"] = value;
        }
    }

    /// <summary>
    /// The author of the po file.
    /// </summary>
    public string? Author
    {
        get => this["Last-Translator"];
        set
        {
            if (value is null)
                Remove("Last-Translator");
            else
                this["Last-Translator"] = value;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    public async Task WriteAsync(TextWriter writer)
    {
        await writer.WriteLineAsync("msgid \"\"");
        await writer.WriteLineAsync("msgstr \"\"");
        foreach (var (key, value) in this)
        {
            // skip empty values.
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) continue;

            await writer.WriteLineAsync($"\"{key}: {value}\\n\"");
        }

        await writer.WriteLineAsync();
    }
}
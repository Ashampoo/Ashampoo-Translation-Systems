using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.PO;

public class POHeader : AbstractFormatHeader
{
    public override string? SourceLanguage { get; set; }

    public override string TargetLanguage
    {
        get => this["Language"] ?? throw new NullReferenceException("TargetLanguage is not set.");
        set
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            this["Language"] = value;
        }
    }

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
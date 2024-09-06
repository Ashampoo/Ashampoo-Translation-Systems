using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;

namespace Ashampoo.Translation.Systems.Formats.CSV;

/// <summary>
/// Header for the <see cref="CsvFormat"/>.
/// </summary>
public sealed class CsvFormatHeader : IFormatHeader
{
    /// <inheritdoc />
    public Language TargetLanguage { get; set; } = Language.Empty;

    /// <inheritdoc />
    public Language? SourceLanguage { get; set; }

    /// <summary>
    /// The delimiter (separator) to use for the csv file.
    /// </summary>
    public char Delimiter { get; set; } = ' ';

    /// <inheritdoc />
    public Dictionary<string, string> AdditionalHeaders { get; set; } = [];

    /// <summary>
    /// Writes the header information stored in this format header to the textwriter.
    /// </summary>
    /// <param name="writer">
    /// The writer to write the header information to.
    /// </param>
    public async Task WriteHeader(TextWriter writer)
    {
        await writer.WriteLineAsync($"#Target Language: {TargetLanguage.Value}");
        await writer.WriteLineAsync($"#Source Language: {SourceLanguage?.Value}");
        await writer.WriteLineAsync($"#Delimiter: {Delimiter}");
    }
}
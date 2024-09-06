using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.CSV;

/// <summary>
/// The format provider for the CSV format.
/// </summary>
public sealed class CsvFormatProvider : IFormatProvider<CsvFormat>
{
    /// <inheritdoc />
    public string Id => "csv";

    /// <inheritdoc />
    public IEnumerable<string> SupportedFileExtensions { get; } = [".csv"];

    /// <inheritdoc />
    public CsvFormat Create() => new();

    /// <inheritdoc />
    /// <param name="fileName">
    /// The name of the file to check if it is supported.
    /// </param>
    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public IFormatBuilder<CsvFormat> GetFormatBuilder() => new CsvFormatBuilder();
}
using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.CSV;

internal sealed class CsvFormatProvider : IFormatProvider<CsvFormat>
{
    public string Id => "csv";
    public IEnumerable<string> SupportedFileExtensions { get; } = [".csv"];

    public CsvFormat Create() => new();

    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }

    public IFormatBuilder<CsvFormat> GetFormatBuilder() => new CsvFormatBuilder();
}
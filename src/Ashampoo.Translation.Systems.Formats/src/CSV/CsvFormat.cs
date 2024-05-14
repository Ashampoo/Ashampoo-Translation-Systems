using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.CSV;

internal sealed class CsvFormat : IFormat
{
    public Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task WriteAsync(Stream stream)
    {
        throw new NotImplementedException();
    }

    public IFormatHeader Header { get; }
    public LanguageSupport LanguageSupport { get; }
    public ICollection<ITranslationUnit> TranslationUnits { get; }
}
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.QT;

public class QTFormatProvider : IFormatProvider<QTFormat>
{
    public string Id { get; }
    public QTFormat Create()
    {
        throw new NotImplementedException();
    }

    public bool SupportsFileName(string fileName)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> SupportedFileExtensions { get; }
    public IFormatBuilder<QTFormat> GetFormatBuilder()
    {
        throw new NotImplementedException();
    }
}
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.QT;

public class QTTranslationString : AbstractTranslationString 
{
    public QTTranslationString(string value, Language language, List<string> comment) : base(value, language, comment)
    {
    }
}
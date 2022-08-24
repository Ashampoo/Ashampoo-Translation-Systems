using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.NLang;

public class TranslationUnit : AbstractTranslationUnit
{
    public TranslationUnit(string id) : base(id)
    {
    }

    public async Task WriteAsync(TextWriter writer)
    {
        foreach (var translation in this)
        {
            if (translation is TranslationString translationString) await translationString.WriteAsync(writer);
        }
    }
}
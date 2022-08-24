using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.PO;

public class TranslationUnit : AbstractTranslationUnit
{
    public TranslationUnit(string id) : base(id)
    {
    }

    public async Task WriteAsync(TextWriter writer)
    {
        if (this.FirstOrDefault() is Message message)
        {
            await message.WriteAsync(writer);
        }
    }
}
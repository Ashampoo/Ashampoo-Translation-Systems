using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.NLang;

public class TranslationString : AbstractTranslationString
{
    public TranslationString(string id, string value, string language, string comment) : base(id, value, language,
        comment)
    {
    }

    public TranslationString(string id, string value, string language) : base(id, value, language)
    {
    }

    public async Task WriteAsync(TextWriter writer)
    {
        await writer.WriteLineAsync($"{Id}={Escape(Value)}");
    }

    private static string Escape(string input)
    {
        return input.Replace("\n", "%CRLF");
    }
}
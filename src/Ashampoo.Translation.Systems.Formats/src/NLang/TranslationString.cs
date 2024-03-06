using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.NLang;

/// <summary>
/// Implementation of the <see cref="ITranslation"/> interface for the NLang format.
/// </summary>
public class TranslationString : AbstractTranslationString
{
    /// <inheritdoc />
    public TranslationString(string value, Language language, List<string> comment) : base(value, language,
        comment)
    {
    }

    /// <inheritdoc />
    public TranslationString(string value, Language language) : base(value, language, [])
    {
    }

    /// <summary>
    /// Asynchronously write the translation string to the given <see cref="TextWriter"/>.
    /// </summary>
    /// <param name="writer">
    /// The <see cref="TextWriter"/> to write to.
    /// </param>
    public async Task WriteAsync(TextWriter writer)
    {
        await writer.WriteLineAsync(Escape(Value));
    }

    private static string Escape(string input)
    {
        return input.Replace("\n", "%CRLF");
    }
}
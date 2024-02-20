using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.NLang;

/// <summary>
/// Implementation of the <see cref="ITranslation"/> interface for the NLang format.
/// </summary>
public class TranslationString : AbstractTranslationString
{
    /// <inheritdoc />
    public TranslationString(string id, string value, string language, string comment) : base(id, value, language,
        comment)
    {
    }

    /// <inheritdoc />
    public TranslationString(string id, string value, string language) : base(id, value, language)
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
        await writer.WriteLineAsync($"{Id}={Escape(Value)}");
    }

    private static string Escape(string input)
    {
        return input.Replace("\n", "%CRLF");
    }
}
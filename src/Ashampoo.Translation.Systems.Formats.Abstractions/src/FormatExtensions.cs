using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Provides extension methods for the <see cref="IFormat"/> interface.
/// </summary>
public static class FormatExtensions
{
    /// <summary>
    /// Imports translations from another format into the calling one
    /// </summary>
    /// <param name="format"></param>
    /// <param name="formatToImport"></param>
    /// <returns></returns>
    public static IList<ITranslation> ImportFrom(this IFormat format, ITranslationUnits formatToImport)
    {
        List<ITranslation> imported = new();
        foreach (var translationUnit in formatToImport)
        {
            foreach (var translation in translationUnit.Translations)
            {
                var id = translationUnit.Id;
                var language = translation.Language;
                var value = translation.Value;
                if (value is null) throw new Exception("Expected translation string");

                if (format[id] is null) continue;
                if (!format[id]!.Translations.TryGetTranslation(language, out var translationString)) continue;
                if (translationString.Value.Equals(value)) continue;

                translationString.Value = value;
                imported.Add(translation);
            }
        }

        return imported;
    }
}
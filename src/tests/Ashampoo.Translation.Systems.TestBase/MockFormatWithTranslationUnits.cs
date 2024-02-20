using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using IFormatProvider = Ashampoo.Translation.Systems.Formats.Abstractions.IFormatProvider;

namespace Ashampoo.Translation.Systems.TestBase;

public class MockFormatWithTranslationUnits : AbstractTranslationUnits, IFormat
{
    public static IFormat CreateMockFormatWithTranslationUnits(string language, string id, string value)
    {
        return new MockFormatWithTranslationUnits(language, id, value);
    }

    public MockFormatWithTranslationUnits()
    {
    }

    public MockFormatWithTranslationUnits(string language, string id, string value)
    {
        var translationString = new MockTranslationString(id: id, value: value, language: language);
        var translationUnit = new MockTranslationUnit(id: id)
        {
            Translations =
            {
                translationString
            }
        };
        Add(translationUnit);
    }

    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        throw new NotImplementedException();
    }

    public IFormatHeader Header { get; init; } = new MockHeader();

    public FormatLanguageCount LanguageCount => FormatLanguageCount.OnlyTarget;

    public void Add(string language, string id, string value)
    {
        var translationString = new MockTranslationString(id: id, value: value, language: language);

        var translationUnit = this[id] ?? new MockTranslationUnit(id: id);
        translationUnit.Translations.AddOrUpdateTranslation(language, translationString);
        Add(translationUnit);
    }

    public void Read(Stream stream, FormatReadOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public void Write(Stream stream)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        throw new NotImplementedException();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using IFormatProvider = Ashampoo.Translation.Systems.Formats.Abstractions.IFormatProvider;

namespace Ashampoo.Translation.Systems.TestBase;

public class MockFormatWithTranslationUnits : IFormat
{
    public static IFormat CreateMockFormatWithTranslationUnits(Language language, string id, string value)
    {
        return new MockFormatWithTranslationUnits(language, id, value);
    }

    private MockFormatWithTranslationUnits(Language language, string id, string value)
    {
        var translationString = new MockTranslationString(id: id, value: value, language: language);
        var translationUnit = new MockTranslationUnit(id: id)
        {
            Translations =
            {
                translationString
            }
        };
        TranslationUnits.Add(translationUnit);
    }

    public Task WriteAsync(Stream stream)
    {
        throw new NotImplementedException();
    }

    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        throw new NotImplementedException();
    }

    public IFormatHeader Header { get; init; } = new MockHeader();

    public LanguageSupport LanguageSupport => LanguageSupport.OnlyTarget;
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

    public void Read(Stream stream, FormatReadOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public void Write(Stream stream)
    {
        throw new NotImplementedException();
    }

    public Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        throw new NotImplementedException();
    }
}
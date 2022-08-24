using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

//FIXME: implement AbstractFormatHeader
public class AshLangFormatHeader : IFormatHeader
{
    public AshLangFormatHeader(LanguageChunk? languageChunk = null, XDataChunk? xDataChunk = null)
    {
        XDataChunk = xDataChunk ?? new XDataChunk();
        LanguageChunk = languageChunk ?? new LanguageChunk();
    }

    public XDataChunk XDataChunk { get; }
    public LanguageChunk LanguageChunk { get; }

    public string this[string key]
    {
        get => XDataChunk[key];
        set => XDataChunk[key] = value;
    }

    public string TargetLanguage
    {
        get => LanguageChunk.LanguageId;
        set
        {
            if (value is null) throw new ArgumentNullException(nameof(TargetLanguage));

            // TODO: set Language and Country
            LanguageChunk.LanguageId = value;
        }
    }

    public string? SourceLanguage
    {
        get => "en-US";
        set
        {
            // Do nothing. SourceLanguage is always "en-US"
        }
    }

    public ICollection<string> Keys => XDataChunk.Keys;

    public ICollection<string> Values => XDataChunk.Values;

    public int Count => XDataChunk.Count;

    public bool IsReadOnly => false;

    public void Add(string key, string value)
    {
        XDataChunk.Add(key, value);
    }

    public void Add(KeyValuePair<string, string> item)
    {
        XDataChunk.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        XDataChunk.Clear();
    }

    public bool Contains(KeyValuePair<string, string> item)
    {
        throw new NotImplementedException();
    }

    public bool ContainsKey(string key)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public bool Remove(string key)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<string, string> item)
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
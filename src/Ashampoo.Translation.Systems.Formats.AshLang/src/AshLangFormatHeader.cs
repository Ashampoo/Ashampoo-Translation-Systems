using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

//FIXME: implement AbstractFormatHeader
/// <summary>
/// Header for the <see cref="AshLangFormat"/>.
/// </summary>
public class AshLangFormatHeader : IFormatHeader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AshLangFormatHeader"/> class.
    /// </summary>
    /// <param name="languageChunk">
    /// The language chunk.
    /// </param>
    /// <param name="xDataChunk">
    /// The x data chunk.
    /// </param>
    public AshLangFormatHeader(LanguageChunk? languageChunk = null, XDataChunk? xDataChunk = null)
    {
        XDataChunk = xDataChunk ?? new XDataChunk();
        LanguageChunk = languageChunk ?? new LanguageChunk();
    }

    /// <summary>
    /// Gets the XDataChunk.
    /// </summary>
    public XDataChunk XDataChunk { get; }
    /// <summary>
    /// Gets the LanguageChunk.
    /// </summary>
    public LanguageChunk LanguageChunk { get; }

    /// <inheritdoc />
    public string this[string key]
    {
        get => XDataChunk[key];
        set => XDataChunk[key] = value;
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    public string? SourceLanguage
    {
        get => "en-US";
        set
        {
            // Do nothing. SourceLanguage is always "en-US"
        }
    }

    /// <inheritdoc />
    public ICollection<string> Keys => XDataChunk.Keys;

    /// <inheritdoc />
    public ICollection<string> Values => XDataChunk.Values;

    /// <inheritdoc />
    public int Count => XDataChunk.Count;

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void Add(string key, string value)
    {
        XDataChunk.Add(key, value);
    }

    /// <inheritdoc />
    public void Add(KeyValuePair<string, string> item)
    {
        XDataChunk.Add(item.Key, item.Value);
    }

    /// <inheritdoc />
    public void Clear()
    {
        XDataChunk.Clear();
    }

    /// <inheritdoc />
    public bool Contains(KeyValuePair<string, string> item)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool ContainsKey(string key)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return XDataChunk.GetEnumerator();
    }

    /// <inheritdoc />
    public bool Remove(string key)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool Remove(KeyValuePair<string, string> item)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return XDataChunk.GetEnumerator();
    }
}
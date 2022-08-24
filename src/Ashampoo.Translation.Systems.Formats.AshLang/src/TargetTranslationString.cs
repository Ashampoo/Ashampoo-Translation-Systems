using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.AshLang.Chunk;

namespace Ashampoo.Translation.Systems.Formats.AshLang;

public class TargetTranslationString : ITranslationString
{
    private readonly TranslationChunk.Translation translation;

    public TargetTranslationString(string language, TranslationChunk.Translation translation)
    {
        Language = language;
        this.translation = translation;
    }

    public string Value
    {
        get => translation.Value;
        set => translation.Value = value;
    }

    public string Id => translation.Id;

    public string? Comment
    {
        get => translation.Comment;
        set => translation.Comment = value ?? "";
    }

    public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

    public string Language { get; set; }
}
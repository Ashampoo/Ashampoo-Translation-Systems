using Ashampoo.Translations.Formats.Abstractions.Translation;

namespace Ashampoo.Translations.Formats.PO;

/// <summary>
/// Implementation of ITranslationString and a PO message string.
/// </summary>
public class MessageString : Message, ITranslationString
{
    /// <summary>
    /// Message string of the po format.
    /// </summary>
    public string MsgStr { get; set; } = "";

    /// <summary>
    /// Provides the value for the ITranslationString interface.
    /// </summary>
    public string Value
    {
        get => MsgStr;
        set => MsgStr = value;
    }

    public override bool IsEmpty => string.IsNullOrWhiteSpace(MsgStr);

    public MessageString(string id, string value, string language, string? comment = null, string msgCtxt = "")
    {
        MsgId = id;
        Value = value;
        Language = language;
        Comment = comment;
        MsgCtxt = msgCtxt;
    }

    public override async Task WriteAsync(TextWriter writer)
    {
        await base.WriteAsync(writer);
        await writer.WriteLineAsync($"{TypeMsgStr}\"{Escape(MsgStr)}\"");
    }
}
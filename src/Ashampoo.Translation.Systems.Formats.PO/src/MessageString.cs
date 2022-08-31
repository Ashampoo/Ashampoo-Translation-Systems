using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.PO;

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

    /// <inheritdoc />
    public override bool IsEmpty => string.IsNullOrWhiteSpace(MsgStr);

    /// <inheritdoc />
    public MessageString(string id, string value, string language, string? comment = null, string msgCtxt = "")
    {
        MsgId = id;
        Value = value;
        Language = language;
        Comment = comment;
        MsgCtxt = msgCtxt;
    }

    /// <inheritdoc />
    public override async Task WriteAsync(TextWriter writer)
    {
        await base.WriteAsync(writer);
        await writer.WriteLineAsync($"{TypeMsgStr}\"{Escape(MsgStr)}\"");
    }
}
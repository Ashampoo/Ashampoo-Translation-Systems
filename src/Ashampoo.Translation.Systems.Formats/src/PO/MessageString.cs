using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of <see cref="ITranslation"/> and a PO message string.
/// </summary>
public class MessageString : Message
{
    /// <summary>
    /// Message string of the po format.
    /// </summary>
    public string MsgStr { get; private set; } = "";

    /// <summary>
    /// Provides the value for the <see cref="ITranslation"/> interface.
    /// </summary>
    public override string Value
    {
        get => MsgStr;
        set => MsgStr = value;
    }

    /// <inheritdoc />
    public MessageString(string id, string value, Language language, IList<string> comments, string msgCtxt = "")
    {
        MsgId = id;
        Value = value;
        Language = language;
        Comments = comments;
        MsgCtxt = msgCtxt;
    }

    /// <inheritdoc />
    public override async Task WriteAsync(TextWriter writer)
    {
        await base.WriteAsync(writer);
        await writer.WriteLineAsync($"{POConstants.TypeMsgStr}\"{Escape(MsgStr)}\"");
    }
}
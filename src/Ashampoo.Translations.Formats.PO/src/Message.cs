using Ashampoo.Translations.Formats.Abstractions.Translation;

namespace Ashampoo.Translations.Formats.PO;

/// <summary>
/// Implementation of ITranslation and PO basic message type.
/// </summary>
public abstract class Message : ITranslation
{
    public const string TypeMsgCtxt = "msgctxt ";
    public const string TypeMsgId = "msgid ";
    public const string TypeMsgIdPlural = "msgid_plural ";
    public const string TypeMsgStr = "msgstr ";
    private const string Divider = "/"; // TODO: move to interface?

    /// <summary>
    /// Message id of the translation in the po format.
    /// </summary>
    public string MsgId { get; init; } = "";

    /// <summary>
    /// The context of the translation.
    /// </summary>
    public string MsgCtxt { get; init; } = "";

    /// <summary>
    /// Provides the id for the ITranslation interface.
    /// </summary>
    public string Id => !string.IsNullOrWhiteSpace(MsgCtxt) ? $"{MsgCtxt}{Divider}{MsgId}" : MsgId;

    /// <summary>
    /// Provides the comment for the ITranslation interface.
    /// </summary>
    public string? Comment { get; set; }

    public abstract bool IsEmpty { get; }

    public string Language { get; set; } = "";

    public void TextWriter(TextWriter writer)
    {
        WriteAsync(writer).Wait();
    }

    public virtual async Task WriteAsync(TextWriter writer)
    {
        if (!string.IsNullOrWhiteSpace(Comment)) await writer.WriteLineAsync($"{Escape(Comment)}");
        if (!string.IsNullOrWhiteSpace(MsgCtxt))
            await writer.WriteLineAsync($"{TypeMsgCtxt}\"{Escape(MsgCtxt)}\"");
        await writer.WriteLineAsync($"{TypeMsgId}\"{Escape(MsgId)}\"");
    }

    protected static string Escape(string input)
    {
        return input.Replace("\n", "\\n");
    }
}
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of ITranslation and PO basic message type.
/// </summary>
public abstract class Message : ITranslation
{
    /// <summary>
    /// Identifier for the message context.
    /// </summary>
    public const string TypeMsgCtxt = "msgctxt ";
    /// <summary>
    /// Identifier for the message id.
    /// </summary>
    public const string TypeMsgId = "msgid ";
    /// <summary>
    /// Identifier for the message id plural.
    /// </summary>
    public const string TypeMsgIdPlural = "msgid_plural ";
    /// <summary>
    /// Identifier for the message string.
    /// </summary>
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

    public string Value { get; set; }

    /// <summary>
    /// Provides the comment for the ITranslation interface.
    /// </summary>
    public string? Comment { get; set; }

    /// <inheritdoc />
    public abstract bool IsEmpty { get; }

    /// <inheritdoc />
    public string Language { get; set; } = "";

    /// <summary>
    /// Write the message to the given writer.
    /// </summary>
    /// <param name="writer">
    /// The writer to write the message to.
    /// </param>
    public void Write(TextWriter writer)
    {
        WriteAsync(writer).Wait();
    }

    /// <summary>
    /// Asynchronously write the message to the given writer.
    /// </summary>
    /// <param name="writer">
    /// The writer to write to.
    /// </param>
    public virtual async Task WriteAsync(TextWriter writer)
    {
        if (!string.IsNullOrWhiteSpace(Comment)) await writer.WriteLineAsync($"{Escape(Comment)}");
        if (!string.IsNullOrWhiteSpace(MsgCtxt))
            await writer.WriteLineAsync($"{TypeMsgCtxt}\"{Escape(MsgCtxt)}\"");
        await writer.WriteLineAsync($"{TypeMsgId}\"{Escape(MsgId)}\"");
    }

    /// <summary>
    /// Escape the given string.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    protected static string Escape(string input)
    {
        return input.Replace("\n", "\\n");
    }
}
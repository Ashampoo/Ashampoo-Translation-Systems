using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of <see cref="ITranslation"/> and a PO message string.
/// </summary>
public class MessageString : ITranslation
{
    /// <summary>
    /// Message id of the translation in the po format.
    /// </summary>
    public string MsgId { get; init; }

    /// <summary>
    /// The context of the translation.
    /// </summary>
    public string MsgCtxt { get; init; }

    /// <summary>
    /// Provides the id for the ITranslation interface.
    /// </summary>
    public string Id => !string.IsNullOrWhiteSpace(MsgCtxt)
        ? $"{MsgCtxt}{POConstants.Divider}{MsgId}"
        : MsgId;

    /// <summary>
    /// Provides the comment for the ITranslation interface.
    /// </summary>
    public IList<string> Comments { get; set; } = [];

    /// <inheritdoc />
    public Language Language { get; set; }

    /// <summary>
    /// Message string of the po format.
    /// </summary>
    public string MsgStr { get; private set; } = "";

    /// <summary>
    /// Provides the value for the <see cref="ITranslation"/> interface.
    /// </summary>
    public string Value
    {
        get => MsgStr;
        set => MsgStr = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <param name="language"></param>
    /// <param name="comments"></param>
    /// <param name="msgCtxt"></param>
    /// <param name="pipeSplitting"></param>
    public MessageString(string id, string value, Language language, IList<string> comments, string msgCtxt = "")
    {
        MsgId = id;
        Value = value;
        Language = language;
        Comments = comments;
        MsgCtxt = msgCtxt;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
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
        // TODO: Check if this is still working with multiple comments
        var comments = Comments.Where(c => c.StartsWith("# ") || c.StartsWith("#. ")).ToList();
        if (comments.Count != 0)
        {
            foreach (var comment in comments)
            {
                await writer.WriteLineAsync($"{Escape(comment)}");
            }
        }

        if (!string.IsNullOrWhiteSpace(MsgCtxt))
            await writer.WriteLineAsync($"{POConstants.TypeMsgCtxt}\"{Escape(MsgCtxt)}\"");
        await writer.WriteLineAsync($"{POConstants.TypeMsgId}\"{Escape(MsgId)}\"");
        await writer.WriteLineAsync($"{POConstants.TypeMsgStr}\"{Escape(MsgStr)}\"");
    }

    /// <summary>
    /// Escape the given string.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private static string Escape(string input)
    {
        return input.Replace("\n", "\\n").Replace("\"", "\\\"").Replace(@"\", @"\\");
    }
}
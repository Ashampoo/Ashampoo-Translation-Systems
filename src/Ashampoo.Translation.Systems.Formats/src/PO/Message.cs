using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of ITranslation and PO basic message type.
/// </summary>
public abstract class Message : ITranslation
{
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
    public string Id => !string.IsNullOrWhiteSpace(MsgCtxt) ? $"{MsgCtxt}{POConstants.Divider}{MsgId}" : MsgId;

    /// <inheritdoc />
    public abstract string Value { get; set; }

    /// <summary>
    /// Provides the comment for the ITranslation interface.
    /// </summary>
    public IList<string> Comments { get; set; } = [];

    /// <inheritdoc />
    public Language Language { get; set; } = Language.Empty;

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
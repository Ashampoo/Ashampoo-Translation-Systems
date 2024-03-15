namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// 
/// </summary>
public static class POConstants
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

    public const string Divider = "/"; // TODO: move to interface?
}
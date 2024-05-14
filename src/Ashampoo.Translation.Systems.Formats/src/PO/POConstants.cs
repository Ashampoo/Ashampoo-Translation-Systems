namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Provides constants for PO file formatting.
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

    /// <summary>
    /// Represents the divider used in PO file formatting.
    /// </summary>
    public const string Divider = "||";
}

using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// IFormatBuilderOptions for POFormat
/// </summary>
public sealed record PoBuilderOptions : IFormatBuilderOptions
{
    /// <summary>
    /// Disables splitting of the id into msgctxt and msgid if a pipe separator is detected.
    /// </summary>
    public bool SplitContextAndId { get; init; } = true;
};
using Ashampoo.Translation.Systems.Formats.Abstractions;

namespace Ashampoo.Translation.Systems.Formats.PO;

public sealed record PoBuilderOptions : IFormatBuilderOptions
{
    /// <summary>
    /// Disables splitting of the id into msgctxt and msgid if a pipe separator is detected.
    /// </summary>
    public bool PipeSplitting { get; init; } = true;
};
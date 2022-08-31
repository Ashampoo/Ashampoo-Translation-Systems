using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of the <see cref="ITranslationUnit"/> interface for the PO format.
/// </summary>
public class TranslationUnit : AbstractTranslationUnit
{
    /// <inheritdoc />
    public TranslationUnit(string id) : base(id)
    {
    }

    /// <summary>
    /// Asynchronously writes the translation unit to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name="writer">
    /// The <see cref="TextWriter"/> to write to.
    /// </param>
    public async Task WriteAsync(TextWriter writer)
    {
        if (this.FirstOrDefault() is Message message)
        {
            await message.WriteAsync(writer);
        }
    }
}
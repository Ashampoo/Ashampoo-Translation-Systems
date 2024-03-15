using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;

namespace Ashampoo.Translation.Systems.Formats.QT;

/// <summary>
/// Special implementation of the <see cref="AbstractTranslationString"/> abstract class for the QT format.
/// </summary>
public class QtTranslationString : AbstractTranslationString
{
    /// <summary>
    /// Gets or sets the type of the QT translation. 
    /// This property is used to determine the current state of the translation.
    /// By default, it is set to 'Finished'.
    /// </summary>
    public QtTranslationType Type { get; set; } = QtTranslationType.Finished;

    /// <inheritdoc />
    public QtTranslationString(string value, Language language, List<string> comment) : base(value, language, comment)
    {
    }
}

/// <summary>
/// Enum representing the different states a QT translation can be in.
/// </summary>
public enum QtTranslationType
{
    /// <summary>
    /// Represents a finished translation.
    /// </summary>
    Finished,

    /// <summary>
    /// Represents an unfinished translation.
    /// </summary>
    Unfinished,

    /// <summary>
    /// Represents an obsolete translation.
    /// </summary>
    Obsolete,

    /// <summary>
    /// Represents a vanished translation.
    /// </summary>
    Vanished
}
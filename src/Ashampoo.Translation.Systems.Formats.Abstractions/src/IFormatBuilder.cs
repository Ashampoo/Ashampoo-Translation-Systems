namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Interface for a builder for a <see cref="IFormat"/>.
/// </summary>
public interface IFormatBuilder
{
    /// <summary>
    /// Builds the instance of <see cref="IFormat"/>.
    /// </summary>
    /// <returns>
    /// The instance of <see cref="IFormat"/>.
    /// </returns>
    IFormat Build();
}
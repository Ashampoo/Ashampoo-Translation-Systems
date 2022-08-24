namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// The exception that is thrown, when an instance of <see cref="IFormat"/> is not valid for the current operation.
/// </summary>
public class UnsupportedFormatException : Exception
{
    /// <summary>
    /// The invalid format.
    /// </summary>
    public IFormat Format { get; }

    /// <inheritdoc />
    public UnsupportedFormatException(IFormat format, string? message)
        : base(message)
    {
        Format = format;
    }

    /// <inheritdoc />
    public UnsupportedFormatException(IFormat format, string? message, Exception? inner)
        : base(message, inner)
    {
        Format = format;
    }
}
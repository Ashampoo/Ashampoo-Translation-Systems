namespace Ashampoo.Translations.Formats.Abstractions;

public class UnsupportedFormatException : Exception
{
    public IFormat Format { get; }

    public UnsupportedFormatException(IFormat format, string? message)
        : base(message)
    {
        Format = format;
    }

    public UnsupportedFormatException(IFormat format, string? message, Exception? inner)
        : base(message, inner)
    {
        Format = format;
    }
}
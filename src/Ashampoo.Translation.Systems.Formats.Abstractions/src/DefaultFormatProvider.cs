namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Default implementation of the <see cref="System.IFormatProvider"/> interface .
/// </summary>
internal class DefaultFormatProvider : IFormatProvider
{
    public DefaultFormatProvider
    (
        string id,
        string[] supportedFileExtensions,
        Type formatType,
        Type formatBuilderType
    )
    {
        Id = id;
        SupportedFileExtensions = supportedFileExtensions;
        FormatType = formatType;
        FormatBuilderType = formatBuilderType;
    }

    public string Id { get; }

    public string[] SupportedFileExtensions { get; }

    public Type FormatType { get; }
    public Type FormatBuilderType { get; }

    public IFormat Create()
    {
        // create new instance from type
        return Activator.CreateInstance(FormatType) as IFormat ??
               throw new InvalidOperationException("Could not create instance of format.");
    }

    public IFormatBuilder GetFormatBuilder()
    {
        // create new instance from type
        return Activator.CreateInstance(FormatBuilderType) as IFormatBuilder ??
               throw new InvalidOperationException("Could not create instance of format builder.");
    }

    public bool SupportsFileName(string fileName)
    {
        return SupportedFileExtensions.Any(ext => fileName.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }
}
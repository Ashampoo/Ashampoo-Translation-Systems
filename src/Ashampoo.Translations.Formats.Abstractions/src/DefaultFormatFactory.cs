namespace Ashampoo.Translations.Formats.Abstractions;

/// <summary>
/// Default implementation of the <see cref="IFormatFactory"/> interface.
/// </summary>
public class DefaultFormatFactory : IFormatFactory
{
    private readonly Dictionary<string, IFormatProvider> formatProviders;

    public DefaultFormatFactory(IEnumerable<IFormatProvider> formatProviders)
    {
        this.formatProviders = formatProviders.ToDictionary(formatProvider => formatProvider.Id.ToLower(),
            formatProvider => formatProvider);
    }
    
    public IFormat CreateFormat(string formatId)
    {
        return formatProviders[formatId.ToLower()].Create();
    }
    
    /// <summary>
    /// Gets the format provider for the specified <see cref="IFormat"/> .
    /// </summary>
    /// <param name="format">
    /// The format.
    /// </param>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if the format provider for the specified format is not found.
    /// </exception>
    public IFormatProvider GetFormatProvider(IFormat format)
    {
        return TryGetFormatProvider(format) ?? throw new Exception("Format provider not found"); // TODO: throw specific exception
    }
    
    /// <summary>
    /// Gets the format provider for a format with the specified id.
    /// </summary>
    /// <param name="formatId">
    /// The format id.
    /// </param>
    /// <returns></returns>
    /// <exception cref="Exception">
    /// Thrown if the format provider for the specified format is not found.
    /// </exception>
    public IFormatProvider GetFormatProvider(string formatId)
    {
        return TryGetFormatProvider(formatId) ?? throw new Exception("Format provider not found"); // TODO: throw specific exception
    }
    
    /// <summary>
    /// Get all registered format providers.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IFormatProvider> GetFormatProviders() => formatProviders.Values;
    
    /// <summary>
    /// Try to create a format from a file, via the file name .
    /// </summary>
    /// <param name="fileName">
    /// The file name.
    /// </param>
    /// <returns>
    /// An instance of <see cref="IFormat"/> if the file name is supported by a format, otherwise null.
    /// </returns>
    public IFormat? TryCreateFormatByFileName(string fileName)
    {
        // Test every format provider to see if it can handle the file name. Return the first one that can.
        return formatProviders.Values.FirstOrDefault(provider => provider.SupportsFileName(fileName))?.Create();
    }
    public IFormatProvider? TryGetFormatProvider(IFormat format)
    {
        return formatProviders.Values.FirstOrDefault(provider => provider.FormatType == format.GetType());
    }

    public IFormatProvider? TryGetFormatProvider(string formatId)
    {
        return formatProviders.Values.FirstOrDefault(provider =>
            string.Equals(provider.Id, formatId, StringComparison.CurrentCultureIgnoreCase));
    }

    public IFormatProvider GetFormatProvider(Type formatType)
    {
        if (!typeof(IFormat).IsAssignableFrom(formatType))
            throw new ArgumentException($"Expected Type of IFormat, got: {formatType} instead.");
        return TryGetFormatProvider(formatType) ?? throw new Exception("Format provider not found"); // TODO: throw specific exception
    }

    public IFormatProvider? TryGetFormatProvider(Type formatType)
    {
        return formatProviders.Values.FirstOrDefault(provider => provider.FormatType == formatType);
    }
}
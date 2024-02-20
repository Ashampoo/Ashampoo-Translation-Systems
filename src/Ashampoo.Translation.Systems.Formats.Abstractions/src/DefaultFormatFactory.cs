namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Default implementation of the <see cref="IFormatFactory"/> interface.
/// </summary>
public class DefaultFormatFactory : IFormatFactory
{
    private readonly Dictionary<string, IFormatProvider> _formatProviders;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultFormatFactory"/> class.
    /// </summary>
    /// <param name="formatProviders">
    /// The format providers to use.
    /// </param>
    public DefaultFormatFactory(IEnumerable<IFormatProvider> formatProviders)
    {
        _formatProviders = formatProviders.ToDictionary(formatProvider => formatProvider.Id.ToLower(),
            formatProvider => formatProvider);
    }

    /// <inheritdoc />
    public IFormat CreateFormat(string formatId)
    {
        return _formatProviders[formatId.ToLower()].Create();
    }

    /// <inheritdoc />
    public IFormatProvider GetFormatProvider(IFormat format)
    {
        return TryGetFormatProvider(format) ?? throw new Exception("Format provider not found"); // TODO: throw specific exception
    }

    /// <inheritdoc />
    public IFormatProvider GetFormatProvider(string formatId)
    {
        return TryGetFormatProvider(formatId) ?? throw new Exception("Format provider not found"); // TODO: throw specific exception
    }

    /// <inheritdoc />
    public IEnumerable<IFormatProvider> GetFormatProviders() => _formatProviders.Values;

    /// <inheritdoc />
    public IFormat? TryCreateFormatByFileName(string fileName)
    {
        // Test every format provider to see if it can handle the file name. Return the first one that can.
        return _formatProviders.Values.FirstOrDefault(provider => provider.SupportsFileName(fileName))?.Create();
    }

    /// <inheritdoc />
    public IFormatProvider? TryGetFormatProvider(IFormat format)
    {
        return _formatProviders.Values.FirstOrDefault(provider => provider.FormatType == format.GetType());
    }

    /// <inheritdoc />
    public IFormatProvider? TryGetFormatProvider(string formatId)
    {
        return _formatProviders.Values.FirstOrDefault(provider =>
            string.Equals(provider.Id, formatId, StringComparison.CurrentCultureIgnoreCase));
    }


    /// <inheritdoc />
    public IFormatProvider GetFormatProvider(Type formatType)
    {
        if (!typeof(IFormat).IsAssignableFrom(formatType))
            throw new ArgumentException($"Expected Type of IFormat, got: {formatType} instead.");
        return TryGetFormatProvider(formatType) ?? throw new Exception( "Format provider not found"); // TODO: throw specific exception
    }


    /// <inheritdoc />
    public IFormatProvider? TryGetFormatProvider(Type formatType)
    {
        return _formatProviders.Values.FirstOrDefault(provider => provider.FormatType == formatType);
    }
}
namespace Ashampoo.Translation.Systems.Formats.Abstractions;

/// <summary>
/// Builder to create an <see cref="IFormatProvider"/>.
/// </summary>
public class FormatProviderBuilder
{
    private string formatProviderId = "";
    private string[] supportedFileExtensions = Array.Empty<string>();
    private Type formatType = default!;
    private Type formatBuilderType = default!;

    /// <summary>
    /// Sets the format provider id.
    /// </summary>
    /// <param name="id">
    /// The id to set.
    /// </param>
    /// <returns>
    /// The current instance of <see cref="FormatProviderBuilder"/> to allow chaining.
    /// </returns>
    public FormatProviderBuilder SetId(string id)
    {
        formatProviderId = id;
        return this;
    }

    /// <summary>
    /// Sets the supported file extensions.
    /// </summary>
    /// <param name="fileExtensions">
    /// The file extensions to set.
    /// </param>
    /// <returns>
    /// The current instance of <see cref="FormatProviderBuilder"/> to allow chaining.
    /// </returns>
    public FormatProviderBuilder SetSupportedFileExtensions(string[] fileExtensions)
    {
        this.supportedFileExtensions = fileExtensions;
        return this;
    }

    /// <summary>
    /// Sets the format type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the <see cref="IFormat"/>.
    /// </typeparam>
    /// <returns>
    /// The current instance of <see cref="FormatProviderBuilder"/> to allow chaining.
    /// </returns>
    public FormatProviderBuilder SetFormatType<T>() where T : IFormat
    {
        formatType = typeof(T);
        return this;
    }

    /// <summary>
    /// Sets the format builder type.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the <see cref="IFormatBuilder"/>.
    /// </typeparam>
    /// <returns>
    /// The current instance of <see cref="FormatProviderBuilder"/> to allow chaining.
    /// </returns>
    public FormatProviderBuilder SetFormatBuilder<T>() where T : IFormatBuilder
    {
        formatBuilderType = typeof(T);
        return this;
    }

    /// <summary>
    /// Builds the <see cref="IFormatProvider"/>.
    /// </summary>
    /// <returns>
    /// The <see cref="IFormatProvider"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the format provider id is not set, the format type is not set,
    /// or the supported file extensions are not set.
    /// </exception>
    public IFormatProvider Create()
    {
        if (string.IsNullOrEmpty(formatProviderId)) throw new InvalidOperationException("formatProviderId must be set");
        if (supportedFileExtensions is null || supportedFileExtensions.Length == 0)
            throw new InvalidOperationException("supportedFileExtensions must be set");
        if (formatType is null) throw new InvalidOperationException("formatType must be set");

        return new DefaultFormatProvider(formatProviderId, supportedFileExtensions, formatType, formatBuilderType);
    }
}
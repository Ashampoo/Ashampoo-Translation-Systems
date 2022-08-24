namespace Ashampoo.Translation.Systems.Formats.Abstractions;

public class FormatProviderBuilder
{
    private string formatProviderId = "";
    private string[] supportedFileExtensions = Array.Empty<string>();
    private Type formatType = default!;
    private Type formatBuilderType = default!;

    public FormatProviderBuilder SetId(string id)
    {
        formatProviderId = id;
        return this;
    }

    public FormatProviderBuilder SetSupportedFileExtensions(string[] fileExtensions)
    {
        this.supportedFileExtensions = fileExtensions;
        return this;
    }

    public FormatProviderBuilder SetFormatType<T>() where T : IFormat
    {
        formatType = typeof(T);
        return this;
    }

    public FormatProviderBuilder SetFormatBuilder<T>() where T : IFormatBuilder
    {
        formatBuilderType = typeof(T);
        return this;
    }

    public IFormatProvider Create()
    {
        if (string.IsNullOrEmpty(formatProviderId)) throw new InvalidOperationException("formatProviderId must be set");
        if (supportedFileExtensions is null || supportedFileExtensions.Length == 0)
            throw new InvalidOperationException("supportedFileExtensions must be set");
        if (formatType is null) throw new InvalidOperationException("formatType must be set");

        return new DefaultFormatProvider(formatProviderId, supportedFileExtensions, formatType, formatBuilderType);
    }
}
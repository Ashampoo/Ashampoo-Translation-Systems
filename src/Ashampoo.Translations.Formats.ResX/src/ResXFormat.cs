using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Formats.Abstractions.Translation;
using Ashampoo.Translations.Formats.ResX.Elements;
using Microsoft.Toolkit.Diagnostics;
using IFormatProvider = Ashampoo.Translations.Formats.Abstractions.IFormatProvider;

namespace Ashampoo.Translations.Formats.ResX;

/// <summary>
/// Implementation of <see cref="IFormat"/> interface for the ResX format.
/// </summary>
public class ResXFormat : AbstractTranslationUnits, IFormat
{
    public IFormatHeader Header { get; init; } = new DefaultFormatHeader();
    public FormatLanguageCount LanguageCount => FormatLanguageCount.OnlyTarget;

    public Root XmlRoot { get; private set; }

    public ResXFormat()
    {
        XmlRoot = new Root();
    }

    public ResXFormat(Root xmlRoot)
    {
        XmlRoot = xmlRoot;
    }

    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        var xmlSettings = new XmlReaderSettings
        {
            Async = true // Allow async reading
        };

        var xmlReader = XmlReader.Create(stream, xmlSettings);
        var xmlSerializer = new XmlSerializer(typeof(Root));
        XmlRoot = xmlSerializer.Deserialize(xmlReader) as Root ??
                  throw new UnsupportedFormatException(this, "Invalid schema");

        if (!await ConfigureOptions(options))
        {
            options!.IsCancelled = true;
            return;
        }

        ReadTranslations();
    }

    private async Task<bool> ConfigureOptions(FormatReadOptions? options)
    {
        if (string.IsNullOrWhiteSpace(options?.TargetLanguage))
        {
            ArgumentNullException.ThrowIfNull(options?.FormatOptionsCallback, nameof(options.FormatOptionsCallback));

            FormatStringOption targetLanguageOption = new("Target language", true);
            FormatOptions formatOptions = new()
            {
                Options = new FormatOption[]
                {
                    targetLanguageOption
                }
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // Ask user for target language
            if (formatOptions.IsCanceled) return false;

            Header.TargetLanguage = targetLanguageOption.Value;
        }
        else
        {
            Header.TargetLanguage = options.TargetLanguage;
        }

        return true;
    }

    private void ReadTranslations()
    {
        ArgumentNullException.ThrowIfNull(XmlRoot.Data);

        foreach (var data in XmlRoot.Data)
        {
            Guard.IsNotNullOrWhiteSpace(data.Name, nameof(data.Name));
            var id = data.Name; // TODO: check if id is valid
            var value = data.Value ?? string.Empty;
            var comment = data.Comment;

            var translationString = new DefaultTranslationString(id, value, Header.TargetLanguage, comment);
            var translationUnit = new DefaultTranslationUnit(id) { translationString };

            Add(translationUnit);
        }
    }

    public void Write(Stream stream)
    {
        WriteAsync(stream).Wait();
    }

    public async Task WriteAsync(Stream stream)
    {
        //Add an empty namespace and empty value
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        var xmlSettings = new XmlWriterSettings
        {
            Async = true,
            Indent = true,
            IndentChars = "\t",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            Encoding = Encoding.UTF8
        };
        var xmlWriter = XmlWriter.Create(stream, xmlSettings);
        var xmlSerializer = new XmlSerializer(typeof(Root));
        xmlSerializer.Serialize(xmlWriter, XmlRoot, ns);

        await xmlWriter.FlushAsync();
    }
    
    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        return builder => builder.SetId("resx")
            .SetSupportedFileExtensions(new[] { ".resx" })
            .SetFormatType<ResXFormat>()
            .SetFormatBuilder<ResXFormatBuilder>()
            .Create();
    }
}
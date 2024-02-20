using System.Xml;
using System.Xml.Serialization;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using Ashampoo.Translation.Systems.Formats.TsProj.Element;
using IFormatProvider = Ashampoo.Translation.Systems.Formats.Abstractions.IFormatProvider;

namespace Ashampoo.Translation.Systems.Formats.TsProj;

/// <summary>
/// Implementation of the <see cref="IFormat"/> interface for the TsProj format. 
/// </summary>
public class TsProjFormat : IFormat
{
    /// <summary>
    /// The project element for the xml structure.
    /// </summary>
    public Project Project { get; private set; }

    /// <inheritdoc />
    public IFormatHeader Header { get; init; } = new DefaultFormatHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.SourceAndTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

    /// <summary>
    /// Default constructor for the <see cref="TsProjFormat"/> class.
    /// </summary>
    public TsProjFormat()
    {
        Project = new Project();
    }

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        var xmlSettings = new XmlReaderSettings
        {
            Async = true
        };
        var xmlReader = XmlReader.Create(stream, xmlSettings);
        var mySerializer = new XmlSerializer(typeof(Project));

        Project = mySerializer.Deserialize(xmlReader) as Project ??
                  throw new UnsupportedFormatException(this, "No project found"); // Serialize file to Project object

        var configureSuccess = await ConfigureHeader(options); // Configure header
        if (!configureSuccess)
        {
            options!.IsCancelled = true;
            return;
        }

        if (Project.Components?.Count != 0 && Project.Translations?.Count != 0)
            throw new UnsupportedFormatException(this,
                "invalid use of components and translations"); // A project can only have one of each

        if (Project.Components?.Count > 0) ReadComponents(Project.Components);
        else if (Project.Translations?.Count > 0) ReadRootTranslations(Project.Translations);
    }

    private void ReadComponents(List<Component> components)
    {
        foreach (var component in components)
        {
            if (component.Translations is null) continue; // No translations for this component

            ReadRootTranslations(component.Translations);
        }
    }

    private void ReadRootTranslations(List<Element.Translation> translations)
    {
        foreach (var unit in ReadTranslations(translations))
        {
            TranslationUnits.Add(unit); // Add the translation unit to the hash set of translation units
        }
    }

    private IEnumerable<ITranslationUnit> ReadTranslations(
        IEnumerable<Element.Translation> translations)
    {
        return translations.Select(CreateTranslationString);
    }

    private ITranslationUnit CreateTranslationString(Element.Translation translation)
    {
        TranslationStringSource? source = null;

        if (!string.IsNullOrWhiteSpace(Header.SourceLanguage))
            source = new TranslationStringSource(translation)
            {
                Language = Header.SourceLanguage
            }; // Create a source translation string, if a source language is specified in the header

        var target = new TranslationStringTarget(translation) // Create a target translation string
        {
            Language = !string.IsNullOrWhiteSpace(Header.TargetLanguage)
                ? Header.TargetLanguage
                : throw new Exception("Target language is missing.")
        };

        var translationUnit = new DefaultTranslationUnit(translation.Id);
        if (source is not null)
            translationUnit.Translations.AddOrUpdateTranslation(source.Language, source); // Add the source translation if it exists

        translationUnit.Translations.AddOrUpdateTranslation(target.Language, target);
        return translationUnit;
    }

    private async Task<bool> ConfigureHeader(FormatReadOptions? options)
    {
        if (!string.IsNullOrWhiteSpace(Project.SourceLanguage))
            Header.SourceLanguage =
                Project.SourceLanguage; // Set the source language if it is specified in the project file

        if (!string.IsNullOrWhiteSpace(Project.TargetLanguage))
            Header.TargetLanguage =
                Project.TargetLanguage; // Set the target language if it is specified in the project file

        if (!string.IsNullOrWhiteSpace(Header.SourceLanguage) &&
            !string.IsNullOrWhiteSpace(Header.TargetLanguage))
            return true; // If both source and target languages are specified, return true


        var setTargetLanguage =
            string.IsNullOrWhiteSpace(options
                ?.TargetLanguage); // If the target language is not specified, ask the user to specify it
        var setSourceLanguage =
            string.IsNullOrWhiteSpace(options
                ?.SourceLanguage); // If the source language is not specified, ask the user to specify it
        if (setTargetLanguage || setSourceLanguage)
        {
            if (options?.FormatOptionsCallback is null)
                throw new InvalidOperationException(
                    "Callback for FormatOptions required."); // If the callback is null, throw an exception

            var targetLanguageOption =
                new FormatStringOption("TargetLanguage", true); // Create a new option for the target language
            var sourceLanguageOption =
                new FormatStringOption("SourceLanguage", true); // Create a new option for the source language

            List<FormatOption> optionList = new();
            if (setSourceLanguage) optionList.Add(sourceLanguageOption);
            if (setTargetLanguage) optionList.Add(targetLanguageOption);

            FormatOptions formatOptions = new()
            {
                Options = optionList.ToArray()
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // Invoke the callback with the options
            if (formatOptions.IsCanceled) return false;

            if (setSourceLanguage)
                Header.SourceLanguage =
                    sourceLanguageOption.Value; // Set the source language if it is specified in the options
            if (setTargetLanguage)
                Header.TargetLanguage =
                    targetLanguageOption.Value; // Set the target language if it is specified in the options
        }
        else
        {
            Header.TargetLanguage = options!.TargetLanguage!;
            Header.SourceLanguage = options.SourceLanguage;
        }

        return true;
    }

    /// <inheritdoc />
    public void Write(Stream stream)
    {
        WriteAsync(stream).Wait();
    }

    /// <summary>
    /// Asynchronously writes the current instance to the specified stream.
    /// </summary>
    /// <param name="stream">
    /// The stream to write to.
    /// </param>
    public async Task WriteAsync(Stream stream)
    {
        //Add an empty namespace and empty value
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        // We need a xml-writer to use settings.
        XmlWriterSettings settings = new()
        {
            Async = true,
            Indent = true,
            NewLineHandling = NewLineHandling.Replace,
            NewLineChars = "\r\n"
        };
        var writer = XmlWriter.Create(stream, settings); // Create a xml writer with the settings

        var xml = new XmlSerializer(typeof(Project)); // Create a xml serializer for the project
        xml.Serialize(writer, Project, ns); // Serialize the project to the writer

        await writer.FlushAsync(); 
    }

    /// <inheritdoc />
    public Func<FormatProviderBuilder, IFormatProvider> BuildFormatProvider()
    {
        return builder => builder.SetId("tsproj")
            .SetSupportedFileExtensions(new[] { ".tsproj" })
            .SetFormatType<TsProjFormat>()
            .SetFormatBuilder<TsProjFormatBuilder>()
            .Create();
    }
}
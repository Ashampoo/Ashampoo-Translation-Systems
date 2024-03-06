using System.Xml;
using System.Xml.Linq;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.QT;

public class QTFormat : IFormat
{
    /// <inheritdoc />
    public IFormatHeader Header { get; } = new DefaultFormatHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport { get; } = LanguageSupport.OnlyTarget;

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; } = [];

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        if (!await ConfigureOptionsAsync(options))
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNull(Header.TargetLanguage.Value);


        var doc = XDocument.Load(stream);
        var messageElements = doc.Descendants().Where(e => e.Name == "message");

        await ReadTranslations(messageElements);
    }

    private Task ReadTranslations(IEnumerable<XElement> elements)
    {
        foreach (var element in elements)
        {
            if (element.IsEmpty || element.NodeType is XmlNodeType.EndElement)
            {
                continue;
            }
            var sourceElement = element.Element("source");
            var translationElement = element.Element("translation");
            if (!string.IsNullOrWhiteSpace(sourceElement?.Value))
            {
                var unit = new DefaultTranslationUnit(sourceElement.Value);
                var translation = new QTTranslationString(translationElement?.Value ?? string.Empty, Header.TargetLanguage, []);
                unit.Translations.Add(translation);
                TranslationUnits.Add(unit);
                Console.WriteLine("Add translation with id {0} and value {1}", unit.Id, translation.Value);
            }
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task WriteAsync(Stream stream)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> ConfigureOptionsAsync(FormatReadOptions? options)
    {
        if (string.IsNullOrWhiteSpace(options?.TargetLanguage.Value))
        {
            Guard.IsNotNull(options?.FormatOptionsCallback);

            FormatStringOption targetLanguageOption = new("Target language", true);
            FormatOptions formatOptions = new()
            {
                Options =
                [
                    targetLanguageOption
                ]
            };

            await options.FormatOptionsCallback.Invoke(formatOptions); // Invoke callback
            if (formatOptions.IsCanceled) return false;

            Header.TargetLanguage = Language.Parse(targetLanguageOption.Value);
        }
        else
        {
            Header.TargetLanguage = options.TargetLanguage!;
        }

        return true;
    }
}
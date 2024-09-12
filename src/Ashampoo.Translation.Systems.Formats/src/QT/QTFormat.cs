using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Linq;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.QT;

/// <summary>
/// Implementation of <see cref="IFormat"/> interface for the QT format.
/// </summary>
public class QtFormat : IFormat
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

        ReadTranslations(messageElements);
    }

    private void ReadTranslations(IEnumerable<XElement> elements)
    {
        foreach (var element in elements)
        {
            if (element.IsEmpty || element.NodeType is XmlNodeType.EndElement)
            {
                continue;
            }

            var sourceElement = element.Element("source");
            var translationElement = element.Element("translation");
            var translationType = translationElement?.Attribute("type");
            var comments = GetComments(element);

            if (string.IsNullOrWhiteSpace(sourceElement?.Value)) continue;
            var unit = new DefaultTranslationUnit(sourceElement.Value);
            var translation = new QtTranslationString(translationElement?.Value ?? string.Empty,
                Header.TargetLanguage, comments);
            if (translationType is not null &&
                Enum.TryParse<QtTranslationType>(translationType.Value, true, out var result))
            {
                translation.Type = result;
            }

            unit.Translations.Add(translation);
            TranslationUnits.Add(unit);
        }
    }

    private List<string> GetComments(XElement element)
    {
        List<string> comments = [];
        if (element.TryGetElement("translatorcomment", out var translatorComment))
        {
            if (translatorComment?.Value != null) comments.Add(translatorComment.Value);
        }

        if (!element.TryGetElement("extracomment", out var extraComment)) return comments;
        if (extraComment?.Value != null) comments.Add(extraComment.Value);

        return comments;
    }

    /// <inheritdoc />
    public async Task WriteAsync(Stream stream, FormatWriteOptions? options = null)
    {
        var test = XmlWriter.Create(stream, new XmlWriterSettings
        {
            Async = true
        });
        await WriteTranslations(test);
    }

    [SuppressMessage("ReSharper", "MethodHasAsyncOverload")]
    private async Task WriteTranslations(XmlWriter writer)
    {
        await WriteTypeData(writer);
        await writer.WriteStartElementAsync(null, "context", null);
        foreach (var unit in TranslationUnits)
        {
            foreach (var translation in unit.Translations)
            {
                await writer.WriteStartElementAsync(null, "message", null);
                await writer.WriteStartElementAsync(null, "source", null);
                await writer.WriteStringAsync(unit.Id);
                await writer.WriteEndElementAsync();
                await WriteTranslationComments(writer, translation.Comments);
                await writer.WriteStartElementAsync(null, "translation", null);
                await writer.WriteStringAsync(translation.Value);
                await writer.WriteEndElementAsync();
                await writer.WriteEndElementAsync();
            }
        }

        await writer.WriteEndElementAsync();
        await writer.WriteEndElementAsync();
        await writer.FlushAsync();
        writer.Close();
    }

    private async Task WriteTypeData(XmlWriter writer)
    {
        await writer.WriteDocTypeAsync("TS", null, null, null);
        await writer.WriteStartElementAsync(null, "TS", null);
        await writer.WriteAttributeStringAsync(null, "version", null, "2.0");
        await writer.WriteAttributeStringAsync(null, "language", null, Header.TargetLanguage.Value.Replace('-', '_'));
    }

    private async Task WriteTranslationComments(XmlWriter writer, IList<string> comments)
    {
        if (!comments.Any()) return;
        await writer.WriteStartElementAsync(null, "translatorcomment", null);
        await writer.WriteStringAsync(string.Join(", ", comments));
        await writer.WriteEndElementAsync();
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

file static class XElementExtensions
{
    public static bool TryGetElement(this XElement element, XName name, out XElement? outElement)
    {
        outElement = element.Element(name);
        return outElement is not null;
    }
}
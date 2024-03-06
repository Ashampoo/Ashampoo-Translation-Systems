using System.Xml;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.QT;

public class QTFormat : IFormat
{
    /// <inheritdoc />
    public IFormatHeader Header { get; }

    /// <inheritdoc />
    public LanguageSupport LanguageSupport { get; }

    /// <inheritdoc />
    public ICollection<ITranslationUnit> TranslationUnits { get; }

    /// <inheritdoc />
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        if (!await ConfigureOptionsAsync(options))
        {
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNull(Header.TargetLanguage.Value);

        using var reader = XmlReader.Create(stream,
            new XmlReaderSettings() { Async = true, IgnoreComments = true, IgnoreWhitespace = true });

        await ReadTranslations(reader);
    }

    private async Task ReadTranslations(XmlReader reader)
    {
        while (await reader.ReadAsync())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    Console.WriteLine("Start Element {0}", reader.Name);
                    break;
                case XmlNodeType.Text:
                    Console.WriteLine("Text Node: {0}", await reader.GetValueAsync());
                    break;
                case XmlNodeType.EndElement:
                    Console.WriteLine("End Element {0}", reader.Name);
                    break;
                default:
                    Console.WriteLine("Other node {0} with value {1}", reader.NodeType, reader.Value);
                    break;
            }
        }
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
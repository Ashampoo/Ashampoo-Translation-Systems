using System.Text;
using System.Text.RegularExpressions;
using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.IO;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using Ashampoo.Translation.Systems.Formats.Abstractions.Translation;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.JavaProperties;

/// <summary>
/// Represents a Java properties file format.
/// </summary>
public partial class JavaPropertiesFormat : IFormat
{
    private static readonly Regex KeyValueRegex = MyRegex();

    /// <inheritdoc />
    public IFormatHeader Header { get; } = new DefaultFormatHeader();

    /// <inheritdoc />
    public LanguageSupport LanguageSupport => LanguageSupport.OnlyTarget;

    /// <inheritdoc/>
    public ICollection<ITranslationUnit> TranslationUnits { get; } = new List<ITranslationUnit>();

    /// <inheritdoc/>
    public void Read(Stream stream, FormatReadOptions? options = null)
    {
        ReadAsync(stream, options).Wait();
    }

    /// <inheritdoc/>
    public async Task ReadAsync(Stream stream, FormatReadOptions? options = null)
    {
        if (!await ConfigureOptionsAsync(options))
        {
            // TODO: Make options a not nullable param to avoid this "Not null" call
            options!.IsCancelled = true;
            return;
        }

        Guard.IsNotNullOrWhiteSpace(Header.TargetLanguage.Value);

        using StreamReader reader = new(stream);
        using LineReader lineReader = new(reader);

        await ReadTranslations(lineReader);
    }

    // TODO: Can this be made Protected in a abstract class to avoid duplicate code?
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

    private async Task ReadTranslations(LineReader reader)
    {
        await reader.SkipEmptyLinesAsync();
        while (await reader.HasMoreLinesAsync())
        {
            TranslationUnits.Add(ParseLine(await reader.ReadLineAsync(), reader.LineNumber));
        }
    }

    private ITranslationUnit ParseLine(string? line, int lineNumber)
    {
        Guard.IsNotNullOrWhiteSpace(line);

        var match = KeyValueRegex.Match(line);
        if (!match.Success)
            throw new UnsupportedFormatException(this, $"Unsupported line: {line} at line number {lineNumber}.");

        var id = match.Groups["key"].Value;
        var value = match.Groups["value"].Value;

        var translation = new DefaultTranslationString(id, value, Header.TargetLanguage);
        return new DefaultTranslationUnit(id)
        {
            Translations =
            {
                translation
            }
        };
    }

    /// <inheritdoc/>
    public void Write(Stream stream)
    {
        using StreamWriter writer = new(stream, leaveOpen: true);

        foreach (var translationUnit in TranslationUnits)
        {
            foreach (var translation in translationUnit.Translations)
            {
                writer.WriteLine($"{translationUnit.Id}={translation.Value}");
            }
        }

        writer.Flush();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stream"></param>
    public async Task WriteAsync(Stream stream)
    {
        await using StreamWriter writer = new(stream, leaveOpen: true);

        foreach (var translationUnit in TranslationUnits)
        {
            foreach (var translation in translationUnit.Translations)
            {
                await writer.WriteLineAsync($"{translationUnit.Id}={translation.Value}");
            }
        }

        await writer.FlushAsync();
    }

    [GeneratedRegex("(?<key>.*?)=(?<value>.*)")]
    private static partial Regex MyRegex();
}
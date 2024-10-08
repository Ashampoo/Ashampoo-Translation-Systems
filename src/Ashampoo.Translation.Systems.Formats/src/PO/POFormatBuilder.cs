using Ashampoo.Translation.Systems.Formats.Abstractions;
using Ashampoo.Translation.Systems.Formats.Abstractions.Models;
using CommunityToolkit.Diagnostics;

namespace Ashampoo.Translation.Systems.Formats.PO;

/// <summary>
/// Implementation of the <see cref="IFormatBuilderWithTarget{T}"/> for the PO format.
/// </summary>
public class POFormatBuilder : IFormatBuilderWithTarget<POFormat>
{
    private Language? _targetLanguage;
    private readonly Dictionary<string, string> _translations = new();

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        _translations.Add(id, target);
    }

    /// <inheritdoc />
    public POFormat Build(IFormatBuilderOptions? options = null)
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage?.Value, nameof(_targetLanguage));

        //Create new PO format and add translations
        var poFormat = new POFormat
        {
            Header =
            {
                TargetLanguage = _targetLanguage.Value
            }
        };

        var builderOptions = options as PoBuilderOptions ?? new PoBuilderOptions();
        foreach (var translation in _translations)
        {
            var translationUnit = new TranslationUnit(translation.Key);
            var index = translation.Key.IndexOf(POConstants.Divider, StringComparison.InvariantCulture);
            if (builderOptions.SplitContextAndId && index > 0) // if divider exists, then a message context is used
            {
                var ctxt = translation.Key[..index];
                var msgId = translation.Key[(index + POConstants.Divider.Length)..];
                translationUnit.Translations.Add(new MessageString(id: msgId, value: translation.Value,
                    language: _targetLanguage.Value,
                    msgCtxt: ctxt, comments: []));
            }
            else
            {
                translationUnit.Translations.Add(new MessageString(translation.Key, translation.Value,
                    _targetLanguage.Value, []));
            }

            poFormat.TranslationUnits.Add(translationUnit);
        }

        return poFormat;
    }

    /// <inheritdoc />
    public void SetHeaderInformation(IFormatHeader header)
    {
        //TODO: implement
    }

    /// <inheritdoc />
    public void AddHeaderInformation(string key, string value)
    {
        //TODO: implement
    }

    /// <inheritdoc />
    public void SetTargetLanguage(Language language)
    {
        _targetLanguage = language;
    }
}
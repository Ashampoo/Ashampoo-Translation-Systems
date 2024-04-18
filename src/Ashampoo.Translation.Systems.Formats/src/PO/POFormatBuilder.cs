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
    private const string Divider = "/"; // TODO: move to interface?

    /// <inheritdoc />
    public void Add(string id, string target)
    {
        _translations.Add(id, target);
    }

    /// <inheritdoc />
    public POFormat Build()
    {
        Guard.IsNotNullOrWhiteSpace(_targetLanguage?.Value, nameof(_targetLanguage));

        //Create new PO format and add translations
        var poFormat = new POFormat
        {
            Header =
            {
                TargetLanguage = (Language)_targetLanguage!
            }
        };

        foreach (var translation in _translations)
        {
            var translationUnit = new TranslationUnit(translation.Key);
            var index = translation.Key.LastIndexOf(Divider, StringComparison.Ordinal); 
            if (index > 0) // if divider exists, then a message context is used
            {
                var ctxt = translation.Key[..index];
                var msgId = translation.Key[(index + 1)..];
                translationUnit.Translations.Add(new MessageString(id: msgId, value: translation.Value, language: (Language)_targetLanguage,
                    msgCtxt: ctxt, comments: []));
            }
            else
                translationUnit.Translations.Add(new MessageString(translation.Key, translation.Value, (Language)_targetLanguage, []));

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
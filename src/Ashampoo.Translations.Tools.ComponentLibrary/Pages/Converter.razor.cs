using Ashampoo.Translations.Formats.Abstractions;
using Ashampoo.Translations.Logging.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Ashampoo.Translations.Tools.ComponentLibrary.Pages;

[AllowAnonymous]
public partial class Converter : ComponentBase
{
    [Inject] private IMediator Mediator { get; init; } = default!;

    private IFormat? format;
    private string fileName = "";

    /// <summary>
    /// Callback to be invoked when the user uploaded a format.
    /// </summary>
    /// <param name="args"></param>
    private async Task FormatUploaded((IFormat? format, string fileName) args)
    {
        format = args.format;
        fileName = args.fileName;

        await Mediator.Publish(new FormatUploadedNotification());
    }

    /// <summary>
    /// Callback to be invoked when the user imported a format.
    /// </summary>
    /// <param name="args"></param>
    private async Task FormatImported((IFormat? format, string fileName) args)
    {
        if (format is null || args.format is null) return;

        var imported = format.ImportFrom(args.format);

        await Mediator.Publish(new ImportedTranslationsNotification(imported.Count));
    }
}
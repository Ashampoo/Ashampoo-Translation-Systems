using Ashampoo.Translation.Systems.Components.Notifications;
using MediatR;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Components.NotificationHandlers;

/// <summary>
/// Handler for the <see cref="ImportedTranslationsNotification"/>.
/// </summary>
public class ImportedTranslationsNotificationHandler : INotificationHandler<ImportedTranslationsNotification>
{
    private readonly ISnackbar snackbar;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportedTranslationsNotificationHandler"/> class.
    /// </summary>
    /// <param name="snackbar">
    /// The snackbar.
    /// </param>
    public ImportedTranslationsNotificationHandler(ISnackbar snackbar)
    {
        this.snackbar = snackbar;
    }

    /// <inheritdoc />
    public Task Handle(ImportedTranslationsNotification notification, CancellationToken cancellationToken)
    {
        switch (notification.Count)
        {
            case 0:
                snackbar.Add("No new translations were imported.", Severity.Warning);
                break;
            case 1:
                snackbar.Add("1 new translations was imported.", Severity.Success);
                break;
            default:
                snackbar.Add($"{notification.Count} new translations were imported.", Severity.Success);
                break;
        }

        return Task.CompletedTask;
    }
}
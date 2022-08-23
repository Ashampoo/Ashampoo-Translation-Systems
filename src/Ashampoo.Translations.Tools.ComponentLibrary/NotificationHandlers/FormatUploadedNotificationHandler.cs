using Ashampoo.Translations.Logging.Notifications;
using MediatR;
using MudBlazor;

namespace Ashampoo.Translations.Tools.ComponentLibrary.NotificationHandlers;

public class FormatUploadedNotificationHandler : INotificationHandler<FormatUploadedNotification>
{
    private readonly ISnackbar snackbar;

    public FormatUploadedNotificationHandler(ISnackbar snackbar)
    {
        this.snackbar = snackbar;
    }

    public Task Handle(FormatUploadedNotification notification, CancellationToken cancellationToken)
    {
        snackbar.Add("Uploaded Format.", Severity.Success);

        return Task.CompletedTask;
    }
}
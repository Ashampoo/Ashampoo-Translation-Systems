using Ashampoo.Translation.Systems.Logging.Notifications;
using Ashampoo.Translation.Systems.Tools.ComponentLibrary.Notifications;
using MediatR;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Tools.ComponentLibrary.NotificationHandlers;

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
using Ashampoo.Translation.Systems.Components.Notifications;
using MediatR;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Components.NotificationHandlers;

public class SimpleWarningNotificationHandler : INotificationHandler<SimpleWarning>
{
    private readonly ISnackbar snackbar;

    public SimpleWarningNotificationHandler(ISnackbar snackbar)
    {
        this.snackbar = snackbar;
    }

    public Task Handle(SimpleWarning notification, CancellationToken cancellationToken)
    {
        snackbar.Add(notification.Message, Severity.Warning);

        return Task.CompletedTask;
    }
}
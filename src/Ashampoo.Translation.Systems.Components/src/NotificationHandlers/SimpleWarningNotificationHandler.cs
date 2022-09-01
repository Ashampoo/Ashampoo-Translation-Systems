using Ashampoo.Translation.Systems.Components.Notifications;
using MediatR;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Components.NotificationHandlers;

/// <summary>
/// Handler for the <see cref="SimpleWarning"/>.
/// </summary>
public class SimpleWarningNotificationHandler : INotificationHandler<SimpleWarning>
{
    private readonly ISnackbar snackbar;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleWarningNotificationHandler"/> class.
    /// </summary>
    /// <param name="snackbar">
    /// The snackbar.
    /// </param>
    public SimpleWarningNotificationHandler(ISnackbar snackbar)
    {
        this.snackbar = snackbar;
    }

    /// <inheritdoc />
    public Task Handle(SimpleWarning notification, CancellationToken cancellationToken)
    {
        snackbar.Add(notification.Message, Severity.Warning);

        return Task.CompletedTask;
    }
}
using Ashampoo.Translation.Systems.Components.Notifications;
using MediatR;
using MudBlazor;

namespace Ashampoo.Translation.Systems.Components.NotificationHandlers;

/// <summary>
/// Handler for the <see cref="FormatUploadedNotification"/>.
/// </summary>
public class FormatUploadedNotificationHandler : INotificationHandler<FormatUploadedNotification>
{
    private readonly ISnackbar snackbar;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormatUploadedNotificationHandler"/> class.
    /// </summary>
    /// <param name="snackbar">
    /// The snackbar.
    /// </param>
    public FormatUploadedNotificationHandler(ISnackbar snackbar)
    {
        this.snackbar = snackbar;
    }

    /// <inheritdoc />
    public Task Handle(FormatUploadedNotification notification, CancellationToken cancellationToken)
    {
        snackbar.Add("Uploaded Format.", Severity.Success);

        return Task.CompletedTask;
    }
}
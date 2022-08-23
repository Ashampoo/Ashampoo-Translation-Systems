using MediatR;

namespace Ashampoo.Translations.Logging.Notifications;

public record SimpleWarning(string Message) : INotification;
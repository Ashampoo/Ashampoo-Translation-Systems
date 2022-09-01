using MediatR;

namespace Ashampoo.Translation.Systems.Logging.Notifications;

public record SimpleWarning(string Message) : INotification;
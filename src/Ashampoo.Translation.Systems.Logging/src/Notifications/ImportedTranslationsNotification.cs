using MediatR;

namespace Ashampoo.Translation.Systems.Logging.Notifications;

public record ImportedTranslationsNotification(int Count) : INotification;
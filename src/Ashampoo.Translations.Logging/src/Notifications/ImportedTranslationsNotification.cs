using MediatR;

namespace Ashampoo.Translations.Logging.Notifications;

public record ImportedTranslationsNotification(int Count) : INotification;
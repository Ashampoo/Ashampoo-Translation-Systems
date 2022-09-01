using MediatR;

namespace Ashampoo.Translation.Systems.Components.Notifications;

public record ImportedTranslationsNotification(int Count) : INotification;
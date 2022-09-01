using MediatR;

namespace Ashampoo.Translation.Systems.Components.Notifications;

public record SimpleWarning(string Message) : INotification;
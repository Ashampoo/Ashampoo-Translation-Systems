using MediatR;

namespace Ashampoo.Translation.Systems.Components.Notifications;

/// <summary>
/// Notification to display a simple warning message.
/// </summary>
/// <param name="Message">
/// The message to display.
/// </param>
public record SimpleWarning(string Message) : INotification;
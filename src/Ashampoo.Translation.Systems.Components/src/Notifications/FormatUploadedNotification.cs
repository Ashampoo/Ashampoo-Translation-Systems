using MediatR;

namespace Ashampoo.Translation.Systems.Components.Notifications;

/// <summary>
/// Notification for when a format was uploaded.
/// </summary>
public record FormatUploadedNotification : INotification;
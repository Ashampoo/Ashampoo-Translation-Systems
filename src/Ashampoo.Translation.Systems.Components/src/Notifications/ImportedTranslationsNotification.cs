using MediatR;

namespace Ashampoo.Translation.Systems.Components.Notifications;

/// <summary>
/// Notification for when a format was imported successfully.
/// </summary>
/// <param name="Count">
/// The number of translations imported.
/// </param>
public record ImportedTranslationsNotification(int Count) : INotification;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Notifications;
using Presentation.Common;
using Unity.Properties;

namespace Presentation.Screens.Settings
{
    public sealed class SettingsViewModel : BindableViewModel, IDisposable
    {
        private readonly INotificationService _notifications;
        private readonly CancellationTokenSource _cts = new();

        private bool _notificationsEnabled = true;
        private bool _isUpdating;

        public SettingsViewModel(INotificationService notifications) => _notifications = notifications;

        [CreateProperty]
        public bool NotificationsEnabled => _notificationsEnabled;

        [CreateProperty]
        public bool IsUpdating => _isUpdating;

        [CreateProperty]
        public string NotificationActionText => _isUpdating ? "Updating…" : _notificationsEnabled ? "Disable notifications" : "Enable notifications";

        [CreateProperty]
        public string NotificationStatusText => _notificationsEnabled ? "Enabled" : "Disabled";

        public async Task ToggleNotificationsAsync()
        {
            if (_isUpdating)
            {
                return;
            }

            var cancellationToken = _cts.Token;
            var enabled = !_notificationsEnabled;
            SetUpdating(true);

            try
            {
                await _notifications.SetEnabledAsync(enabled, cancellationToken);

                if (SetProperty(ref _notificationsEnabled, enabled, nameof(NotificationsEnabled)))
                {
                    NotifyPropertyChanged(nameof(NotificationActionText));
                    NotifyPropertyChanged(nameof(NotificationStatusText));
                }
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // Screen lifetime ended while the request was in flight.
            }
            finally
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    SetUpdating(false);
                }
            }
        }

        private void SetUpdating(bool value)
        {
            if (SetProperty(ref _isUpdating, value, nameof(IsUpdating)))
            {
                NotifyPropertyChanged(nameof(NotificationActionText));
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}

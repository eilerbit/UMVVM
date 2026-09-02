using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Adapters;

namespace Application.Notifications
{
    /// <summary>
    /// Application-facing feature composed from multiple external integrations.
    /// The application knows capabilities; adapters know SDK/protocol details.
    /// </summary>
    public sealed class NotificationService : INotificationService
    {
        private readonly IHttpAdapter _http;
        private readonly IPushAdapter _push;

        public NotificationService(IHttpAdapter http, IPushAdapter push)
        {
            _http = http;
            _push = push;
        }

        public async Task SetEnabledAsync(bool enabled, CancellationToken cancellationToken = default)
        {
            await _http.SetNotificationPreferenceAsync(enabled, cancellationToken);
            await _push.SetEnabledAsync(enabled, cancellationToken);
        }
    }
}

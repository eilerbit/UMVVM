using System.Threading;
using System.Threading.Tasks;
using Application.Users;

namespace Infrastructure.Adapters
{
    /// <summary>Adapts an external HTTP client/package to the application.</summary>
    public interface IHttpAdapter
    {
        Task<UserProfile> GetCurrentUserAsync(CancellationToken cancellationToken = default);
        Task SetNotificationPreferenceAsync(bool enabled, CancellationToken cancellationToken = default);
    }
}

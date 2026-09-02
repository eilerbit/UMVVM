using System.Threading;
using System.Threading.Tasks;
using Application.Users;

namespace Infrastructure.Adapters
{
    public sealed class MockHttpAdapter : IHttpAdapter
    {
        public async Task<UserProfile> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            await Task.Delay(500, cancellationToken);
            return new UserProfile("Alex Morgan", "Unity Engineer", "alex@example.com");
        }

        public Task SetNotificationPreferenceAsync(bool enabled, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}

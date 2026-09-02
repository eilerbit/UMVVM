using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Adapters
{
    public sealed class MockPushAdapter : IPushAdapter
    {
        public Task SetEnabledAsync(bool enabled, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Adapters
{
    /// <summary>Adapts an external push SDK (Firebase, OneSignal, APNs wrapper, etc.).</summary>
    public interface IPushAdapter
    {
        Task SetEnabledAsync(bool enabled, CancellationToken cancellationToken = default);
    }
}

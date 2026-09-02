using System.Threading;
using System.Threading.Tasks;

namespace Application.Notifications
{
    public interface INotificationService
    {
        Task SetEnabledAsync(bool enabled, CancellationToken cancellationToken = default);
    }
}

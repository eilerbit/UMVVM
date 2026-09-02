using System.Threading;
using System.Threading.Tasks;

namespace Application.Users
{
    public interface IUserService
    {
        Task<UserProfile> GetCurrentUserAsync(CancellationToken cancellationToken = default);
    }
}

using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Adapters;

namespace Application.Users
{
    /// <summary>Application-facing user functionality. External details stay behind adapters.</summary>
    public sealed class UserService : IUserService
    {
        private readonly IHttpAdapter _http;

        public UserService(IHttpAdapter http) => _http = http;

        public Task<UserProfile> GetCurrentUserAsync(CancellationToken cancellationToken = default) =>
            _http.GetCurrentUserAsync(cancellationToken);
    }
}

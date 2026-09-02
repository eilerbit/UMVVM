using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Users;
using Presentation.Common;
using Unity.Properties;

namespace Presentation.Screens.Profile
{
    public sealed class ProfileViewModel : BindableViewModel, IDisposable
    {
        private readonly IUserService _userService;
        private readonly CancellationTokenSource _cts = new();

        private string _name = "Loading…";
        private string _role = string.Empty;
        private string _email = string.Empty;
        private bool _isLoading = true;

        public ProfileViewModel(IUserService userService) => _userService = userService;

        [CreateProperty]
        public string Name => _name;

        [CreateProperty]
        public string Role => _role;

        [CreateProperty]
        public string Email => _email;

        [CreateProperty]
        public bool IsLoading => _isLoading;

        public async Task LoadAsync()
        {
            var cancellationToken = _cts.Token;

            try
            {
                SetProperty(ref _isLoading, true, nameof(IsLoading));

                var user = await _userService.GetCurrentUserAsync(cancellationToken);
                SetProperty(ref _name, user.DisplayName, nameof(Name));
                SetProperty(ref _role, user.Role, nameof(Role));
                SetProperty(ref _email, user.Email, nameof(Email));
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                // Screen lifetime ended while the request was in flight.
            }
            finally
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    SetProperty(ref _isLoading, false, nameof(IsLoading));
                }
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}

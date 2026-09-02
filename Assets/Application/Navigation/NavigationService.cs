using System;
using Presentation.Navigation;
using R3;

namespace Application.Navigation
{
    public sealed class NavigationService : INavigationService, IDisposable
    {
        private readonly ReactiveProperty<AppRoute> _currentRoute = new(AppRoute.Home);
        public ReadOnlyReactiveProperty<AppRoute> CurrentRoute => _currentRoute;

        public void Navigate(AppRoute route)
        {
            if (_currentRoute.Value != route)
                _currentRoute.Value = route;
        }

        public void Dispose() => _currentRoute.Dispose();
    }
}

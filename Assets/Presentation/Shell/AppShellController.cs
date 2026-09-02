using System;
using Application.Navigation;
using Presentation.Navigation;
using R3;
using UnityEngine.UIElements;
using VContainer.Unity;

namespace Presentation.Shell
{
    public sealed class AppShellController : IInitializable, IDisposable
    {
        private readonly AppShellView _view;
        private readonly INavigationService _navigation;
        private readonly ScreenFactory _screenFactory;

        private IDisposable _routeSubscription;
        private ScreenInstance _currentScreen;
        private Button _homeButton;
        private Button _profileButton;
        private Button _settingsButton;

        public AppShellController(AppShellView view, INavigationService navigation, ScreenFactory screenFactory)
        {
            _view = view;
            _navigation = navigation;
            _screenFactory = screenFactory;
        }

        public void Initialize()
        {
            _view.Reloaded += OnViewReloaded;
            BindView();
        }

        private void OnViewReloaded() => BindView();

        private void BindView()
        {
            _currentScreen?.Dispose();
            _currentScreen = null;

            UnbindNavigationButtons();

            // Keep references to the exact elements we subscribed to. AppShellView replaces
            // its element references before raising Reloaded when PanelRenderer rebuilds the UI.
            _homeButton = _view.HomeButton;
            _profileButton = _view.ProfileButton;
            _settingsButton = _view.SettingsButton;

            _homeButton.clicked += NavigateHome;
            _profileButton.clicked += NavigateProfile;
            _settingsButton.clicked += NavigateSettings;

            _routeSubscription?.Dispose();
            _routeSubscription = _navigation.CurrentRoute.Subscribe(RenderRoute);
        }

        private void UnbindNavigationButtons()
        {
            if (_homeButton != null)
            {
                _homeButton.clicked -= NavigateHome;
                _homeButton = null;
            }

            if (_profileButton != null)
            {
                _profileButton.clicked -= NavigateProfile;
                _profileButton = null;
            }

            if (_settingsButton != null)
            {
                _settingsButton.clicked -= NavigateSettings;
                _settingsButton = null;
            }
        }

        private void NavigateHome() =>
            _navigation.Navigate(AppRoute.Home);

        private void NavigateProfile() =>
            _navigation.Navigate(AppRoute.Profile);

        private void NavigateSettings() =>
            _navigation.Navigate(AppRoute.Settings);

        private void RenderRoute(AppRoute route)
        {
            _currentScreen?.Dispose();
            _view.ScreenHost.Clear();
            _currentScreen = _screenFactory.Create(route);
            _view.ScreenHost.Add(_currentScreen.Root);

            _view.HomeButton.EnableInClassList("nav-button--active", route == AppRoute.Home);
            _view.ProfileButton.EnableInClassList("nav-button--active", route == AppRoute.Profile);
            _view.SettingsButton.EnableInClassList("nav-button--active", route == AppRoute.Settings);
        }

        public void Dispose()
        {
            _view.Reloaded -= OnViewReloaded;

            _currentScreen?.Dispose();
            _currentScreen = null;

            UnbindNavigationButtons();

            _routeSubscription?.Dispose();
            _routeSubscription = null;
        }
    }
}

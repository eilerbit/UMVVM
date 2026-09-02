using Application.Navigation;
using Application.Notifications;
using Application.Users;
using Infrastructure.Adapters;
using Presentation.Navigation;
using Presentation.Screens.Home;
using Presentation.Screens.Profile;
using Presentation.Screens.Settings;
using Presentation.Shell;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Composition
{
    public static class Bootstrap
    {
        private static LifetimeScope _scope;
        private static PanelRenderer _panelRenderer;
        private static AppShellView _shellView;
        private static int _uiVersion = int.MinValue;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (_panelRenderer != null)
            {
                return;
            }

            var shellAsset = Resources.Load<VisualTreeAsset>("Shell/AppShell");
            var panelSettings = Resources.Load<PanelSettings>("UI/MainPanelSettings");

            if (shellAsset == null)
            {
                throw new MissingReferenceException("Missing Resources/Shell/AppShell.uxml");
            }

            if (panelSettings == null)
            {
                throw new MissingReferenceException("Missing Resources/UI/MainPanelSettings.asset");
            }

            var host = new GameObject("App");
            Object.DontDestroyOnLoad(host);

            _panelRenderer = host.AddComponent<PanelRenderer>();
            _panelRenderer.panelSettings = panelSettings;
            _panelRenderer.visualTreeAsset = shellAsset;
            _panelRenderer.sortingOrder = 0;
            _panelRenderer.RegisterUIReloadCallback(OnUiReload);
        }

        private static void OnUiReload(PanelRenderer panelRenderer, VisualElement root, int version)
        {
            if (_uiVersion == version)
            {
                return;
            }

            _uiVersion = version;

            if (_scope == null)
            {
                BuildApplication(root);
                return;
            }

            // PanelRenderer preserves its UI and can reload the visual tree in the Editor.
            // Rebind the shell so live reload never leaves the controller pointing at stale elements.
            _shellView.Reload(root);
        }

        private static void BuildApplication(VisualElement root)
        {
            _shellView = new AppShellView(root);

            _scope = LifetimeScope.Create(builder =>
            {
                builder.RegisterInstance(_shellView);

                builder.Register<MockHttpAdapter>(Lifetime.Singleton).As<IHttpAdapter>();
                builder.Register<MockPushAdapter>(Lifetime.Singleton).As<IPushAdapter>();

                builder.Register<UserService>(Lifetime.Singleton).As<IUserService>();
                builder.Register<NotificationService>(Lifetime.Singleton).As<INotificationService>();

                builder.Register<NavigationService>(Lifetime.Singleton).As<INavigationService>();
                builder.Register<ScreenFactory>(Lifetime.Singleton);

                builder.Register<HomeViewModel>(Lifetime.Transient);
                builder.Register<HomeBinder>(Lifetime.Transient);
                builder.Register<ProfileViewModel>(Lifetime.Transient);
                builder.Register<ProfileBinder>(Lifetime.Transient);
                builder.Register<SettingsViewModel>(Lifetime.Transient);
                builder.Register<SettingsBinder>(Lifetime.Transient);

                builder.RegisterEntryPoint<AppShellController>();
            }, "Root");

            _scope.Build();
            Object.DontDestroyOnLoad(_scope.gameObject);
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStaticState()
        {
            _scope = null;
            _panelRenderer = null;
            _shellView = null;
            _uiVersion = int.MinValue;
        }
    }
}

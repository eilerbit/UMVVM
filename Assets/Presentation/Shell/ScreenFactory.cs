using System;
using Presentation.Common;
using Presentation.Navigation;
using Presentation.Screens.Home;
using Presentation.Screens.Profile;
using Presentation.Screens.Settings;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Presentation.Shell
{
    public sealed class ScreenFactory
    {
        private readonly IObjectResolver _resolver;
        public ScreenFactory(IObjectResolver resolver) => _resolver = resolver;

        public ScreenInstance Create(AppRoute route)
        {
            var assetName = route.ToString();
            var asset = Resources.Load<VisualTreeAsset>($"Screens/{assetName}");
            if (asset == null) throw new InvalidOperationException($"Missing screen UXML: Resources/Screens/{assetName}.uxml");

            IScreenBinder binder = route switch
            {
                AppRoute.Home => _resolver.Resolve<HomeBinder>(),
                AppRoute.Profile => _resolver.Resolve<ProfileBinder>(),
                AppRoute.Settings => _resolver.Resolve<SettingsBinder>(),
                _ => throw new ArgumentOutOfRangeException(nameof(route), route, null)
            };

            // CloneTree() without a target returns a TemplateContainer. That extra
            // container is content-sized by default and can prevent the routed screen
            // from filling ScreenHost. Clone directly into our own stretchable root.
            var root = new VisualElement();
            root.AddToClassList("screen-instance");
            asset.CloneTree(root);

            binder.Bind(root);
            return new ScreenInstance(root, binder);
        }
    }

    public sealed class ScreenInstance : IDisposable
    {
        private readonly IScreenBinder _binder;
        public ScreenInstance(VisualElement root, IScreenBinder binder) { Root = root; _binder = binder; }
        public VisualElement Root { get; }
        public void Dispose() => _binder.Dispose();
    }
}

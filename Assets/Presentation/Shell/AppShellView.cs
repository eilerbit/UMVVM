using System;
using UnityEngine.UIElements;

namespace Presentation.Shell
{
    public sealed class AppShellView
    {
        public AppShellView(VisualElement root) => Bind(root);

        public event Action Reloaded;

        public VisualElement Root { get; private set; }
        public VisualElement ScreenHost { get; private set; }
        public Button HomeButton { get; private set; }
        public Button ProfileButton { get; private set; }
        public Button SettingsButton { get; private set; }

        public void Reload(VisualElement root)
        {
            Bind(root);
            Reloaded?.Invoke();
        }

        private void Bind(VisualElement root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));

            // PanelRenderer gives us the runtime root through its reload callback.
            // Explicitly let the generated root consume the complete panel viewport.
            Root.style.flexGrow = 1;
            Root.style.minWidth = 0;
            Root.style.minHeight = 0;

            ScreenHost = Require<VisualElement>(root, "screen-host");
            HomeButton = Require<Button>(root, "nav-home");
            ProfileButton = Require<Button>(root, "nav-profile");
            SettingsButton = Require<Button>(root, "nav-settings");
        }

        private static T Require<T>(VisualElement root, string name) where T : VisualElement
        {
            return root.Q<T>(name)
                ?? throw new InvalidOperationException($"Required UI element '{name}' ({typeof(T).Name}) was not found in AppShell.");
        }
    }
}

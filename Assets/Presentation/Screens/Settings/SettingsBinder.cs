using System;
using Presentation.Common;
using UnityEngine.UIElements;

namespace Presentation.Screens.Settings
{
    public sealed class SettingsBinder : IScreenBinder
    {
        private readonly SettingsViewModel _viewModel;
        private VisualElement _root;
        private Button _button;
        private VisualElement _statusIndicator;

        public SettingsBinder(SettingsViewModel viewModel) => _viewModel = viewModel;

        public void Bind(VisualElement root)
        {
            _root = root;
            _root.dataSource = _viewModel;

            _button = root.Q<Button>("notifications-button");
            _statusIndicator = root.Q<VisualElement>("notifications-status");

            _button.clicked += OnClicked;
            _viewModel.propertyChanged += OnViewModelPropertyChanged;

            RefreshVisualState();
        }

        private void OnClicked() => _ = _viewModel.ToggleNotificationsAsync();

        private void OnViewModelPropertyChanged(object sender, BindablePropertyChangedEventArgs args) =>
            RefreshVisualState();

        private void RefreshVisualState()
        {
            _button.SetEnabled(!_viewModel.IsUpdating);
            _statusIndicator.EnableInClassList("status-indicator--enabled", _viewModel.NotificationsEnabled);
            _statusIndicator.EnableInClassList("status-indicator--disabled", !_viewModel.NotificationsEnabled);
        }

        public void Dispose()
        {
            if (_button != null)
            {
                _button.clicked -= OnClicked;
            }

            _viewModel.propertyChanged -= OnViewModelPropertyChanged;

            if (_root != null)
            {
                _root.dataSource = null;
            }

            _viewModel.Dispose();
        }
    }
}

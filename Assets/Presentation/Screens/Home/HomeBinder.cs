using Presentation.Common;
using UnityEngine.UIElements;

namespace Presentation.Screens.Home
{
    public sealed class HomeBinder : IScreenBinder
    {
        private readonly HomeViewModel _viewModel;
        private VisualElement _root;
        private Button _incrementButton;
        private Button _profileButton;

        public HomeBinder(HomeViewModel viewModel) => _viewModel = viewModel;

        public void Bind(VisualElement root)
        {
            _root = root;
            _root.dataSource = _viewModel;

            _incrementButton = root.Q<Button>("increment-button");
            _profileButton = root.Q<Button>("profile-button");

            _incrementButton.clicked += _viewModel.Increment;
            _profileButton.clicked += _viewModel.OpenProfile;
        }

        public void Dispose()
        {
            if (_incrementButton != null)
            {
                _incrementButton.clicked -= _viewModel.Increment;
            }

            if (_profileButton != null)
            {
                _profileButton.clicked -= _viewModel.OpenProfile;
            }

            if (_root != null)
            {
                _root.dataSource = null;
            }
        }
    }
}

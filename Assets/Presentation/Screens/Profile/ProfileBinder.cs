using Presentation.Common;
using UnityEngine.UIElements;

namespace Presentation.Screens.Profile
{
    public sealed class ProfileBinder : IScreenBinder
    {
        private readonly ProfileViewModel _viewModel;
        private VisualElement _root;

        public ProfileBinder(ProfileViewModel viewModel) => _viewModel = viewModel;

        public void Bind(VisualElement root)
        {
            _root = root;
            _root.dataSource = _viewModel;
            _ = _viewModel.LoadAsync();
        }

        public void Dispose()
        {
            if (_root != null)
            {
                _root.dataSource = null;
            }

            _viewModel.Dispose();
        }
    }
}

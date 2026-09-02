using Application.Navigation;
using Presentation.Common;
using Presentation.Navigation;
using Unity.Properties;

namespace Presentation.Screens.Home
{
    public sealed class HomeViewModel : BindableViewModel
    {
        private readonly INavigationService _navigation;
        private int _counter;

        public HomeViewModel(INavigationService navigation) => _navigation = navigation;

        [CreateProperty]
        public int Counter => _counter;

        public void Increment()
        {
            _counter++;
            NotifyPropertyChanged(nameof(Counter));
        }

        public void OpenProfile() => _navigation.Navigate(AppRoute.Profile);
    }
}

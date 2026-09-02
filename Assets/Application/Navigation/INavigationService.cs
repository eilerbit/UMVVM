using Presentation.Navigation;
using R3;

namespace Application.Navigation
{
    public interface INavigationService
    {
        ReadOnlyReactiveProperty<AppRoute> CurrentRoute { get; }
        void Navigate(AppRoute route);
    }
}

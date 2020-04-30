using Windows.UI.Xaml.Controls;
using SbNotifierUwp.ViewModels;

namespace SbNotifierUwp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += async (currentWin, arguments) =>
            {
                var mainPageViewModel = new MainPageViewModel();
                await mainPageViewModel.LoadAsync();
                DataContext = mainPageViewModel;
            };
        }
    }
}

using System.Windows;
using SbNotifierDevice.ViewModels;

namespace SbNotifierDevice
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += async (window, parameters) =>
            {
                var mainPageViewModel = new MainPageViewModel();
                await mainPageViewModel.RegisterDeviceAsync();
                DataContext = mainPageViewModel;
            };
        }
    }
}
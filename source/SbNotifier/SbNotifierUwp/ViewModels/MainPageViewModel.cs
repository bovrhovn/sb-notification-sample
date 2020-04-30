using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using SbNotifierUwp.Annotations;

namespace SbNotifierUwp.ViewModels
{
    public sealed class MainPageViewModel : INotifyPropertyChanged
    {
        public Task LoadAsync()
        {
            RegistrationMessage =
                $"App is registered to use Notification Hub {((App) Application.Current).RegistrationID}";
            return Task.CompletedTask;
        }
        
        private string registrationMessage;

        public string RegistrationMessage
        {
            get => registrationMessage;
            set
            {
                if (value == registrationMessage) return;
                registrationMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
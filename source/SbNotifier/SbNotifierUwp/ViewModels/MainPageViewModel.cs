using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Background;
using Windows.UI.Xaml;
using SbNotifierUwp.Annotations;
using SbNotifierUwp.Helpers;

namespace SbNotifierUwp.ViewModels
{
    public sealed class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel() => RegisterBackGroundTasksCommand = new RelayCommand(_ => RegisterBackgroundTask());

        private void RegisterBackgroundTask()
        {
            var taskRegistered = false;
            var pushNotificationBackgroundTask = "PushNotificationBackgroundTask";

            foreach (var currentTask in BackgroundTaskRegistration.AllTasks)
            {
                if (currentTask.Value.Name == pushNotificationBackgroundTask)
                    taskRegistered = true;
            }
            
            if (!taskRegistered) {

                var builder = new BackgroundTaskBuilder
                {
                    Name = pushNotificationBackgroundTask,
                    TaskEntryPoint = "SbNotifierUwp.Tasks.PushNotificationBackgroundTask"
                };

                builder.SetTrigger(new PushNotificationTrigger());
                var task = builder.Register();
            }
        }

        public Task LoadAsync()
        {
            var currentApp = (App) Application.Current;

            RegistrationMessage =
                $"App is registered to use Notification Hub {currentApp.RegistrationID}";
            var pushNotificationChannel = currentApp.NotificationChannel;
            if (pushNotificationChannel != null)
            {
                pushNotificationChannel.PushNotificationReceived += (sender, args) =>
                {
                    if (args.RawNotification != null)
                        RegistrationMessage = $"Received RAW notification {args.RawNotification.Content}";
                };
            }

            return Task.CompletedTask;
        }

        public ICommand RegisterBackGroundTasksCommand { get; private set; }

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
        private void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
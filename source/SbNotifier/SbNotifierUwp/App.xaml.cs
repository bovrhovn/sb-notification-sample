using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.Messaging;
using SbNotifierUwp.Helpers;

namespace SbNotifierUwp
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                }

                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }

                Window.Current.Activate();
            }

            InitNotificationsAsync();
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private async void InitNotificationsAsync()
        {
            try
            {
                NotificationChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            
                var hub = new NotificationHub(Secrets.HubName, Secrets.HubConnectionString);
                var result = await hub.RegisterNativeAsync(NotificationChannel.Uri);

                // Displays the registration ID so you know it was successful
                if (result.RegistrationId != null) RegistrationID = result.RegistrationId;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public PushNotificationChannel NotificationChannel { get; set; }
        
        public string RegistrationID { get; set; } = string.Empty;
    }
}
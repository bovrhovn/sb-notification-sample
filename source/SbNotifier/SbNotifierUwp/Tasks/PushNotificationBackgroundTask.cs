using Windows.ApplicationModel.Background;
using Windows.Networking.PushNotifications;

namespace SbNotifierUwp.Tasks
{
    public class PushNotificationBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
    
            var notification = (RawNotification)taskInstance.TriggerDetails;
            string content = notification.Content;

            //do something with it - save to storage or execute external program
            
            
            deferral.Complete();
        }
    }
}
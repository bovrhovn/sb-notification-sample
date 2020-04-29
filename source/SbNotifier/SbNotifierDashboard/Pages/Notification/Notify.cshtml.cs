using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using SbNotifierDashboard.Helpers;
using SbNotifierDashboard.Options;

namespace SbNotifierDashboard.Pages.Notification
{
    public class NotifyPageModel : PageModel
    {
        public NotifyPageModel(IOptions<NotificationHubOptions> nhOptions) =>
            notificationHub = NotificationHubClient
                .CreateClientFromConnectionString(nhOptions.Value.ConnectionString, nhOptions.Value.HubName);

        [BindProperty] public string Text { get; set; }

        [BindProperty, TempData] public string Message { get; set; } = "";

        [BindProperty] public string MessageType { get; set; }

        public readonly string[] MessageTypes = {"Raw", "Toast"};

        private readonly NotificationHubClient notificationHub;

        public async Task<IActionResult> OnPostAsync()
        {
            var text = Text;
            Microsoft.Azure.NotificationHubs.Notification notification;
            if (MessageType == "Raw")
            {
                // var settings = new JObject {{"text1", "my app"}, {"link", "https://download.link.to.a.file"}};
                //notification = new MpnsNotification(settings.ToString());
                notification = new WindowsNotification(text);
                notification.Headers.Add("X-WNS-Type", "wns/raw");
            }
            else
            {
                text = string.Format(Constants.ToastTemplate, text);
                notification = new WindowsNotification(text);
            } 

            await notificationHub.SendNotificationAsync(notification);

            Message = $"Message {Text} with {MessageType} sent to notification hub and all subscriber!";
            return Page();
        }
    }
}
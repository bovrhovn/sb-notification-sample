using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Options;
using SbNotifierDashboard.Hub;
using SbNotifierDashboard.Options;

namespace SbNotifierDashboard.Pages.Notification
{
    public class HistoryPageModel : PageModel
    {
        private readonly IHubContext<NotificationHub> hubContext;
        private readonly EventHubClient eventHubClient;

        public HistoryPageModel(IOptions<IotOptions> optionsValue, IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
            var connectionString = new EventHubsConnectionStringBuilder(
                new Uri(optionsValue.Value.EventHubsCompatibleEndpoint),
                optionsValue.Value.EventHubName, "service", optionsValue.Value.SasKey);
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());
        }

        public async Task OnGetAsync()
        {
            var runtimeInfo = await eventHubClient.GetRuntimeInformationAsync();
            var d2cPartitions = runtimeInfo.PartitionIds;

            var cts = new CancellationTokenSource();

            var tasks = new List<Task>();
            foreach (string partition in d2cPartitions)
            {
                tasks.Add(ReceiveMessagesFromDeviceAsync(partition, cts.Token));
            }

            await hubContext.Clients.All.SendAsync("broadcastMessage",
                $"Starting to listen on {d2cPartitions.Length} partitions", cts.Token);
            // Wait for all the PartitionReceivers to finsih.
            Task.WaitAll(tasks.ToArray());
        }

        private async Task ReceiveMessagesFromDeviceAsync(string partition, CancellationToken ct)
        {
            // Create the receiver using the default consumer group.
            // For the purposes of this sample, read only messages sent since 
            // the time the receiver is created. Typically, you don't want to skip any messages.
            var eventHubReceiver =
                eventHubClient.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
            while (true)
            {
                if (ct.IsCancellationRequested) break;
                string message = $"Listening for messages on: {partition}";
                // Check for EventData - this methods times out if there is nothing to retrieve.
                var events = await eventHubReceiver.ReceiveAsync(100);

                // If there is data in the batch, process it.
                if (events == null) continue;

                foreach (EventData eventData in events)
                {
                    string data = Encoding.UTF8.GetString(eventData.Body.Array);
                    message += $"Message received on partition {partition} {data}<br/>";
                    message += "Application properties (set by device):<br/>";
                    foreach (var prop in eventData.Properties)
                    {
                        message += $"{prop.Key} {prop.Value}<br/>";
                    }

                    message += "System properties (set by IoT Hub):<br/>";
                    foreach (var prop in eventData.SystemProperties)
                    {
                        message += $"{prop.Key} {prop.Value}<br/>";
                    }
                }

                await hubContext.Clients.All.SendAsync("broadcastMessage", message, cancellationToken: ct);
            }
        }
    }
}
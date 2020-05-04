using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SbNotifierDashboard.Models;
using SbNotifierDashboard.Options;

namespace SbNotifierDashboard.Pages.Notification
{
    public class IotPageModel : PageModel
    {
        private readonly RegistryManager registryManager;
        private readonly ServiceClient serviceClient;

        public IotPageModel(IOptions<IotOptions> optionsValue)
        {
            var valueConnectionString = optionsValue.Value.ConnectionString;
            registryManager = RegistryManager.CreateFromConnectionString(valueConnectionString);
            serviceClient = ServiceClient.CreateFromConnectionString(valueConnectionString);
        }

        public List<DeviceInfo> Devices { get; private set; } = new List<DeviceInfo>();

        [BindProperty] public string DeviceId { get; set; }
        [BindProperty] public string Message { get; set; }
        [BindProperty, TempData] public string InfoText { get; set; }

        public async Task OnGetAsync()
        {
            var query = registryManager.CreateQuery("SELECT * FROM devices", 100);
            var list = new List<DeviceInfo>();
            while (query.HasMoreResults)
            {
                var page = await query.GetNextAsTwinAsync();
                foreach (var twin in page)
                {
                    list.Add(new DeviceInfo
                    {
                        Id = twin.DeviceId,
                        Name = string.Empty
                    });
                }
            }

            Devices = list;

            InfoText = $"{list.Count} devices loaded!";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var methodInvocation = new CloudToDeviceMethod("UpdateRequested")
                {ResponseTimeout = TimeSpan.FromSeconds(30)};
            string message = Message + "http://https://sbwebdashboard.azurewebsites.net/download";
            var json = JsonConvert.SerializeObject(message);
            methodInvocation.SetPayloadJson(json);

            try
            {
                var response = await serviceClient.InvokeDeviceMethodAsync(DeviceId, methodInvocation);
                InfoText = $"Response status: {response.Status}, payload:{response.GetPayloadAsJson()}";
            }
            catch (Exception e)
            {
                InfoText = $"Message was not delivered!{Environment.NewLine}{e.Message}";
            }
            return Page();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Options;
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
        [BindProperty] public string InfoText { get; set; }

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
            methodInvocation.SetPayloadJson($"{Message} - http://https://sbwebdashboard.azurewebsites.net/download");

            var response = await serviceClient.InvokeDeviceMethodAsync(DeviceId, methodInvocation);

            InfoText = $"Response status: {response.Status}, payload:{response.GetPayloadAsJson()}";
            return Page();
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Extensions.Options;
using SbNotifierDashboard.Options;

namespace SbNotifierDashboard.Pages.Devices
{
    public class GeneratePageModel : PageModel
    {
        [BindProperty, TempData] public string InfoText { get; set; }
        [BindProperty] public string Tags { get; set; }
        [BindProperty] public int Number { get; set; }
        private readonly RegistryManager registryManager;
        private readonly ServiceClient serviceClient;

        public GeneratePageModel(IOptions<IotOptions> optionsValue)
        {
            var valueConnectionString = optionsValue.Value.ConnectionString;
            registryManager = RegistryManager.CreateFromConnectionString(valueConnectionString);
            serviceClient = ServiceClient.CreateFromConnectionString(valueConnectionString);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Tags))
            {
                InfoText = "Tags should not be empty - enter at least one item";
                return RedirectToPage("Generate");
            }

            var tags = Tags.Split(";");
            int counter = 0;
            for (int currentItem = 0; currentItem < Number; currentItem++)
            {
                var deviceId = Guid.NewGuid().ToString();
                var device = new Device(deviceId);

                try
                {
                    await registryManager.AddDeviceAsync(device);
                    counter++;
                }
                catch (DeviceAlreadyExistsException ex)
                {
                    InfoText = $"Device is already added. Check ID.{Environment.NewLine}{ex.Message}";
                }

                //add tags value - for having an ability to define tags
                var twin = await registryManager.GetTwinAsync(deviceId);

                //if he only enters one item
                var value = tags.Length == 0 ? Tags : tags[0];

                //set random tag for device
                if (tags.Length >= 1)
                    value = tags[new Random().Next(0, tags.Length - 1)];

                await registryManager.UpdateTwinAsync(deviceId,
                    "{\"tags\": {\"location\":\"" + value + "\"}}", twin.ETag);
            }

            InfoText = $"Added {counter} devices with default tags...";

            //generate devices
            return RedirectToPage("Index");
        }
    }
}
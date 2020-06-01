using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Options;
using SbNotifierDashboard.Helpers;
using SbNotifierDashboard.Models;
using SbNotifierDashboard.Options;

namespace SbNotifierDashboard.Pages.Devices
{
    public class IndexPageModel : PageModel
    {
        [BindProperty, TempData] public string InfoText { get; set; }
        public PaginatedList<DeviceInfo> Devices { get; set; }
        [BindProperty] public string Query { get; set; }

        private readonly RegistryManager registryManager;
        //private string connectionStringTemplate = "HostName={0}.azure-devices.net;DeviceId={1};SharedAccessKey={2}";
        private readonly IotOptions optionsValue;

        public IndexPageModel(IOptions<IotOptions> optionsValue)
        {
            this.optionsValue = optionsValue.Value;
            var valueConnectionString = this.optionsValue.ConnectionString;
            registryManager = RegistryManager.CreateFromConnectionString(valueConnectionString);
        }

        public async Task OnGetAsync(int? pageIndex)
        {
            var deviceQuery = "SELECT * FROM devices";
            if (!string.IsNullOrEmpty(Query)) deviceQuery += $" WHERE tags.location='{deviceQuery}'";
            var query = registryManager.CreateQuery(deviceQuery, optionsValue.PageSize);
            var list = new List<DeviceInfo>();
            while (query.HasMoreResults)
            {
                var page = await query.GetNextAsTwinAsync();
                foreach (var twin in page)
                {
                    list.Add(new DeviceInfo
                    {
                        Id = twin.DeviceId,
                        Name = string.IsNullOrEmpty(twin.ModuleId) ? twin.ModuleId : "Not defined"
                    });
                }
            }

            Devices = PaginatedList<DeviceInfo>.Create(list.AsQueryable(), pageIndex ?? 1, optionsValue.PageSize);

            InfoText = $"{list.Count} devices loaded!";
        }
    }
}
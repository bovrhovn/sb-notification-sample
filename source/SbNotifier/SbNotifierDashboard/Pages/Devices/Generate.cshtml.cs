using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
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
        
        public void OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Tags))
            {
                var tags = Tags.Split(";");
                if (tags.Length == 0)
                {
                    //
                }
                else
                {
                    //add randomly 
                }
            }
        }
        
        
    }
}
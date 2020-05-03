using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SbNotifierDashboard.Hub;
using SbNotifierDashboard.Options;

namespace SbNotifierDashboard
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<NotificationHubOptions>(Configuration.GetSection("NotificationHub"));
            services.Configure<IotOptions>(Configuration.GetSection("IotHub"));
            services.AddSignalR().AddAzureSignalR();
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Home/Index", "");
                options.Conventions.AddPageRoute("/Home/Download", "download");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<NotificationHub>("/notification");
            });
        }
    }
}
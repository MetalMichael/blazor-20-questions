using Blazor20Questions.Client.Services;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Blazor20Questions.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazorModal();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.UseLocalTimeZone();
            app.AddComponent<App>("app");
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Blazor20Questions.Client.Services
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBlazorModal(this IServiceCollection services)
        {
            return services.AddScoped<ModalService>();
        }
    }
}

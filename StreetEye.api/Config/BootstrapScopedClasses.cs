using StreetEye.Repository.SseEvent;
using StreetEye.Services.SseEvent;

namespace StreetEye.Config
{
    public static class BootstrapScopedClasses
    {
        public static void Init(IServiceCollection services)
        {
            HandleHttpClientClasses(services);
            HandleScopedClasses(services);
        }

        private static void HandleScopedClasses(IServiceCollection services)
        {
            services.AddScoped<ISseEventRepository, SseEventRepository>();
            services.AddScoped<ISseEventService, SseEventService>();
        }

        private static void HandleHttpClientClasses(IServiceCollection services)
        {
            services.AddHttpClient<SseEventRepository>();
        }
    }
}
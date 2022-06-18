using BillingAPI.Helpers;

namespace BillingAPI.Extensions.ServiceExtensions
{
    public static class HelpersExtension
    {
        public static IServiceCollection AddHelpers(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            return services;
        }
    }
}

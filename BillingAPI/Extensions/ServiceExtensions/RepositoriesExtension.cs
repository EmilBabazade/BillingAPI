namespace BillingAPI.Extensions.ServiceExtensions
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration config)
        {
            //services.AddScoped<IRepo, Repo>();
            return services;
        }
    }
}

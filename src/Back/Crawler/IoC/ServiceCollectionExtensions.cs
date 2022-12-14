using Crawler.Abtsracts;
using Crawler.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Crawler.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClubeFiiService(this IServiceCollection services)
        {
            services.AddHttpClient<IFiiService, ClubeFiiService>(client =>
            {
                client.BaseAddress = new Uri("https://www.clubefii.com.br");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.2");
            });

            services.AddMemoryCache();

            return services;
        }

        public static IServiceCollection AddStatusInvestService(this IServiceCollection services)
        {
            services.AddHttpClient<IFiiService, StatusInvestService>(client =>
            {
                client.BaseAddress = new Uri("https://statusinvest.com.br");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.29.2");
            });

            services.AddMemoryCache();

            return services;
        }
    }
}

using bookify.domain.Bookings;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application
{

    public static class DependecyInjection
    {
        public static IServiceCollection serviceCollection(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependecyInjection).Assembly))
                    .AddTransient<PricingService>();
            return services;
        }
    }
}

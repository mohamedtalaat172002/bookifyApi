using bookify.domain.Bookings;
using Bookify.Application.Abstraction.Behavior;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Application
{

    public static class DependecyInjection
    {
        public static IServiceCollection AddApplicationDependecies(this IServiceCollection services)
        {
            services.AddMediatR(
                cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(DependecyInjection).Assembly);
                    cfg.AddOpenBehavior(typeof(LoggingBeahvior<,>));
                    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                });
            services.AddTransient<PricingService>();
            services.AddValidatorsFromAssembly(typeof(DependecyInjection).Assembly);
            return services;
        }
    }
}

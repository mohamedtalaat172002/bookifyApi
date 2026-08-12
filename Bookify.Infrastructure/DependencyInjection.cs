using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.EmailService;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();


            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
            (configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration))));

            return services;
        }
    }
}

using bookify.domain.Abstractions;
using bookify.domain.Apartments;
using bookify.domain.Bookings;
using bookify.domain.Users;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.EmailService;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Repositories;
using Dapper;
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

            services.AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IApartmentRepository, ApartmentRepository>()
                .AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IUniteOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            //services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
            return services;
        }
    }
}

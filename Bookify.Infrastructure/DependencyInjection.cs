using bookify.domain.Abstractions;
using bookify.domain.Apartments;
using bookify.domain.Bookings;
using bookify.domain.Users;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.EmailService;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookify.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.")));

            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();
            addPersistence(services, configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
            services.ConfigureOptions<JwtBearerOptionsSetup>();
            return services;
        }

        private static void addPersistence(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISqlConnectionFactory>(sp =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException("Connection string not found.");
                return new SqlConnectionFactory(connectionString);
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            services.AddScoped<IUniteOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }
    }
}

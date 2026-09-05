using Asp.Versioning;
using bookify.domain.Abstractions;
using bookify.domain.Apartments;
using bookify.domain.Bookings;
using bookify.domain.Users;
using Bookify.Application.Abstraction.Authentication;
using Bookify.Application.Abstraction.Caching;
using Bookify.Application.Abstraction.Clock;
using Bookify.Application.Abstraction.Data;
using Bookify.Application.Abstraction.EmailService;
using Bookify.Infrastructure.Authentication;
using Bookify.Infrastructure.Authorization;
using Bookify.Infrastructure.Caching;
using Bookify.Infrastructure.Clock;
using Bookify.Infrastructure.Data;
using Bookify.Infrastructure.Email;
using Bookify.Infrastructure.Outbox;
using Bookify.Infrastructure.Repositories;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using AuthenticationOptions = Bookify.Infrastructure.Authentication.AuthenticationOptions;
using AuthenticationService = Bookify.Infrastructure.Authentication.AuthenticationService;
using IAuthenticationService = Bookify.Application.Abstraction.Authentication.IAuthenticationService;

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

            AddCashingDependecis(services, configuration);

            AddVersioning(services);

            addAuthorization(services);

            AddAuthenticationServices(services, configuration);

            addOutboxServices(services, configuration);
            return services;


        }

        private static void addOutboxServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OutboxOptions>(configuration.GetSection("outbox"));
            services.AddQuartz();
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });
        }

        private static void AddVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1);
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddMvc()
            .AddApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'V";
                opt.SubstituteApiVersionInUrl = true;
            }
            );
        }
        private static void AddCashingDependecis(IServiceCollection services, IConfiguration configuration)
        {
            var ConnectionString = configuration.GetConnectionString("Cache") ?? throw new ArgumentNullException(nameof(configuration));
            services.AddStackExchangeRedisCache(opt => opt.Configuration = ConnectionString);
            services.AddSingleton<ICachService, CacheService>();
        }

        private static void addAuthorization(IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();
            services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();
        }

        private static void AddAuthenticationServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
            services.ConfigureOptions<JwtBearerOptionsSetup>();

            services.Configure<KeycloakOptions>(configuration.GetSection("Keyclock"));
            services.AddTransient<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IAuthenticationService, AuthenticationService>((ServiceProvider, HttpClient) =>
            {

                var KeyclockOptions = ServiceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                HttpClient.BaseAddress = new Uri(KeyclockOptions.AdminUrl);

            }).AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();

            services.AddHttpClient<IJwTService, JwTService>((ServiceProvider, HttpClient) =>
            {

                var KeyclockOptions = ServiceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
                HttpClient.BaseAddress = new Uri(KeyclockOptions.TokenUrl);
            });
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

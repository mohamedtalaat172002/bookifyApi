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
        //public static IServiceCollection AddInfrastructureDependecies(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        //    services.AddTransient<IEmailService, EmailService>();



        //    services.AddSingleton<ISqlConnectionFactory>(serviceProvider =>
        //    {
        //        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        //        var connectionString = configuration.GetConnectionString("DefaultConnection");
        //        // ?? throw new ArgumentNullException(nameof(connectionString));

        //        return new SqlConnectionFactory(connectionString);
        //    });
        //    //services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
        //    //(configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(configuration))));


        //    services.AddScoped<IUserRepository, UserRepository>()
        //    .AddScoped<IApartmentRepository, ApartmentRepository>()
        //    .AddScoped<IBookingRepository, BookingRepository>();

        //    services.AddScoped<IUniteOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        //    //services.AddScoped<IUnitOfWork, ApplicationDbContext>();
        //    services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        //    SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        //    return services;
        //}

        public static IServiceCollection AddInfrastructureDependecies(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. تسجيل الـ DbContext (ده كان معلق ومحتاج يتفك)
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.")));

            // 2. الخدمات الأساسية
            services.AddTransient<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();

            // 3. تسجيل الـ SqlConnectionFactory بشكل واحد فقط (أنت كنت مسجله مرتين وده غلط)
            services.AddSingleton<ISqlConnectionFactory>(sp =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException("Connection string not found.");
                return new SqlConnectionFactory(connectionString);
            });

            // 4. تسجيل الـ Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApartmentRepository, ApartmentRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();

            // 5. تسجيل الـ UnitOfWork (الـ Context هو اللي بينفذ الـ UnitOfWork)
            services.AddScoped<IUniteOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());

            return services;
        }
    }
}

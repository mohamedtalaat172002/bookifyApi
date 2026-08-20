using Bogus;
using bookify.domain.Apartments;
using Bookify.Application.Abstraction.Data;
using Dapper;
using System.Data;
using System.Text.Json;

namespace bookify.Api.Extensions
{
    internal static class SeedDataExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            ISqlConnectionFactory sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using IDbConnection connection = sqlConnectionFactory.CreateConnection();
            var faker = new Faker();
            List<object> apartments = new();
            for (int i = 0; i < 100; i++)
            {
                var amenitiesList = new List<int> { (int)Amenity.Parking, (int)Amenity.MountainView };
                apartments.Add(new
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Company.CompanyName(),
                    Description = "Amazing view",
                    Country = faker.Address.Country(),
                    ZipCode = faker.Address.ZipCode(),
                    City = faker.Address.City(),
                    Street = faker.Address.StreetAddress(),
                    PriceAmount = faker.Random.Decimal(50, 1000),
                    PriceCurrency = "USD",
                    CleaningFeeAmount = faker.Random.Decimal(25, 200),
                    CleaningFeeCurrency = "USD",
                    Amenities = JsonSerializer.Serialize(amenitiesList),
                    LastBookedOn = new DateTime(2000, 1, 1),
                    Version = 0L
                });
            }

            const string sql = """
                INSERT INTO Apartments
                (id, Name, Description, Address_Country, Address_ZipCode, Address_City, Address_Street, Price_Amount, Price_Currency, CleaningFee_Amount, CleaningFee_Currency, Amenities, LastBookedOnUtc, Version)
                VALUES(@Id, @Name, @Description, @Country, @ZipCode, @City, @Street, @PriceAmount, @PriceCurrency, @CleaningFeeAmount, @CleaningFeeCurrency, @Amenities, @LastBookedOn, @Version);
                """;

            connection.Execute(sql, apartments);
        }
    }
}
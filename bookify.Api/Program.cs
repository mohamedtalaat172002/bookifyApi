using bookify.Api.Extensions;
using bookify.Api.openApi;
using Bookify.Application;
using Bookify.Infrastructure;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureDependecies(builder.Configuration);
builder.Services.AddApplicationDependecies();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    //app.SeedData();
    app.UseSwaggerUI(options =>
    {
        // var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in app.DescribeApiVersions())
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });

}

app.UseHttpsRedirection();
app.UseRequestContextLogging();

app.UseSerilogRequestLogging();
app.UseCustomeExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

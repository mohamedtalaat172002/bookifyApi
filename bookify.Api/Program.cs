using bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureDependecies(builder.Configuration);
builder.Services.AddApplicationDependecies();

builder.Host.UseSerilog((context, configuration) =>
configuration.ReadFrom.Configuration(context.Configuration));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    //app.SeedData();
}

app.UseHttpsRedirection();
app.UseRequestContextLogging();

app.UseSerilogRequestLogging();
app.UseCustomeExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

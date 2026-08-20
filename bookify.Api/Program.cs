using bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureDependecies(builder.Configuration);
builder.Services.AddApplicationDependecies();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    //app.SeedData();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCustomeExceptionHandler();
app.MapControllers();

app.Run();

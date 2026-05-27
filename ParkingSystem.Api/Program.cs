using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Api.Middlewares;
using ParkingSystem.Application.Interfaces.Repositories;
using ParkingSystem.Application.Interfaces.Services;
using ParkingSystem.Application.Services;
using ParkingSystem.Application.Validators;
using ParkingSystem.Infrastructure.Configurations;
using ParkingSystem.Infrastructure.ExternalServices.Services;
using ParkingSystem.Infrastructure.Persistence;
using ParkingSystem.Infrastructure.Repositories;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt",rollingInterval: RollingInterval.Day,retainedFileCountLimit: 30)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddScoped< IParkingService, ParkingService>();
builder.Services.AddScoped< IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IParkingMovementRepository,ParkingMovementRepository>();
builder.Services.AddScoped<IRateConfigurationRepository,RateConfigurationRepository>();

builder.Services.Configure<EmailApiSettings>( builder.Configuration.GetSection("EmailApiSettings"));

builder.Services.AddHttpClient<IEmailService,EmailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAngular",
        policy =>
        {
            policy
                .WithOrigins(
                    "https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining< RegisterEntryRequestDtoValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

Log.Information("Starting ParkingSystem API");

app.Lifetime.ApplicationStopping.Register(() =>
{
    Log.Information("Stopping ParkingSystem API");
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();
app.Run();
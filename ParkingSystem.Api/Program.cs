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

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

builder.Services.AddScoped<IParkingService, ParkingService>();

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IParkingMovementRepository, ParkingMovementRepository>();
builder.Services.AddScoped<IRateConfigurationRepository, RateConfigurationRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterEntryRequestDtoValidator>();

builder.Services.Configure<EmailApiSettings>(builder.Configuration.GetSection("EmailApiSettings"));
builder.Services.AddHttpClient<IEmailService, EmailService>();


var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
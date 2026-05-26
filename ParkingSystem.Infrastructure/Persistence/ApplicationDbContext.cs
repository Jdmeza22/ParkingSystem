
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Domain.Entities;

namespace ParkingSystem.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    public DbSet<VehicleType> VehicleTypes => Set<VehicleType>();

    public DbSet<ParkingMovement> ParkingMovements => Set<ParkingMovement>();

    public DbSet<RateConfiguration> RateConfigurations => Set<RateConfiguration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }
}

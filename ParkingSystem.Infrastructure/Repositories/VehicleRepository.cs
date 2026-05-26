using ParkingSystem.Application.Interfaces.Repositories;
using ParkingSystem.Domain.Entities;
using ParkingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ParkingSystem.Infrastructure.Repositories;

public class VehicleRepository(ApplicationDbContext _context) : IVehicleRepository
{

    public async Task<Vehicle?> GetByPlateAsync(string plate)
    {
        return await _context.Vehicles.FirstOrDefaultAsync(x => x.Plate == plate);
    }

    public async Task AddVehicleAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

using ParkingSystem.Application.Interfaces.Repositories;
using ParkingSystem.Domain.Entities;
using ParkingSystem.Domain.Enums;
using ParkingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ParkingSystem.Infrastructure.Repositories;

public class ParkingMovementRepository(ApplicationDbContext _context) : IParkingMovementRepository
{

    public async Task AddParkingMovementAsync(ParkingMovement movement)
    {
        await _context.ParkingMovements.AddAsync(movement);
    }

    public async Task<ParkingMovement?> GetActiveMovementByPlateAsync(string plate)
    {
        return await _context.ParkingMovements
            .Include(x => x.Vehicle)
            .ThenInclude(x => x.VehicleType)
            .FirstOrDefaultAsync(x =>
                x.Vehicle.Plate == plate &&
                x.Status == ParkingStatus.Active
            );
    }

    public async Task<List<ParkingMovement>> GetActiveVehiclesAsync()
    {
        return await _context.ParkingMovements
            .Include(x => x.Vehicle)
            .ThenInclude(x => x.VehicleType)
            .Where(x => x.Status == ParkingStatus.Active)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

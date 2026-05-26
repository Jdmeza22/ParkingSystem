using ParkingSystem.Domain.Entities;

namespace ParkingSystem.Application.Interfaces.Repositories;

public interface IParkingMovementRepository
{
    Task AddParkingMovementAsync(ParkingMovement movement);
    Task<ParkingMovement?> GetActiveMovementByPlateAsync(string plate);
    Task<List<ParkingMovement>> GetActiveVehiclesAsync();
    Task SaveChangesAsync();
}

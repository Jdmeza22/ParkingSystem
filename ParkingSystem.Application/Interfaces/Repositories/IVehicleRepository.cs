using ParkingSystem.Domain.Entities;

namespace ParkingSystem.Application.Interfaces.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetByPlateAsync(string plate);
    Task AddVehicleAsync(Vehicle vehicle);
    Task SaveChangesAsync();
}
using ParkingSystem.Domain.Entities;

namespace ParkingSystem.Application.Interfaces.Repositories;

public interface IRateConfigurationRepository
{
    Task<RateConfiguration?> GetActiveRateAsync();
}

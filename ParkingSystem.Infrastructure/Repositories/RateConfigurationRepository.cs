using ParkingSystem.Application.Interfaces.Repositories;
using ParkingSystem.Domain.Entities;
using ParkingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ParkingSystem.Infrastructure.Repositories;


public class RateConfigurationRepository(ApplicationDbContext _context) : IRateConfigurationRepository
{
    public async Task<RateConfiguration?> GetActiveRateAsync()
    {
        return await _context.RateConfigurations
            .FirstOrDefaultAsync(x => x.Active);
    }
}

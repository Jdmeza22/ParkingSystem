using ParkingSystem.Application.DTOs;

namespace ParkingSystem.Application.Interfaces.Services;

public interface IParkingService
{
    Task RegisterEntryAsync(RegisterEntryRequestDto request);
    Task<ExitResponseDto> RegisterExitAsync(RegisterExitRequestDto request);
}

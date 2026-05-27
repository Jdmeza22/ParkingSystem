using Microsoft.AspNetCore.Mvc;
using ParkingSystem.Application.DTOs;
using ParkingSystem.Application.Interfaces.Repositories;
using ParkingSystem.Application.Interfaces.Services;
using ParkingSystem.Domain.Entities;

namespace ParkingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParkingController(IParkingService _parkingService, IParkingMovementRepository _parkingMovementRepository) : ControllerBase
{
 
    [HttpPost("entry")]
    public async Task<IActionResult> RegisterEntry(RegisterEntryRequestDto request)
    {
        await _parkingService.RegisterEntryAsync(request);
        return Ok(new{ message = "Vehicle entry registered successfully." });
    }

    [HttpPost("exit")]
    public async Task<IActionResult> RegisterExit( RegisterExitRequestDto request)
    {
        ExitResponseDto result = await _parkingService.RegisterExitAsync(request);
        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveVehicles()
    {
        List<ParkingMovement> activeVehicles = await _parkingMovementRepository.GetActiveVehiclesAsync();

        var response =
            activeVehicles.Select(x => new
            {
                Plate = x.Vehicle.Plate,
                VehicleType = x.Vehicle.VehicleType.Name,
                EntryDate = x.EntryDate
            });

        return Ok(response);
    }
}

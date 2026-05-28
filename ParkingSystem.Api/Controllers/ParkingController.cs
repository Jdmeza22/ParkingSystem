using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ParkingSystem.Application.Common;
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
        return Ok();
    }

    [HttpPost("exit")]
    public async Task<IActionResult> RegisterExit( RegisterExitRequestDto request)
    {
        ExitResponseDto result = await _parkingService.RegisterExitAsync(request);
        return Ok(ApiResponse<ExitResponseDto>.SuccessResponse(result));
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveVehicles()
    {
        List<ParkingMovement> activeVehicles = await _parkingMovementRepository.GetActiveVehiclesAsync();
        List<ActiveVehicleResponseDto> response = activeVehicles.Select(x => new ActiveVehicleResponseDto
        {
            Plate = x.Vehicle.Plate,
            VehicleType = x.Vehicle.VehicleType.Name,
            EntryDate = x.EntryDate
        }).ToList();

        return Ok(ApiResponse<List<ActiveVehicleResponseDto>>.SuccessResponse(response));
    }
}

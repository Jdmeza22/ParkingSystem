using ParkingSystem.Application.DTOs;
using ParkingSystem.Application.Exceptions;
using ParkingSystem.Application.Interfaces.Repositories;
using ParkingSystem.Application.Interfaces.Services;
using ParkingSystem.Domain.Entities;
using ParkingSystem.Domain.Enums;
namespace ParkingSystem.Application.Services;

public class ParkingService( 
    IVehicleRepository _vehicleRepository, 
    IParkingMovementRepository _parkingMovementRepository,
    IRateConfigurationRepository _rateConfigurationRepository) : IParkingService
{
    public async Task RegisterEntryAsync(RegisterEntryRequestDto request)
    {
        ParkingMovement activeMovement =  await _parkingMovementRepository.GetActiveMovementByPlateAsync(request.Plate);

        if (activeMovement is not null) {  throw new BadRequestException(   "Vehicle already has an active parking movement." ); }

        var vehicle = await _vehicleRepository.GetByPlateAsync(request.Plate);

        if (vehicle is null)
        {
            vehicle = new Vehicle
            {
                Plate = request.Plate.ToUpper(),
                VehicleTypeId = request.VehicleTypeId
            };

            await _vehicleRepository.AddVehicleAsync(vehicle);
            await _vehicleRepository.SaveChangesAsync();
        }

        ParkingMovement movement = new ParkingMovement
        {
            VehicleId = vehicle.Id,
            EntryDate = DateTime.UtcNow,
            Status = ParkingStatus.Active,
            EmailSent = false
        };

        await _parkingMovementRepository.AddParkingMovementAsync(movement);
        await _parkingMovementRepository.SaveChangesAsync();
    }

    public async Task<ExitResponseDto> RegisterExitAsync( RegisterExitRequestDto request)
    {
        ParkingMovement movement =  await _parkingMovementRepository.GetActiveMovementByPlateAsync(request.Plate);

        if (movement is null){ throw new NotFoundException("Vehicle does not have an active parking movement." );  }

        DateTime exitDate = DateTime.UtcNow;
        int totalMinutes =(int)Math.Ceiling( (exitDate - movement.EntryDate).TotalMinutes );
        RateConfiguration rate =  await _rateConfigurationRepository.GetActiveRateAsync();

        if (rate is null){ throw new NotFoundException("Active rate configuration not found.");}

        decimal totalAmount = totalMinutes * rate.PricePerMinute;

        movement.ExitDate = exitDate;
        movement.TotalMinutes = totalMinutes;
        movement.TotalAmount = totalAmount;
        movement.Status = ParkingStatus.Closed;

        await _parkingMovementRepository.SaveChangesAsync();

        return new ExitResponseDto
        {
            Plate = movement.Vehicle.Plate,
            VehicleType = movement.Vehicle.VehicleType.Name,
            TotalMinutes = totalMinutes,
            TotalAmount = totalAmount
        };
    }
}

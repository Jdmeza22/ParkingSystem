using ParkingSystem.Application.DTOs;
using ParkingSystem.Application.Exceptions;
using ParkingSystem.Application.Interfaces.Repositories;
using ParkingSystem.Application.Interfaces.Services;
using ParkingSystem.Domain.Entities;
using ParkingSystem.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace ParkingSystem.Application.Services;

public class ParkingService( 
    IVehicleRepository _vehicleRepository, 
    IParkingMovementRepository _parkingMovementRepository,
    IRateConfigurationRepository _rateConfigurationRepository,
    IEmailService _emailService,
    ILogger<ParkingService> _logger) : IParkingService
{
    public async Task RegisterEntryAsync(RegisterEntryRequestDto request)
    {
        _logger.LogInformation("Registering vehicle entry for plate: {Plate}", request.Plate);
        ParkingMovement activeMovement =  await _parkingMovementRepository.GetActiveMovementByPlateAsync(request.Plate);

        if( activeMovement is not null) {
            _logger.LogWarning("Vehicle with plate {Plate} already has an active parking movement.", request.Plate);
            throw new BadRequestException(   "Vehicle already has an active parking movement." ); 
        }

        var vehicle = await _vehicleRepository.GetByPlateAsync(request.Plate);
        if (vehicle is null)
        {
            _logger.LogInformation("Vehicle with plate {Plate} does not exist. Creating new vehicle.",request.Plate);

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
        _logger.LogInformation("Vehicle entry registered successfully for plate: {Plate}", request.Plate);
    }

    public async Task<ExitResponseDto> RegisterExitAsync( RegisterExitRequestDto request)
    {
        _logger.LogInformation( "Registering vehicle exit for plate: {Plate}", request.Plate);
        ParkingMovement movement =  await _parkingMovementRepository.GetActiveMovementByPlateAsync(request.Plate);

        if (movement is null){
            _logger.LogWarning( "Vehicle with plate {Plate} does not have an active parking movement.", request.Plate);
            throw new NotFoundException("Vehicle does not have an active parking movement." ); 
        }

        DateTime exitDate = DateTime.UtcNow;
        int totalMinutes =(int)Math.Ceiling( (exitDate - movement.EntryDate).TotalMinutes );
        RateConfiguration rate =  await _rateConfigurationRepository.GetActiveRateAsync();

        if (rate is null){
            _logger.LogError( "Active rate configuration was not found.");
            throw new NotFoundException("Active rate configuration not found.");
        }

        decimal totalAmount = totalMinutes * rate.PricePerMinute;

        movement.ExitDate = exitDate;
        movement.TotalMinutes = totalMinutes;
        movement.TotalAmount = totalAmount;
        movement.Status = ParkingStatus.Closed;

        await _parkingMovementRepository.SaveChangesAsync();
        _logger.LogInformation( "Vehicle exit processed successfully for plate: {Plate}. TotalMinutes: {TotalMinutes}. TotalAmount: {TotalAmount}",  movement.Vehicle.Plate,totalMinutes, totalAmount);

        try
        {
            await _emailService.SendVehicleExitEmailAsync("cliente@test.com",  movement.Vehicle.Plate, movement.Vehicle.VehicleType.Name, totalMinutes, totalAmount );
            movement.EmailSent = true;
            await _parkingMovementRepository.SaveChangesAsync();
            _logger.LogInformation( "Email sent successfully for plate: {Plate}", movement.Vehicle.Plate);
        }
        catch (Exception ex)
        {
            movement.EmailSent = false;
            await _parkingMovementRepository.SaveChangesAsync();
            _logger.LogError( ex, "Failed to send email for plate: {Plate}",movement.Vehicle.Plate);
        }

        return new ExitResponseDto
        {
            Plate = movement.Vehicle.Plate,
            VehicleType = movement.Vehicle.VehicleType.Name,
            TotalMinutes = totalMinutes,
            TotalAmount = totalAmount
        };
    }
}

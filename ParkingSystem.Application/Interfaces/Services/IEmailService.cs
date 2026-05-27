namespace ParkingSystem.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendVehicleExitEmailAsync( string toEmail,string plate, string vehicleType, int totalMinutes, decimal totalAmount);
}

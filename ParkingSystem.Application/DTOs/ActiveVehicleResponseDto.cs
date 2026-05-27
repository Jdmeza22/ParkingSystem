namespace ParkingSystem.Application.DTOs;

public class ActiveVehicleResponseDto
{
    public string Plate { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public DateTime EntryDate { get; set; }
}

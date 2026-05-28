namespace ParkingSystem.Application.DTOs;

public class ExitResponseDto
{
    public string Plate { get; set; } = string.Empty;

    public string VehicleType { get; set; } = string.Empty;

    public int TotalMinutes { get; set; }

    public decimal TotalAmount { get; set; }
}

namespace ParkingSystem.Infrastructure.ExternalServices.DTOs;


public class TokenResponseDto
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}

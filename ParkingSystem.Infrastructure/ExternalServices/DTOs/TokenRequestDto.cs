
using System.Text.Json.Serialization;

namespace ParkingSystem.Infrastructure.ExternalServices.DTOs;

public class TokenRequestDto
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
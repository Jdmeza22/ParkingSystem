using Microsoft.Extensions.Options;
using ParkingSystem.Application.Exceptions;
using ParkingSystem.Application.Interfaces.Services;
using ParkingSystem.Infrastructure.Configurations;
using ParkingSystem.Infrastructure.ExternalServices.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ParkingSystem.Infrastructure.ExternalServices.Services;

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly EmailApiSettings _settings;

    public EmailService( HttpClient httpClient,IOptions<EmailApiSettings> options)
    {
        _httpClient = httpClient;
        _settings = options.Value;
    }

    public async Task SendVehicleExitEmailAsync( string toEmail, string plate,  string vehicleType, int totalMinutes, decimal totalAmount)
    {
        var token = await GetTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new SendEmailRequestDto
        {
            ConfigParams = new ConfigParams
            {
                IdUser = "ParkingSystem",
                IdMessage = Guid.NewGuid().ToString()
            },

            Receivers = new Receivers
            {
                EmailOrigin = "parking@test.com",
                To = [toEmail]
            },

            Email = new EmailContent
            {
                Subject = "Vehicle Exit Notification",

                Message = $@"
                    Vehicle Plate: {plate}
                    Vehicle Type: {vehicleType}
                    Total Minutes: {totalMinutes}
                    Total Amount: ${totalAmount}
                "
            }
        };

        var json = JsonSerializer.Serialize(request);

        var content = new StringContent(  json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync( $"{_settings.BaseUrl}/api/email/sendEmail", content);

        if (!response.IsSuccessStatusCode){throw new BadRequestException( "Failed to send email."); }
    }

    private async Task<string> GetTokenAsync()
    {
        var request = new TokenRequestDto
        {
            Username = _settings.Username,
            Password = _settings.Password
        };

        var json = JsonSerializer.Serialize(request);
        var content =new StringContent( json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync( $"{_settings.BaseUrl}/api/token", content);

        if (!response.IsSuccessStatusCode) {throw new UnauthorizedException( "Failed to authenticate email API.");}

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TokenResponseDto>( responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Token  ?? throw new UnauthorizedException( "Invalid token response.");
    }
}

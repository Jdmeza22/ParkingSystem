using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParkingSystem.Application.Exceptions;
using ParkingSystem.Application.Interfaces.Services;
using ParkingSystem.Infrastructure.Configurations;
using ParkingSystem.Infrastructure.ExternalServices.DTOs;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ParkingSystem.Infrastructure.ExternalServices.Services;

public class EmailService(
    ILogger<EmailService> _logger,
    HttpClient _httpClient,
    IOptions<EmailApiSettings> _options) : IEmailService
{
    
    public async Task SendVehicleExitEmailAsync( string toEmail, string plate,  string vehicleType, int totalMinutes, decimal totalAmount)
    {
        _logger.LogInformation("Sending vehicle exit email for plate: {Plate}", plate);
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
        var response = await _httpClient.PostAsync( $"{_options.Value.BaseUrl}/api/email/sendEmail", content);

        if (!response.IsSuccessStatusCode){
            _logger.LogError( "Failed to send vehicle exit email for plate: {Plate}", plate);
            throw new BadRequestException( "Failed to send email.");
        }

        _logger.LogInformation( "Vehicle exit email sent successfully for plate: {Plate}", plate);
    }

    private async Task<string> GetTokenAsync()
    {
        _logger.LogInformation( "Authenticating against external email API.");

        var request = new TokenRequestDto
        {
            Username = _options.Value.Username,
            Password = _options.Value.Password
        };

        var json = JsonSerializer.Serialize(request);
        var content =new StringContent( json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync( $"{_options.Value.BaseUrl}/api/token", content);

        if (!response.IsSuccessStatusCode) {
            _logger.LogError( "Failed to authenticate against external email API. Response: {Response}", response);
            throw new UnauthorizedException( "Failed to authenticate email API.");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<TokenResponseDto>( responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (string.IsNullOrWhiteSpace(result?.Token))
        {
            _logger.LogError("External email API returned an invalid token response.");

            throw new UnauthorizedException("Invalid token response.");
        }

        _logger.LogInformation("Successfully authenticated against external email API.");

        return result.Token ;
    }
}

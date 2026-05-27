
namespace ParkingSystem.Infrastructure.ExternalServices.DTOs;

public class SendEmailRequestDto
{
    public ConfigParams ConfigParams { get; set; } = new();
    public Receivers Receivers { get; set; } = new();
    public EmailContent Email { get; set; } = new();
}

public class ConfigParams
{
    public string IdUser { get; set; } = string.Empty;
    public string IdMessage { get; set; } = string.Empty;
}

public class Receivers
{
    public string EmailOrigin { get; set; } = string.Empty;
    public List<string> To { get; set; } = [];
    public List<string> CopyTo { get; set; } = [];
    public List<string> HiddenCopyTo { get; set; } = [];
}

public class EmailContent
{
    public string Subject { get; set; } = string.Empty;
    public string UrlHeader { get; set; } = string.Empty;
    public string UrlFooter { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<string> UrlFiles { get; set; } = [];
}

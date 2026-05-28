using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingSystem.Application.DTOs;

public class RegisterEntryRequestDto
{
    public string Plate { get; set; } = string.Empty;

    public int VehicleTypeId { get; set; }
}

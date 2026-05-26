using ParkingSystem.Domain.Common;

namespace ParkingSystem.Domain.Entities;

public class Vehicle  : BaseEntity
{
    public long Id { get; set; }
    public string Plate { get; set; } = string.Empty;
    public int VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; } = default!;
    public ICollection<ParkingMovement> ParkingMovements { get; set; } = [];
}

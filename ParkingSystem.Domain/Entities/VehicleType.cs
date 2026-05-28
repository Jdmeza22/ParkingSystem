using ParkingSystem.Domain.Common;
namespace ParkingSystem.Domain.Entities;

public class VehicleType : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Vehicle> Vehicles { get; set; } = [];
}
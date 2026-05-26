using ParkingSystem.Domain.Common;
namespace ParkingSystem.Domain.Entities;

public class RateConfiguration : BaseEntity
{
    public int Id { get; set; }
    public decimal PricePerMinute { get; set; }
    public bool Active { get; set; }
}

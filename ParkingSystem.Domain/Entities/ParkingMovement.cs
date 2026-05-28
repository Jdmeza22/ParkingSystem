
using ParkingSystem.Domain.Enums;

namespace ParkingSystem.Domain.Entities;

public class ParkingMovement
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = default!;
    public DateTime EntryDate { get; set; }
    public DateTime? ExitDate { get; set; }
    public int? TotalMinutes { get; set; }
    public decimal? TotalAmount { get; set; }
    public ParkingStatus Status { get; set; }
    public bool EmailSent { get; set; }
}

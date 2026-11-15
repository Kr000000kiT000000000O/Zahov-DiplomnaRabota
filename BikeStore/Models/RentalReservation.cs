using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStore.Models;
public enum RentalStatus { Pending = 0, Approved = 1, Active = 2, Returned = 3, Cancelled = 4 }
public class RentalReservation
{
    public int Id { get; set; }
    public int BicycleId { get; set; }
    public Bicycle Bicycle { get; set; } = null!;
    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
    public RentalStatus Status { get; set; } = RentalStatus.Pending;
}

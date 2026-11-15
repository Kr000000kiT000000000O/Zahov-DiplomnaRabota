using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStore.Models;
public class Bicycle
{
    public int Id { get; set; }
    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(80)]
    public string Brand { get; set; } = string.Empty;
    [StringLength(1500)]
    public string? Description { get; set; }
    [Range(0.01, 1000000)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SalePrice { get; set; }
    [Range(0.01, 100000)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyRentalPrice { get; set; }
    [Range(0, 100000)]
    public int StockQuantity { get; set; }
    public bool CanBeRented { get; set; }
    [StringLength(500)]
    public string? ImageUrl { get; set; }
    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<RentalReservation> Reservations { get; set; } = new List<RentalReservation>();
}

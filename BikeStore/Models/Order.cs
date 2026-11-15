using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeStore.Models;
public enum OrderStatus { New = 0, Processing = 1, Completed = 2, Cancelled = 3 }
public class Order
{
    public int Id { get; set; }
    [Required]
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.New;
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

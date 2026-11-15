using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BikeStore.Models;
public class ApplicationUser : IdentityUser
{
    [StringLength(100)]
    public string? FullName { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<RentalReservation> RentalReservations { get; set; } = new List<RentalReservation>();
}

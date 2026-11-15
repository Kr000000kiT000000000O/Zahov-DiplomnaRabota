using System.ComponentModel.DataAnnotations;

namespace BikeStore.Models;
public class Category
{
    public int Id { get; set; }
    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;
    [StringLength(500)]
    public string? Description { get; set; }
    public ICollection<Bicycle> Bicycles { get; set; } = new List<Bicycle>();
}

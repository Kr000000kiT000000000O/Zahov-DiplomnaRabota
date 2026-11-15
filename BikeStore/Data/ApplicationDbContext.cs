using BikeStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Data;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Bicycle> Bicycles => Set<Bicycle>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<RentalReservation> RentalReservations => Set<RentalReservation>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Bicycle>().HasOne(x => x.Category).WithMany(x => x.Bicycles)
            .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Bicycle>().HasIndex(x => new { x.CategoryId, x.Name });
        builder.Entity<Bicycle>().ToTable(t =>
        {
            t.HasCheckConstraint("CK_Bicycles_StockQuantity", "[StockQuantity] >= 0");
            t.HasCheckConstraint("CK_Bicycles_SalePrice", "[SalePrice] > 0");
        });
        builder.Entity<Order>().HasOne(x => x.User).WithMany(x => x.Orders)
            .HasForeignKey(x => x.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<OrderItem>().HasOne(x => x.Order).WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<OrderItem>().HasOne(x => x.Bicycle).WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.BicycleId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<RentalReservation>().HasOne(x => x.User).WithMany(x => x.RentalReservations)
            .HasForeignKey(x => x.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<RentalReservation>().HasOne(x => x.Bicycle).WithMany(x => x.Reservations)
            .HasForeignKey(x => x.BicycleId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<RentalReservation>().ToTable(t => t.HasCheckConstraint(
            "CK_Rentals_Dates", "[EndDate] > [StartDate]"));
        builder.Entity<RentalReservation>().HasIndex(x => new { x.BicycleId, x.StartDate, x.EndDate });
    }
}

using BikeStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Data;
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration config)
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await db.Database.EnsureCreatedAsync();

        foreach (var role in new[] { "Client", "Employee", "Admin" })
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        if (!await db.Categories.AnyAsync())
        {
            var mountain = new Category { Name = "Планински", Description = "Велосипеди за неравни терени" };
            var city = new Category { Name = "Градски", Description = "Велосипеди за градско каране" };
            var road = new Category { Name = "Шосейни", Description = "Велосипеди за асфалт" };
            var electric = new Category { Name = "Електрически", Description = "Електрически велосипеди" };
            db.Categories.AddRange(mountain, city, road, electric);
            db.Bicycles.AddRange(
                new Bicycle { Name = "Trail 29", Brand = "DemoBike", Category = mountain,
                    SalePrice = 950m, DailyRentalPrice = 28m, StockQuantity = 5,
                    CanBeRented = true, Description = "Примерен планински велосипед; не е реална оферта." },
                new Bicycle { Name = "City Move", Brand = "DemoBike", Category = city,
                    SalePrice = 650m, DailyRentalPrice = 20m, StockQuantity = 4,
                    CanBeRented = true, Description = "Примерен градски велосипед; не е реална оферта." },
                new Bicycle { Name = "Road Speed", Brand = "DemoBike", Category = road,
                    SalePrice = 1250m, DailyRentalPrice = 35m, StockQuantity = 2,
                    CanBeRented = false, Description = "Примерен шосеен велосипед; не е реална оферта." }
            );
            await db.SaveChangesAsync();
        }

        var email = config["Bootstrapping:AdminEmail"];
        var password = config["Bootstrapping:AdminPassword"];
        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
        {
            var admin = await userManager.FindByEmailAsync(email);
            if (admin is null)
            {
                admin = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(admin, password);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Admin bootstrap error: " +
                        string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var result = await userManager.AddToRoleAsync(admin, "Admin");
                if (!result.Succeeded) throw new InvalidOperationException("Cannot assign Admin role.");
            }
        }
    }
}

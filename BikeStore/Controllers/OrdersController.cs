using BikeStore.Data;
using BikeStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Controllers;
[Authorize]
public sealed class OrdersController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    public OrdersController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db; _userManager = userManager;
    }
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        bool canSeeAll = User.IsInRole("Admin") || User.IsInRole("Employee");
        var query = _db.Orders.AsNoTracking().AsQueryable();
        if (!canSeeAll)
        {
            var userId = _userManager.GetUserId(User);
            query = query.Where(x => x.ApplicationUserId == userId);
        }
        return View(await query.OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct));
    }
}

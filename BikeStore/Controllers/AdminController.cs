using BikeStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Controllers;
[Authorize(Roles = "Admin,Employee")]
public sealed class AdminController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdminController(ApplicationDbContext db) => _db = db;
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["BikeCount"] = await _db.Bicycles.CountAsync(ct);
        ViewData["OrderCount"] = await _db.Orders.CountAsync(ct);
        ViewData["RentalCount"] = await _db.RentalReservations.CountAsync(ct);
        return View();
    }
}

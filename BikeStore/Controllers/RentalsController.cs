using BikeStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers;
public sealed class RentalsController : Controller
{
    private readonly IBicycleCatalogService _catalog;
    public RentalsController(IBicycleCatalogService catalog) => _catalog = catalog;
    public async Task<IActionResult> Index(CancellationToken ct) =>
        View(await _catalog.GetRentableAsync(ct));
    // Планирано за следващ етап: резервация с проверка за припокриващи се периоди.
}

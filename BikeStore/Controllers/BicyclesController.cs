using BikeStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.Controllers;
public sealed class BicyclesController : Controller
{
    private readonly IBicycleCatalogService _catalog;
    public BicyclesController(IBicycleCatalogService catalog) => _catalog = catalog;
    public async Task<IActionResult> Index(string? search, int? categoryId, CancellationToken ct)
    {
        ViewData["Search"] = search;
        return View(await _catalog.SearchAsync(search, categoryId, ct));
    }
    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var bicycle = await _catalog.GetByIdAsync(id, ct);
        return bicycle is null ? NotFound() : View(bicycle);
    }
}

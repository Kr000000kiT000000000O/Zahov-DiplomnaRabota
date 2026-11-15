using BikeStore.Data;
using BikeStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Services;
public sealed class BicycleCatalogService : IBicycleCatalogService
{
    private readonly ApplicationDbContext _db;
    public BicycleCatalogService(ApplicationDbContext db) => _db = db;

    public async Task<IReadOnlyList<Bicycle>> SearchAsync(string? search, int? categoryId, CancellationToken ct = default)
    {
        IQueryable<Bicycle> query = _db.Bicycles.AsNoTracking().Include(x => x.Category);
        if (!string.IsNullOrWhiteSpace(search))
        {
            string keyword = search.Trim();
            query = query.Where(x => x.Name.Contains(keyword) || x.Brand.Contains(keyword));
        }
        if (categoryId.HasValue)
            query = query.Where(x => x.CategoryId == categoryId.Value);
        return await query.OrderBy(x => x.Name).ToListAsync(ct);
    }
    public Task<Bicycle?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Bicycles.AsNoTracking().Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Bicycle>> GetRentableAsync(CancellationToken ct = default) =>
        await _db.Bicycles.AsNoTracking().Include(x => x.Category)
            .Where(x => x.CanBeRented && x.StockQuantity > 0)
            .OrderBy(x => x.Name).ToListAsync(ct);
}

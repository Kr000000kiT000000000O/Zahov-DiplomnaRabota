using BikeStore.Models;

namespace BikeStore.Services;
public interface IBicycleCatalogService
{
    Task<IReadOnlyList<Bicycle>> SearchAsync(string? search, int? categoryId, CancellationToken ct = default);
    Task<Bicycle?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<Bicycle>> GetRentableAsync(CancellationToken ct = default);
}

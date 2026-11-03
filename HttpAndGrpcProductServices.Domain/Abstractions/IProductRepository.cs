using HttpAndGrpcProductServices.Domain.Enums;
using HttpAndGrpcProductServices.Domain.Models;

namespace HttpAndGrpcProductServices.Domain.Abstractions
{
    public interface IProductRepository
    {
        Task<Guid> AddAsync(Product product, CancellationToken ct = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<(IReadOnlyList<Product> Items, int TotalCount)> GetFilteredAsync(
            DateTime? createdFrom,
            DateTime? createdTo,
            ProductType? type,
            int? warehouseNumber,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default);
    }
}

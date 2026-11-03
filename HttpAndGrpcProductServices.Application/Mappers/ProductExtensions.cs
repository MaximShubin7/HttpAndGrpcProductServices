using HttpAndGrpcProductServices.Application.Dtos;
using HttpAndGrpcProductServices.Domain.Models;

namespace HttpAndGrpcProductServices.Application.Mappers
{
    public static class ProductExtensions
    {
        public static ProductDto MapToDto(this Product p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Weight = p.Weight,
            Type = (int)p.Type,
            CreatedAt = p.CreatedAt,
            WarehouseNumber = p.WarehouseNumber
        };
    }
}

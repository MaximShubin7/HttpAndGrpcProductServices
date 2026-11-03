using FluentValidation;
using HttpAndGrpcProductServices.Application.Abstractions;
using HttpAndGrpcProductServices.Application.Dtos;
using HttpAndGrpcProductServices.Application.Mappers;
using HttpAndGrpcProductServices.Domain.Abstractions;
using HttpAndGrpcProductServices.Domain.Enums;
using HttpAndGrpcProductServices.Domain.Models;

namespace HttpAndGrpcProductServices.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly IValidator<GetProductsFilterDto> _filterValidator;

        public ProductService(
            IProductRepository repository,
            IValidator<CreateProductDto> createValidator,
            IValidator<GetProductsFilterDto> filterValidator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _filterValidator = filterValidator;
        }

        public async Task<Guid> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
        {
            var validationResult = await _createValidator.ValidateAsync(dto, ct);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Price = dto.Price,
                Weight = dto.Weight,
                Type = (ProductType)dto.Type,
                CreatedAt = DateTime.UtcNow,
                WarehouseNumber = dto.WarehouseNumber
            };

            return await _repository.AddAsync(product, ct);
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var product = await _repository.GetByIdAsync(id, ct);
            if (product is null) return null;

            return product.MapToDto();
        }

        public async Task<(IReadOnlyList<ProductDto> Items, int TotalCount)> GetFilteredAsync(
            GetProductsFilterDto filter, CancellationToken ct = default)
        {
            var validationResult = await _filterValidator.ValidateAsync(filter, ct);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var (items, total) = await _repository.GetFilteredAsync(
                filter.CreatedFrom,
                filter.CreatedTo,
                filter.Type.HasValue ? (ProductType)filter.Type.Value : null,
                filter.WarehouseNumber,
                filter.PageNumber,
                filter.PageSize,
                ct);

            return (items.Select(x => x.MapToDto()).ToList(), total);
        }
    }
}

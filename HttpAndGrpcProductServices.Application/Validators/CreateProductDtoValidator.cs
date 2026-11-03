using FluentValidation;
using HttpAndGrpcProductServices.Application.Dtos;

namespace HttpAndGrpcProductServices.Application.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название товара обязательно")
                .MaximumLength(200);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше 0");

            RuleFor(x => x.Weight)
                .GreaterThan(0).WithMessage("Вес должен быть больше 0");

            RuleFor(x => x.Type)
                .InclusiveBetween(0, 3).WithMessage("Вид товара должен быть от 0 до 3");

            RuleFor(x => x.WarehouseNumber)
                .GreaterThan(0).WithMessage("Номер склада должен быть больше 0");
        }
    }
}

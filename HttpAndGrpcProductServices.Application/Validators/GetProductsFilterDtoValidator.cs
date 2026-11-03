using FluentValidation;
using HttpAndGrpcProductServices.Application.Dtos;

namespace HttpAndGrpcProductServices.Application.Validators
{
    public class GetProductsFilterDtoValidator : AbstractValidator<GetProductsFilterDto>
    {
        public GetProductsFilterDtoValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Номер страницы должен быть больше 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Размер страницы должен быть от 1 до 100");

            RuleFor(x => x.CreatedFrom)
                .LessThanOrEqualTo(x => x.CreatedTo)
                .When(x => x.CreatedFrom.HasValue && x.CreatedTo.HasValue)
                .WithMessage("Дата начала не может быть позже даты окончания");

            RuleFor(x => x.Type)
                .InclusiveBetween(0, 3)
                .When(x => x.Type.HasValue)
                .WithMessage("Вид товара должен быть от 0 до 3");
        }
    }
}

using HttpAndGrpcProductServices.Application.Abstractions;
using HttpAndGrpcProductServices.Application.Services;
using HttpAndGrpcProductServices.Application.Validators;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace HttpAndGrpcProductServices.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();
            return services;
        }
    }
}

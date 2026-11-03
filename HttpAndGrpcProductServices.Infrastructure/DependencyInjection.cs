using HttpAndGrpcProductServices.Domain.Abstractions;
using HttpAndGrpcProductServices.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HttpAndGrpcProductServices.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IProductRepository, ProductRepository>();
            return services;
        }
    }
}

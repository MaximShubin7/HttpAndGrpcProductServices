using HttpAndGrpcProductServices.Application;
using HttpAndGrpcProductServices.Infrastructure;
using HttpAndGrpcProductServices.Grpc;
using HttpAndGrpcProductServices.Http;

namespace HttpAndGrpcProductServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            builder.Services.AddHttpPresentation();
            builder.Services.AddGrpcPresentation();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/http/swagger.json", "Product HTTP API");
                c.SwaggerEndpoint("/swagger/grpc/swagger.json", "Product gRPC transcoding API");
            });

            app.UseHttpPresentation();
            app.UseGrpcPresentation();

            app.Run();
        }
    }
}

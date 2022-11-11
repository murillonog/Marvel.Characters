using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Marvel.Characters.Infra.IoC
{
    public static class DependencyInjectionSwagger
    {
        public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Marvel Characters API", Version = "v1" });
            });

            return services;
        }
    }
}

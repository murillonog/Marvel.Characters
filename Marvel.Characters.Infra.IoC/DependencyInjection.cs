using AutoMapper;
using Marvel.Characters.Application.Interfaces;
using Marvel.Characters.Application.Mappings;
using Marvel.Characters.Application.Services;
using Marvel.Characters.Domain.Interfaces;
using Marvel.Characters.Infra.Data.Context;
using Marvel.Characters.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marvel.Characters.Infra.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                , b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<ICharacterService, CharacterService>();

            services.AddScoped<ICharacterRepository, CharacterRepository>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new DomainToDTOMappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}

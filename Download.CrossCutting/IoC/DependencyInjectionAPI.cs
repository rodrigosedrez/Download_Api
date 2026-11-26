using Download.Domain.Interfaces;
using Download.Application.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Download.Infrastructure.Repositories;

namespace Download.CrossCutting.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddDependencyInjectionAPI(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IEmulatorRepository, EmulatorRepository>();
            services.AddScoped<IExternalLinksRepository, ExternalLinksRepository>();
            services.AddScoped<IGameCategoryRepository, GameCategoryRepository>();
            services.AddScoped<IGameModelRepository, GameModelRepository>();
            services.AddScoped<IMagazineRepository, MagazineRepository>();
            services.AddScoped<IPlataformRepository, PlataformRepository>();
            services.AddScoped<IPostModelRepository, PostModelRepository>();
            services.AddScoped<IPublicityRepository, PublicityRepository>();
            services.AddScoped<IGameTagModelRepository, GameTagModelRepository>();

            services.AddAutoMapper(typeof(DomainToDTOProfile));

            return services;
        }
    }
}

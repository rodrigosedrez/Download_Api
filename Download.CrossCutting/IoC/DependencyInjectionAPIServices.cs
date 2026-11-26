
using Download.Application.Interfaces;
using Download.Application.Services;
using Download.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dwonload.CrossCutting.IoC
{

    public static class DependencyInjectionAPIServices
    {
        public static IServiceCollection AddDependencyInjectionAPIServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<DownloadsDBContext>(options =>
              options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")) );


            services.AddScoped<IEmulatorService, EmulatorService>();
            services.AddScoped<IExternalLinksService, ExternalLinksService>();
            services.AddScoped<IGameCategoryService, GameCategoryService>();
            services.AddScoped<IGameModelService, GameModelService>();
            services.AddScoped<IMagazineService, MagazineService>();
            services.AddScoped<IPlataformService, PlataformService>();
            services.AddScoped<IPostModelService, PostModelService>();
            services.AddScoped<IPublicityService, PublicityService>();
            services.AddSingleton<ICloudflareService,CloudflareService>();
            services.AddScoped<IGameTagsService, GameTagsService>();

            return services;

        }
    }
}

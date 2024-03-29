using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApplicationAPI.Interfaces.Repositories;
using WebApplicationAPI.Interfaces.Services;
using WebApplicationAPI.Repositories;
using WebApplicationAPI.Services;

namespace WebApplicationAPI.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApiExtensions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IStackOverflowApiService, StackOverflowApiService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddDbContext<WebApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetSection("ConnectionString").Value));
        }
    }
}

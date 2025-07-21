using Dispatcher.DAL.EF;
using Dispatcher.DAL.Repositories;
using Dispatcher.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dispatcher.DAL.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}

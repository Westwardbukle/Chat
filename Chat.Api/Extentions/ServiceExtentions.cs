using Chat.Database.Repository.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Chat.Extentions
{
    public static class ServiceExtentions
    {
        public static void ConfigureRepositoryManager(this IServiceCollection services)
            => services.AddScoped<IRepositoryManager, RepositoryManager>();
        
        /*public static void ConfigureServiceManager(this IServiceCollection services) 
            =>services.AddScoped<>()*/
    }
}
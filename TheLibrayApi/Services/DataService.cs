using LibraryTools;

namespace TheLibrayApi.Services
{
    public static  class DataService
    {
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            services.AddTransient<ILibraryDataAsceses>(sp =>

   ActivatorUtilities.CreateInstance<LibraryDataAsceses>(sp, "LibraryDb"));
            

            return services;
        }
    }
}

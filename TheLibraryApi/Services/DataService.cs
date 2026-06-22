using LibraryTools;

namespace TheLibraryApi.Services
{
    public static  class DataService
    {
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            services.AddTransient<ILibraryDataAcsees>(sp =>

   ActivatorUtilities.CreateInstance<LibraryDataAsceses>(sp, "LibraryDb"));
            

            return services;
        }
    }
}

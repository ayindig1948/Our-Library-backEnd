namespace TheLibraryApi.Services
{
    public static class ValidationService
    {
        public static void AddValidationServices(this IServiceCollection services)
        {
            services.AddValidation();
        }
    }
}

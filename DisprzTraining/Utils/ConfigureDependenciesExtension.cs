using DisprzTraining.Business;
using DisprzTraining.DataAccess;


namespace DisprzTraining.Utils
{
    public static class ConfigureDependenciesExtension
    {
        public static void ConfigureDependencyInjections(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<IAppointmentBL, AppointmentBL>();
            services.AddScoped<IAppointmentDAL, AppointmentDAL>();
        }
    }
}

using Hangfire;
using SampleApplication.Filters;

namespace SampleApplication.Extensions;

public static class AppExtensions
{
    public static void AddBaseApplication(this IApplicationBuilder app)
    {
        app.HangfireExtensions();
    }

    private static void HangfireExtensions(this IApplicationBuilder app)
    {
        // Configure o Hangfire Dashboard
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() } // Opcional: Adicione autorização ao Dashboard
        });
    }
}

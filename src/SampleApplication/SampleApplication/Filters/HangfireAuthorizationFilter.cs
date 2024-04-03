using Hangfire.Dashboard;

namespace SampleApplication.Filters;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
        // Aqui você pode implementar a lógica de autorização conforme necessário
        // Por exemplo, verifique se o usuário está autenticado e tem permissão para acessar o dashboard

        // Verifique se o usuário está autenticado
        var user = context.GetHttpContext().User;

        if(user.Identity is null)
            return false;

        if (!user.Identity.IsAuthenticated)
            return false;
        
        // Verifique se o usuário possui o papel "Admin"
        return user.IsInRole("Admin");
    }
}
